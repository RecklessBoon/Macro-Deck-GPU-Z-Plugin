using SuchByte.MacroDeck.GUI.CustomControls;
using System.Windows.Forms;

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

                _config.Save();
                this.Close();
            }
        }
    }
}
