// ReSharper disable InconsistentNaming

using HarmonyLib;

namespace AlwaysMist.EX;

public static class MazeControllerEX
{
    public static TraverseEX<string[]> sceneNames(this MazeController instance) =>
        new(Traverse.Create(instance).Field<string[]>(nameof(sceneNames)));

    public static TraverseEX<int> neededCorrectDoors(this MazeController instance) =>
        new(Traverse.Create(instance).Field<int>(nameof(neededCorrectDoors)));

    public static TraverseEX<int> allowedIncorrectDoors(this MazeController instance) =>
        new(Traverse.Create(instance).Field<int>(nameof(allowedIncorrectDoors)));

    public static TraverseEX<int> restScenePoint(this MazeController instance) =>
        new(Traverse.Create(instance).Field<int>(nameof(restScenePoint)));

    public static TraverseEX<string> restSceneName(this MazeController instance) =>
        new(Traverse.Create(instance).Field<string>(nameof(restSceneName)));

    public static TraverseEX<string> exitSceneName(this MazeController instance) =>
        new(Traverse.Create(instance).Field<string>(nameof(exitSceneName)));

    public static TraverseEX<object[]> entryMatchExit(this MazeController instance) =>
        new(Traverse.Create(instance).Field<object[]>(nameof(entryMatchExit)));
}