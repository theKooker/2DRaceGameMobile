using System.Collections.Generic;

namespace UnityEngine.Advertisements
{
    sealed class Configuration
    {
        public bool enabled { get; private set; }
        public string defaultPlacement { get; private set; }
        public Dictionary<string, bool> placements { get; private set; }

        public Configuration(string configurationResponse)
        {
            var configurationJson = (Dictionary<string, object>)MiniJSON.Json.Deserialize(configurationResponse);
            enabled = (bool)configurationJson["enabled"];
            placements = new Dictionary<string, bool>();
            foreach (Dictionary<string, object> placement in (List<object>)configurationJson["placements"])
            {
                var id = (string)placement["id"];
                var allowSkip = (bool)placement["allowSkip"];
                if ((bool)placement["default"])
                {
                    defaultPlacement = id;
                }
                placements.Add(id, allowSkip);
            }
        }
    }
}
