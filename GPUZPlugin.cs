using RecklessBoon.MacroDeck.GPUZ.Actions;
using RecklessBoon.MacroDeck.GPUZ.UI;
using SuchByte.MacroDeck.Logging;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Variables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace RecklessBoon.MacroDeck.GPUZ
{
    public static class PluginInstance
    {
        public static GPUZPlugin Plugin;
        public static Configuration Configuration;
        public static SharedMemory GPUZ;
    }

    public class GPUZPlugin : MacroDeckPlugin
    {
        // A short description what the plugin can do
        public override string Description => "GPU-Z plugin";

        // You can add a image from your resources here
        public override Image Icon => Properties.Resources.Macro_Deck_GPU_Z_Icon;

        // Optional; If your plugin can be configured, set to "true". It'll make the "Configure" button appear in the package manager.
        public override bool CanConfigure => true;

        public bool ShouldUpdateVariables = true;

        public Task grabbingInfoTask = null;

        protected bool saveVarsInDB = true;

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

        public static string FormatName(string name)
        {
            return Regex.Replace(name, @"[^a-zA-Z0-9]", "_", RegexOptions.Multiline);
        }

        public void SetVariable(VariableState variableState)
        {
            var formattedName = GPUZPlugin.FormatName(variableState.Name);
            VariableManager.SetValue(string.Format("gpu_z_{0}", formattedName), variableState.Value, variableState.Type, this, variableState.Save);
        }

        public string GetVariable(string key)
        {
            var name = String.Format("gpu_z_{0}", GPUZPlugin.FormatName(key));
            return VariableManager.Variables.Find(x => x.Name == name).Value;
        }

        public void SetVariable(VariableState[] variableStates)
        {
            foreach (VariableState state in variableStates)
            {
                SetVariable(state);
            }
        }

        public void DeleteVariable(string key)
        {
            DeleteVariable(key, "");
        }

        public void DeleteVariable(string key, string postfix)
        {
            VariableManager.DeleteVariable(String.Format("gpu_z_{0}{1}", GPUZPlugin.FormatName(key), postfix));
        }

        public void RemoveVariables()
        {
            Task.Run(() =>
            {
                VariableManager.Variables.FindAll(x => x.Creator == "GPU_Z Plugin").ForEach(delegate (Variable variable)
                {
                    var isWhitelisted = !String.IsNullOrWhiteSpace(PluginInstance.Configuration.VariableWhitelist.Find((x) =>
                    {
                        x = GPUZPlugin.FormatName(x).ToLower();
                        string[] matches = new string[]
                        {
                            String.Format("gpu_z_{0}", x),
                            String.Format("gpu_z_{0}_unit", x),
                            String.Format("gpu_z_{0}_digits", x),
                            String.Format("gpu_z_{0}_value", x)
                        };
                        return matches.Contains(variable.Name);
                    }));
                    if (!isWhitelisted)
                    {
                        VariableManager.DeleteVariable(variable.Name);
                    }
                });
            });
        }

        public GPUZPlugin()
        {
            PluginInstance.Plugin ??= this;

            cts = new CancellationTokenSource();
            cancellationToken = cts.Token;
        }

        // Gets called when the plugin is loaded
        public override void Enable()
        {
            PluginInstance.Configuration ??= new Configuration(this);
            PluginInstance.GPUZ ??= new SharedMemory();
            var failureLogged = false;
            PluginInstance.GPUZ.OnDataUpdated += (sender, record) =>
            {
                var isWhitelisted = !String.IsNullOrWhiteSpace(PluginInstance.Configuration.VariableWhitelist.Find(x => x.Equals(record.key)));
                if (isWhitelisted && !String.IsNullOrWhiteSpace(record.value))
                {
                    SetVariable(new VariableState { Name = record.key, Value = record.value, Type = VariableType.String, Save = saveVarsInDB });
                }
                else
                {
                    DeleteVariable(record.key);
                }
            };
            PluginInstance.GPUZ.OnSensorUpdated += (sender, record) =>
            {
                var isWhitelisted = !String.IsNullOrWhiteSpace(PluginInstance.Configuration.VariableWhitelist.Find(x => x.Equals(record.name)));
                if (isWhitelisted && !String.IsNullOrWhiteSpace(record.unit))
                {
                    SetVariable(new VariableState { Name = record.name + "_unit", Value = record.unit, Type = VariableType.String, Save = saveVarsInDB });
                } else
                {
                    DeleteVariable(record.name, "_unit");
                }
                if (isWhitelisted && !String.IsNullOrWhiteSpace(record.digits.ToString()))
                {
                    SetVariable(new VariableState { Name = record.name + "_digits", Value = (int)record.digits, Type = VariableType.Integer, Save = saveVarsInDB });
                } else
                {
                    DeleteVariable(record.name, "_digits");
                }
                if (isWhitelisted && !String.IsNullOrWhiteSpace(record.value.ToString()))
                {
                    SetVariable(new VariableState { Name = record.name + "_value", Value = (float)record.value, Type = VariableType.Float, Save = saveVarsInDB });
                } else
                {
                    DeleteVariable(record.name, "_value");
                }
            };
            PluginInstance.GPUZ.OnRefreshStarted += (sender, args) =>
            {
                RemoveVariables();
            };
            PluginInstance.GPUZ.OnRefreshComplete += (sender, args) =>
            {
                saveVarsInDB = false;
                failureLogged = false;
            };
            PluginInstance.GPUZ.OnMemoryReadFailed += (sender, failure) =>
            {
                if (!failureLogged)
                {
                    MacroDeckLogger.Warning(PluginInstance.Plugin, "GPU-Z shared memory file does not exist. Ensure the GPU-Z program is running to receive data.");
                    failureLogged = true;
                    saveVarsInDB = true;
                }
            };

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
                            _ = PluginInstance.GPUZ.Refresh();
                            Thread.Sleep(PluginInstance.Configuration.PollingFrequency * 1000);
                        }
                        catch (Exception ex)
                        {
                            MacroDeckLogger.Info(PluginInstance.Plugin,
                                                    String.Format("Failed to parse GPU-Z shared memory. Sleeping 60 seconds before trying again...\nException thrown: {0}",
                                                                  ex.ToString()));
                            RemoveVariables();
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
            configurator.FormClosed += Configurator_FormClosed;
        }

        private void Configurator_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            saveVarsInDB = true;
        }
    }
}
