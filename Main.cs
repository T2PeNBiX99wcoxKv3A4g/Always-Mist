using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace AlwaysMist;

[BepInPlugin(Utils.Guid, Utils.Name, Utils.Version)]
public class Main : BaseUnityPlugin
{
    private readonly Harmony _harmony = new(Utils.Guid);

    internal static ConfigEntry<bool>? ResetMazeSaveData;
    internal static ConfigEntry<bool>? RandomNeededCorrectDoors;
    internal static ConfigEntry<int>? MaxRandomNeededCorrectDoors;
    internal static ConfigEntry<int>? MinRandomNeededCorrectDoors;
    internal static ConfigEntry<bool>? TrueAlwaysMist;
    internal static ConfigEntry<bool>? RestBenchInMist;

    private const string SectionOptions = "Options";

    private void Awake()
    {
        Utils.Logger.Info($"Plugin {Utils.Name} loaded, version {Utils.Version}");
        _harmony.PatchAll(Assembly.GetExecutingAssembly());

        ResetMazeSaveData = Config.Bind(SectionOptions, nameof(ResetMazeSaveData), false,
            "Always reset maze save data when enter even you're not death.");
        RandomNeededCorrectDoors = Config.Bind(SectionOptions, nameof(RandomNeededCorrectDoors), false,
            "Random the correct doors needed when enter every single times.");
        MaxRandomNeededCorrectDoors = Config.Bind(SectionOptions, nameof(MaxRandomNeededCorrectDoors), 10,
            new ConfigDescription("The max value of random the correct doors needed",
                new AcceptableValueRange<int>(2, 100)));
        MinRandomNeededCorrectDoors = Config.Bind(SectionOptions, nameof(MinRandomNeededCorrectDoors), 2,
            new ConfigDescription("The min value of random the correct doors needed",
                new AcceptableValueRange<int>(2, 100)));
        TrueAlwaysMist = Config.Bind(SectionOptions, nameof(TrueAlwaysMist), false, "Always enter mist maze");
        RestBenchInMist =
            Config.Bind(SectionOptions, nameof(RestBenchInMist), false, "Turn on rest bench in mist maze");

        var obj = new GameObject(nameof(AlwaysMistController));
        obj.AddComponent<AlwaysMistController>();
        DontDestroyOnLoad(obj);
    }
}