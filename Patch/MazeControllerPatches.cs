using AlwaysMist.Proxy;
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
        if (__instance.isActive().V) return;
        var controller = AlwaysMistController.Instance;
        if (!controller) return;
        if (Configs.RandomNeededCorrectDoors || Configs.TrueAlwaysMist)
        {
            if (controller.LastRandomValue < 0)
                controller.LastRandomValue = Random.Range(Configs.MinRandomNeededCorrectDoors,
                    Configs.MaxRandomNeededCorrectDoors);
            __instance.neededCorrectDoors().V = controller.LastRandomValue;
            __instance.restScenePoint().V = (int)Math.Round(controller.LastRandomValue / 2f);
        }

        if (controller.CurrentSceneName == AlwaysMistController.MazeExitSceneName &&
            !string.IsNullOrEmpty(controller.TargetSceneName))
            __instance.exitSceneName().V = controller.TargetSceneName;

        if (controller.CurrentSceneName != AlwaysMistController.MazeEntranceSceneName) return;
        if (!Configs.TrueAlwaysMist) return;
        if (string.IsNullOrEmpty(controller.TargetEntryDoorDir) || controller.TargetEntryDoorDir == "left") return;
        var newDoors = TransitionPoint.TransitionPoints.FirstOrDefault(door =>
            door.gameObject.scene == __instance.gameObject.scene &&
            door.gameObject.name == AlwaysMistController.RightTransitionPoint);
        if (!newDoors) return;

        var playerData = PlayerData.instance;
        Utils.Logger.Debug($"playerData.PreviousMazeTargetDoor: {playerData.PreviousMazeTargetDoor}");

        __instance.entryDoors().V.Clear();
        __instance.entryDoors().V.Add(newDoors);
    }

    [HarmonyPatch(nameof(GetExitMatch))]
    [HarmonyPrefix]
    private static bool GetExitMatch(MazeController __instance, ref object __result)
    {
        var controller = AlwaysMistController.Instance;
        if (!controller) return true;
        if (!Configs.TrueAlwaysMist) return true;
        if (string.IsNullOrEmpty(controller.TargetSceneName)) return true;
        if (!controller.IsEnteredMaze) return true;

        Utils.Logger.Debug("Create Entry Match");

        var exitMatch = new MazeControllerEntryMatchProxy();
        exitMatch.EntryScene().V = controller.EnterSceneName;
        exitMatch.EntryDoorDir().V = controller.TargetEntryDoorDir;
        exitMatch.ExitDoorDir().V = controller.CurrentSceneName == AlwaysMistController.MazeExitSceneName
            ? controller.TargetExitDoorName
            : controller.TargetExitDoorDir;
        exitMatch.FogRotationRange().V = controller.TargetEntryDoorDir switch
        {
            "left" => new(0, 0),
            "right" => new(0, 3.1416f),
            _ => new(0, 0)
        };

        var ret = exitMatch.GetObject();
        Utils.Logger.Debug($"Create Entry Match is {ret}");
        if (ret == null) return true;
        __result = ret;
        return false;
    }

    [HarmonyPatch(nameof(SubscribeDoorEntered))]
    [HarmonyPrefix]
    private static bool SubscribeDoorEntered(MazeController __instance, TransitionPoint door)
    {
        var controller = AlwaysMistController.Instance;
        if (!controller) return true;
        if (!Configs.TrueAlwaysMist) return true;

        door.OnBeforeTransition += () =>
        {
            var instance = PlayerData.instance;
            var name = door.name;
            if (!__instance.isCapScene().V)
            {
                if (door.targetScene == __instance.restSceneName())
                {
                    instance.EnteredMazeRestScene = true;
                    instance.CorrectMazeDoorsEntered = __instance.neededCorrectDoors() - __instance.restScenePoint();
                    instance.IncorrectMazeDoorsEntered = 0;
                }
                else if (instance.PreviousMazeTargetDoor != name)
                {
                    if (__instance.correctDoors().V.Contains(door))
                    {
                        ++instance.CorrectMazeDoorsEntered;
                        instance.IncorrectMazeDoorsEntered = 0;
                    }
                    else
                    {
                        instance.CorrectMazeDoorsEntered = 0;
                        ++instance.IncorrectMazeDoorsEntered;
                        instance.EnteredMazeRestScene = false;

                        var isLeft = string.IsNullOrEmpty(controller.TargetEntryDoorDir) ||
                                     controller.TargetEntryDoorDir == "left";

                        if (instance.IncorrectMazeDoorsEntered >= __instance.allowedIncorrectDoors() &&
                            name.StartsWith(isLeft ? "right" : "left"))
                        {
                            door.SetTargetScene("Dust_Maze_09_entrance");
                            door.entryPoint = isLeft
                                ? AlwaysMistController.LeftTransitionPoint
                                : AlwaysMistController.RightTransitionPoint;
                        }
                    }
                }
            }

            instance.PreviousMazeTargetDoor = door.entryPoint;
            instance.PreviousMazeScene = door.gameObject.scene.name;
            instance.PreviousMazeDoor = name;
        };
        return false;
    }
}