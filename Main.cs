using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace AlwaysMist;

[BepInPlugin(Utils.Guid, Utils.Name, Utils.Version)]
public class Main : BaseUnityPlugin
{
    private const string SectionOptions = "Options";

    internal static ConfigEntry<bool>? ResetMazeSaveData;
    internal static ConfigEntry<bool>? RandomNeededCorrectDoors;
    internal static ConfigEntry<int>? MaxRandomNeededCorrectDoors;
    internal static ConfigEntry<int>? MinRandomNeededCorrectDoors;
    internal static ConfigEntry<bool>? TrueAlwaysMist;
    internal static ConfigEntry<bool>? RestBenchInMist;
    private readonly Harmony _harmony = new(Utils.Guid);

    private void Awake()
    {
        Utils.Logger.Info($"Plugin {Utils.Name} loaded, version {Utils.Version}");
        _harmony.PatchAll(Assembly.GetExecutingAssembly());

        ResetMazeSaveData = Config.Bind(SectionOptions, nameof(ResetMazeSaveData), false,
            "Always reset maze save data when enter even if you're not dead.");
        RandomNeededCorrectDoors = Config.Bind(SectionOptions, nameof(RandomNeededCorrectDoors), false,
            "Random the correct doors are needed when enter every single time.");
        MaxRandomNeededCorrectDoors = Config.Bind(SectionOptions, nameof(MaxRandomNeededCorrectDoors), 10,
            new ConfigDescription("The max value of random the correct doors needed.",
                new AcceptableValueRange<int>(2, 100)));
        MinRandomNeededCorrectDoors = Config.Bind(SectionOptions, nameof(MinRandomNeededCorrectDoors), 2,
            new ConfigDescription("The min value of random the correct doors needed.",
                new AcceptableValueRange<int>(2, 100)));
        TrueAlwaysMist = Config.Bind(SectionOptions, nameof(TrueAlwaysMist), false,
            "Always enter the mist maze first when entering any rooms or any doors.");
        RestBenchInMist =
            Config.Bind(SectionOptions, nameof(RestBenchInMist), false, "Turn on the rest bench in the mist maze.");

        var obj = new GameObject(nameof(AlwaysMistController));
        obj.AddComponent<AlwaysMistController>();
        DontDestroyOnLoad(obj);
    }
}