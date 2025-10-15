using AlwaysMist.Behaviour;
using BepInEx;
using BepInExUtils.Attributes;
using UnityEngine;

namespace AlwaysMist;

[BepInUtils("io.github.ykysnk.AlwaysMist", "Always Mist", Version)]
[BepInDependency("io.github.ykysnk.BepinExUtils", "0.8.1")]
[BepInProcess(Utils.GameName)]
[ConfigBind<bool>("ResetMazeSaveData", SectionOptions, false,
    "Always reset maze save data when enter even if you're not dead.")]
[ConfigBind<bool>("RandomNeededCorrectDoors", SectionOptions, false,
    "Random the correct doors are needed when enter every single time.")]
[ConfigBind<int>("MaxRandomNeededCorrectDoors", SectionOptions, 10, "The max value of random the correct doors needed.",
    2, 100)]
[ConfigBind<int>("MinRandomNeededCorrectDoors", SectionOptions, 2, "The min value of random the correct doors needed.",
    2, 100)]
[ConfigBind<bool>("TrueAlwaysMist", SectionOptions, false,
    "Always enter the mist maze first when entering any rooms or any doors.")]
[ConfigBind<bool>("RestBenchInMist", SectionOptions, false, "Turn on the rest bench in the mist maze.")]
public partial class Main
{
    private const string SectionOptions = "Options";
    private const string Version = "0.1.25";

    protected override void PostAwake()
    {
        var obj = new GameObject(nameof(AlwaysMistController));
        obj.AddComponent<AlwaysMistController>();
        DontDestroyOnLoad(obj);
    }
}