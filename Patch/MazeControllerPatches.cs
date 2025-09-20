using HarmonyLib;
using Random = UnityEngine.Random;

// ReSharper disable InconsistentNaming

namespace AlwaysMist.Patch;

[HarmonyPatch(typeof(MazeController))]
internal class MazeControllerPatches
{
    [HarmonyPatch(nameof(MazeController.Activate))]
    [HarmonyPrefix]
    private static void Activate(MazeController __instance)
    {
        var controller = AlwaysMistController.Instance;
        if (!controller) return;
        // ReSharper disable once InvertIf
        if (Main.RandomNeededCorrectDoors is { Value: true } || Main.TrueAlwaysMist is { Value: true })
        {
            if (controller.LastRandomValue < 0)
                controller.LastRandomValue = Random.Range(Main.MinRandomNeededCorrectDoors.Value(),
                    Main.MaxRandomNeededCorrectDoors.Value());
            __instance.neededCorrectDoors().V = controller.LastRandomValue;
            __instance.restScenePoint().V = (int)Math.Round(controller.LastRandomValue / 2f);
        }

        // if (!controller.IsEnteredMaze) return;
        // if (controller.CurrentSceneName != AlwaysMistController.MazeExitSceneName) return;
        // if (Main.TrueAlwaysMist is not { Value: true }) return;
        // if (string.IsNullOrEmpty(controller.TargetSceneName)) return;
        // __instance.exitSceneName().V = controller.TargetSceneName;
    }

    [HarmonyPatch(nameof(GetExitMatch))]
    [HarmonyPrefix]
    private static void GetExitMatch(MazeController __instance)
    {
        var controller = AlwaysMistController.Instance;
        if (!controller) return;
        if (Main.TrueAlwaysMist is not { Value: true }) return;
        if (string.IsNullOrEmpty(controller.TargetSceneName)) return;
        if (!controller.IsEnteredMaze) return;
        var playerData = PlayerData.instance;
        playerData.MazeEntranceScene = controller.TargetEntryDoorDir switch
        {
            "left" => "Shadow_04",
            "right" => "Dust_05",
            _ => AlwaysMistController.MazeOldTargetSceneName
        };
        playerData.MazeEntranceDoor = "left";
    }

    // [HarmonyPatch(nameof(GetExitMatch))]
    // [HarmonyPrefix]
    // private static bool GetExitMatch(MazeController __instance, ref object __result)
    // {
    //     var controller = AlwaysMistController.Instance;
    //     if (!controller) return true;
    //     if (Main.TrueAlwaysMist is not { Value: true }) return true;
    //     if (string.IsNullOrEmpty(controller.TargetSceneName)) return true;
    //     if (controller.CurrentSceneName != AlwaysMistController.MazeExitSceneName) return true;
    //
    //     Utils.Logger.Debug("Create Entry Match");
    //
    //     var exitMatch = new MazeControllerEntryMatchProxy();
    //     exitMatch.EntryScene().V = controller.TargetSceneName;
    //     exitMatch.EntryDoorDir().V = controller.TargetEntryDoorDir;
    //     exitMatch.ExitDoorDir().V = controller.TargetExitDoorDir;
    //     exitMatch.FogRotationRange().V = controller.TargetEntryDoorDir switch
    //     {
    //         "left" => new MinMaxFloat(0, 0),
    //         "right" => new MinMaxFloat(0, 3.1416f),
    //         _ => new MinMaxFloat(0, 0)
    //     };
    //
    //     var ret = exitMatch.GetObject();
    //     Utils.Logger.Debug($"Create Entry Match is {ret}");
    //     if (ret == null) return true;
    //     __result = ret;
    //     return false;
    // }
}