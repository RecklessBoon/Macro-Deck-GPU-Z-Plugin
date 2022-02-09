using RecklessBoon.MacroDeck.GPUZ.Actions;
using RecklessBoon.MacroDeck.GPUZ.UI;
using SuchByte.MacroDeck.Logging;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Variables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecklessBoon.MacroDeck.GPUZ
{
    public static class PluginInstance
    {
        public static GPUZPlugin Plugin;
        public static Configuration Configuration;
    }

    public class GPUZPlugin : MacroDeckPlugin
    {
        // A short description what the plugin can do
        public override string Description => "GPU-Z plugin";

        // You can add a image from your resources here
        public override System.Drawing.Image Icon => null;

        // Optional; If your plugin can be configured, set to "true". It'll make the "Configure" button appear in the package manager.
        public override bool CanConfigure => true;

        public bool ShouldUpdateVariables = true;

        public Task grabbingInfoTask = null;

        public int GPUZ_PID = -1;

        public CancellationTokenSource cts;
        public CancellationToken cancellationToken;

        public class VariableState
        {
            public string Name { get; set; }
            protected VariableType _type = VariableType.Bool;
            public VariableType Type { get { return _type; } set { _type = value; } }
            protected object _value = false;
            public object Value { get { return _value; } set { _value = value; } }
            protected bool _save = true;
            public bool Save { get { return _save; } set { _save = value; } }

        }

        public void SetVariable(VariableState variableState)
        {
            VariableManager.SetValue(string.Format("gpu_z_{0}", variableState.Name), variableState.Value, variableState.Type, this, variableState.Save);
        }

        public string GetVariable(string key)
        {
            var name = String.Format("gpu_z_{0}", key);
            return VariableManager.Variables.Find(x => x.Name == name).Value;
        }

        public void SetVariable(VariableState[] variableStates)
        {
            foreach (VariableState state in variableStates)
            {
                SetVariable(state);
            }
        }

        public void ResetVariables()
        {
            Task.Run(() =>
            {
                var pluginVars = VariableManager.Variables.FindAll(x => x.Creator == "GPU_Z Plugin");
                foreach(var variable in pluginVars)
                {
                    VariableType type = Enum.Parse<VariableType>(variable.Type);
                    switch (type)
                    {
                        case VariableType.Integer:
                        case VariableType.Float:
                            VariableManager.SetValue(variable.Name, -1, type, this);
                            break;
                        case VariableType.String:
                        default:
                            VariableManager.SetValue(variable.Name, "", type, this);
                            break;
                    }

                }
            });
        }

        public GPUZPlugin()
        {
            PluginInstance.Plugin ??= this;
            PluginInstance.Configuration ??= new Configuration(this);
            cts = new CancellationTokenSource();
            cancellationToken = cts.Token;
        }

        // Gets called when the plugin is loaded
        public override void Enable()
        {
            BeginWatch();

            Actions = new List<PluginAction>()
            {
                new RefreshVariablesAction()
            };
        }

        public async Task RestartWatcher()
        {
            cts.Cancel();
            await grabbingInfoTask.ConfigureAwait(true);
            cts = new CancellationTokenSource();
            cancellationToken = cts.Token;
            BeginWatch();
        }

        protected void BeginWatch()
        {
            if (grabbingInfoTask == null || grabbingInfoTask.Status != TaskStatus.Running)
            {
                grabbingInfoTask = Task.Run(() =>
                {
                    while (ShouldUpdateVariables && !cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            using var mmf = MemoryMappedFile.OpenExisting("GPUZShMem");
                            using var accessor = mmf.CreateViewAccessor();

                            int shMemSize = Marshal.SizeOf(typeof(GPUZ_SH_MEM));
                            int recordSize = Marshal.SizeOf(typeof(GPUZ_RECORD));
                            int recordArraySize = recordSize * 128;
                            int sensorSize = Marshal.SizeOf(typeof(GPUZ_SENSOR_RECORD));
                            int sensorArraySize = sensorSize * 128;

                            GPUZ_SH_MEM mem;
                            List<GPUZ_RECORD> data = new List<GPUZ_RECORD>();
                            List<GPUZ_SENSOR_RECORD> sensors = new List<GPUZ_SENSOR_RECORD>();

                            // Meta data
                            accessor.Read(0, out mem);

                            // System data
                            for (var i = 0; i < recordArraySize; i += recordSize)
                            {
                                GPUZ_RECORD record;
                                byte[] buffer = new byte[256];
                                accessor.ReadArray<byte>(shMemSize + i, buffer, 0, 256);
                                record.key = Encoding.Unicode.GetString(buffer).Trim('\0');
                                buffer = new byte[256];
                                accessor.ReadArray<byte>(shMemSize + i + 512, buffer, 0, 256);
                                record.value = Encoding.Unicode.GetString(buffer).Trim('\0');

                                SetVariable(new VariableState { Name = record.key, Value = record.value, Type = VariableType.String });

                                data.Add(record);
                            }

                            // Sensor data
                            for (var i = 0; i < sensorArraySize; i += sensorSize)
                            {
                                GPUZ_SENSOR_RECORD record;
                                byte[] buffer = new byte[256];
                                var position = shMemSize + recordArraySize + i;

                                accessor.ReadArray<byte>(position, buffer, 0, 256);
                                position += 512;
                                record.name = Encoding.Unicode.GetString(buffer).Trim('\0');

                                buffer = new byte[8];
                                accessor.ReadArray<byte>(position, buffer, 0, 8);
                                position += 16;
                                record.unit = Encoding.Unicode.GetString(buffer).Trim('\0');

                                record.digits = accessor.ReadUInt32(position);
                                position += Marshal.SizeOf(typeof(UInt32));
                                record.value = accessor.ReadDouble(position);

                                SetVariable(new VariableState { Name = record.name + "_unit", Value = record.unit, Type = VariableType.String });
                                SetVariable(new VariableState { Name = record.name + "_digits", Value = (int)record.digits, Type = VariableType.Integer });
                                SetVariable(new VariableState { Name = record.name + "_value", Value = (float)record.value, Type = VariableType.Float });

                                sensors.Add(record);
                            }

                            Thread.Sleep(PluginInstance.Configuration.PollingFrequency*1000);
                        }
                        catch (FileNotFoundException)
                        {
                            MacroDeckLogger.Info(PluginInstance.Plugin, "GPU-Z shared memory not available. Sleeping 30 seconds before trying again...");
                            ResetVariables();
                            Thread.Sleep(30000);
                        }
                        catch (Exception ex)
                        {
                            MacroDeckLogger.Info(PluginInstance.Plugin,
                                                    String.Format("Failed to parse GPU-Z shared memory. Sleeping 60 seconds before trying again...\nException thrown: {0}",
                                                                  ex.ToString()));
                            ResetVariables();
                            Thread.Sleep(60000);
                        }
                    }
                }, cancellationToken);
            }
        }

        /**
         * Pad a bitmap with default padding
         */
        private Bitmap PadBitmap(Bitmap bm)
        {
            return PadBitmap(bm, 1.3f, 1.3f);
        }

        /**
         * Pad a bitmap with equal percentage on x and y axis
         */
        private Bitmap PadBitmap(Bitmap bm, float padding)
        {
            return PadBitmap(bm, padding, padding);
        }

        /**
         * Pad a bitmap with explicit percentage on x and y axis
         */
        private Bitmap PadBitmap(Bitmap bm, float xPadding, float yPadding)
        {
            Bitmap paddedBm = new Bitmap((int)(bm.Width * xPadding), (int)(bm.Height * yPadding));
            using (Graphics graphics = Graphics.FromImage(paddedBm))
            {
                graphics.Clear(Color.Transparent);
                int x = (paddedBm.Width - bm.Width) / 2;
                int y = (paddedBm.Height - bm.Height) / 2;
                graphics.DrawImage(bm, x, y, bm.Width, bm.Height);
            }
            return paddedBm;
        }

        // Optional; Gets called when the user clicks on the "Configure" button in the package manager; If CanConfigure is not set to true, you don't need to add this
        public override void OpenConfigurator()
        {
            // Open your configuration form here
            using var configurator = new PluginConfigForm(PluginInstance.Configuration);
            configurator.ShowDialog();
        }
    }
}
