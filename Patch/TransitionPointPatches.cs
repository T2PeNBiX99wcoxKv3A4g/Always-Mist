using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace AlwaysMist.Patch;

[HarmonyPatch(typeof(TransitionPoint))]
internal static class TransitionPointPatches
{
    [HarmonyPatch(nameof(DoSceneTransition))]
    [HarmonyPrefix]
    private static void DoSceneTransition(TransitionPoint __instance, bool doFade)
    {
        if (Main.TrueAlwaysMist is not { Value: true }) return;
        var controller = AlwaysMistController.Instance;
        if (!controller || !controller.IsOutsideMaze ||
            !controller.ChangedTransitionPoint.TryGetValue(__instance, out var oldTargetScene)) return;
        controller.TargetSceneName = oldTargetScene;
        controller.TargetEntryDoorDir = Utils.GetEntryDoorDir(__instance.name) ?? "left";
        controller.TargetExitDoorDir = __instance.entryPoint;
        controller.EnterDoorName = __instance.name;
        Utils.Logger.Debug($"controller.TargetSceneName: {controller.TargetSceneName}");
        Utils.Logger.Debug($"controller.TargetEntryDoorDir: {controller.TargetEntryDoorDir}");
        Utils.Logger.Debug($"controller.TargetExitDoorDir: {controller.TargetExitDoorDir}");
        Utils.Logger.Debug($"controller.EnterDoorName: {controller.EnterDoorName}");
        __instance.entryPoint = "right1";
    }
}