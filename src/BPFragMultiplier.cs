using BPFragMultiplier.Config;

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
