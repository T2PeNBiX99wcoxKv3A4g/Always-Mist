using AlwaysMist.Behaviour;
using HarmonyLib;

namespace AlwaysMist.Patches;

[HarmonyPatch(typeof(TransitionPoint))]
internal static class TransitionPointPatches
{
    [HarmonyPatch(nameof(DoSceneTransition))]
    [HarmonyPrefix]
    private static void DoSceneTransition(TransitionPoint __instance, bool doFade)
    {
        if (!Configs.TrueAlwaysMist) return;
        var controller = AlwaysMistController.Instance;
        if (!controller || !controller.IsOutsideMaze ||
            !controller.ChangedTransitionPoint.TryGetValue(__instance, out var oldTargetScene)) return;
        controller.TargetSceneName = oldTargetScene;
        controller.TargetEntryDoorDir = Utils.GetEntryDoorDir(__instance.name) ??
                                        Utils.GetDoorDirMatch(__instance.entryPoint) ?? "left";
        controller.TargetExitDoorDir = Utils.GetDoorDir(__instance.entryPoint) ?? "right";
        controller.TargetExitDoorName = __instance.entryPoint;
        controller.EnterDoorName = __instance.name;
        Utils.Logger.Debug($"controller.TargetSceneName: {controller.TargetSceneName}");
        Utils.Logger.Debug($"controller.TargetEntryDoorDir: {controller.TargetEntryDoorDir}");
        Utils.Logger.Debug($"controller.TargetExitDoorDir: {controller.TargetExitDoorDir}");
        Utils.Logger.Debug($"controller.TargetExitDoorName: {controller.TargetExitDoorName}");
        Utils.Logger.Debug($"controller.EnterDoorName: {controller.EnterDoorName}");
        __instance.SetTargetDoor(controller.TargetEntryDoorDir switch
        {
            "left" => "right1",
            "right" => "left1",
            _ => "right1"
        });
    }
}