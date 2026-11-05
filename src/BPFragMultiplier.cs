using System.Collections.Generic;
using System.IO;
using System.Linq;
using BPFragMultiplier.Config;
using HarmonyLib;
using UnityEngine;

namespace BPFragMultiplier;

public class BPFragMultiplier : IHarmonyModHooks
{
    public static ConfigManager Config { get; private set; }

    public void OnLoaded(OnHarmonyModLoadedArgs args)
    {
        Config = new ConfigManager();
    }

    public void OnUnloaded(OnHarmonyModUnloadedArgs args) { }
}
