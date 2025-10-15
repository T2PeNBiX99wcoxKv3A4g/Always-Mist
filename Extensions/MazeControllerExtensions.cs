using BepInExUtils.Attributes;
using JetBrains.Annotations;

namespace AlwaysMist.Extensions;

[AccessExtensions]
[AccessInstance<MazeController>]
[AccessField<bool>("isCapScene")]
[AccessField<List<TransitionPoint>>("entryDoors")]
[AccessField<string[]>("sceneNames")]
[AccessField<int>("neededCorrectDoors")]
[AccessField<int>("allowedIncorrectDoors")]
[AccessField<int>("restScenePoint")]
[AccessField<string>("restSceneName")]
[AccessField<string>("exitSceneName")]
[AccessField<object[]>("entryMatchExit")]
[AccessField<bool>("isActive")]
[AccessField<List<TransitionPoint>>("correctDoors")]
[PublicAPI]
public static partial class MazeControllerExtensions
{
}