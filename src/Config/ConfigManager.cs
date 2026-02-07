using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace BPFragMultiplier.Config;

public class ConfigManager
{
    private const string ConfigPath = "HarmonyMods/BPFragMultiplier/config.json";

    public ConfigData Data { get; private set; }

    public ConfigManager() => LoadConfig();

    public void LoadConfig()
    {
        Data = new ConfigData();

        if (!File.Exists(ConfigPath))
        {
            SaveConfig();

            return;
        }

        try
        {
            Data =
                JsonConvert.DeserializeObject<ConfigData>(File.ReadAllText(ConfigPath))
                ?? throw new Exception("CONFIG_ERROR");
        }
        catch
        {
            Debug.LogError(
                "The configuration seems to be missing or malformed. Defaults will be loaded."
            );
        }
    }

    private void SaveConfig()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath));
        File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Data, Formatting.Indented));
    }

    public class ConfigData
    {
        [JsonProperty("Basic blueprint fragment multiplier")]
        public int BasicFragmentMultiplier = 2;

        [JsonProperty("Advanced blueprint fragment multiplier")]
        public int AdvancedFragmentMultiplier = 2;
    }
}
