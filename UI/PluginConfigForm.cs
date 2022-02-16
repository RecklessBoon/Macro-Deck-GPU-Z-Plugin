using SuchByte.MacroDeck.GUI.CustomControls;
using System.Windows.Forms;
using System.Linq;
using static System.Windows.Forms.ListView;
using System.Collections.Generic;
using System;

namespace RecklessBoon.MacroDeck.GPUZ.UI
{
    public partial class PluginConfigForm : DialogForm
    {
        protected Configuration _config { get; set; }

        public PluginConfigForm(Configuration config)
        {
            _config ??= config;
            InitializeComponent();

            pollingFrequency.Value = _config.PollingFrequency;
            var form = this;

            EventHandler<EventArgs> handler = null;
            handler = (sender, args) =>
            {
                GPUZ_RECORD[] data = new GPUZ_RECORD[PluginInstance.GPUZ.Data.Count];
                GPUZ_SENSOR_RECORD[] sensors = new GPUZ_SENSOR_RECORD[PluginInstance.GPUZ.Sensors.Count];
                PluginInstance.GPUZ.Data.CopyTo(data);
                PluginInstance.GPUZ.Sensors.CopyTo(sensors);
                _ = form.BeginInvoke(new MethodInvoker(delegate
                {
                    variablesWhitelist.Items.Clear();
                    foreach(var variable in data) 
                    { 
                        ListViewItem item = new ListViewItem()
                        {
                            Name = variable.key,
                            Text = variable.key
                        };
                        variablesWhitelist.Items.Add(item);
                        if (config.VariableWhitelist.Contains(variable.key))
                        {
                            item.Checked = true;
                        }

                    }
                    foreach(var variable in sensors)
                    {
                        ListViewItem item = new ListViewItem()
                        {
                            Name = variable.name,
                            Text = variable.name
                        };
                        variablesWhitelist.Items.Add(item);
                        if (config.VariableWhitelist.Contains(variable.name))
                        {
                            item.Checked = true;
                        }
                    }
                    variablesWhitelist.Invalidate();
                    PluginInstance.GPUZ.OnRefreshComplete -= handler;
                }));
            };
            PluginInstance.GPUZ.OnRefreshComplete += handler;
            _ = PluginInstance.GPUZ.Refresh();
        }

        private void PollingFrequency_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var input = sender as NumericUpDown;
            if (input.Value < input.Minimum)
            {
                pollingFrequencyErrorProvider.SetError(input, "Value must be >= 5");
                e.Cancel = true;
            } else
            {
                pollingFrequencyErrorProvider.SetError(input, string.Empty);
            }
        }

        private void btn_OK_Click(object sender, System.EventArgs e)
        {
            if (this.Validate())
            {
                _config.PollingFrequency = (int)pollingFrequency.Value;
                _config.VariableWhitelist = new List<string>();
                foreach(ListViewItem variable in variablesWhitelist.CheckedItems)
                {
                    _config.VariableWhitelist.Add(variable.Text);
                }

                _config.Save();
                this.Close();
            }
        }
    }
}
