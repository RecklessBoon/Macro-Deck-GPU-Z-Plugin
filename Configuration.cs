using Newtonsoft.Json;
using SuchByte.MacroDeck.Plugins;
using System;
using SuchByte.MacroDeck.Logging;
using System.Collections.Generic;

namespace RecklessBoon.MacroDeck.GPUZ
{
    public class Configuration
    {
        const int DEFAULT_POLLING_FREQUENCY = 15;

        [JsonIgnore]
        protected GPUZPlugin _plugin;

        protected int _pollingFrequency = DEFAULT_POLLING_FREQUENCY;
        public int PollingFrequency { 
            get 
            { 
                return _pollingFrequency; 
            }
            set
            {
                _pollingFrequency = value;
            }
        }

        protected List<string> _variableWhitelist = new List<string>();
        public List<string> VariableWhitelist
        {
            get { return _variableWhitelist; }
            set { _variableWhitelist = value; }
        }

        public Configuration(GPUZPlugin plugin)
        {
            if (plugin != null)
            {
                _plugin = plugin;
                Reload();
            }
        }

        public void Save()
        {
            SaveConfig();
            LoadConfig();
        }

        public void Reload()
        {
            LoadConfig();
        }

        protected void LoadConfig()
        {
            var json = PluginConfiguration.GetValue(_plugin, "config");
            if (json != null)
            {
                try
                {
                    var config = JsonConvert.DeserializeObject<Configuration>(json);
                    PollingFrequency = config != null ? config.PollingFrequency : DEFAULT_POLLING_FREQUENCY;
                    VariableWhitelist = config != null ? config.VariableWhitelist : new List<string>();
                }
                catch (Exception ex)
                {
                    MacroDeckLogger.Error(_plugin, "Invalid/corrupt configuration saved. Please backup and then clear the configuration file. Exception that ocurred: " + ex.ToString());
                }
            }
        }

        protected void SaveConfig()
        {
            var json = JsonConvert.SerializeObject(this);
            PluginConfiguration.SetValue(_plugin, "config", json);
        }

    }
}
