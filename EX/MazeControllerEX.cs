// ReSharper disable InconsistentNaming

using HarmonyLib;
using JetBrains.Annotations;

namespace AlwaysMist.EX;

public static class MazeControllerEX
{
    extension(MazeController instance)
    {
        [UsedImplicitly]
        public bool isCapScene
        {
            get => Traverse.Create(instance).Field<bool>("isCapScene").Value;
            set => Traverse.Create(instance).Field<bool>("isCapScene").Value = value;
        }

        [UsedImplicitly]
        public List<TransitionPoint> entryDoors
        {
            get => Traverse.Create(instance).Field<List<TransitionPoint>>("entryDoors").Value;
            set => Traverse.Create(instance).Field<List<TransitionPoint>>("entryDoors").Value = value;
        }

        [UsedImplicitly]
        public string[] sceneNames
        {
            get => Traverse.Create(instance).Field<string[]>("sceneNames").Value;
            set => Traverse.Create(instance).Field<string[]>("sceneNames").Value = value;
        }

        [UsedImplicitly]
        public int neededCorrectDoors
        {
            get => Traverse.Create(instance).Field<int>("neededCorrectDoors").Value;
            set => Traverse.Create(instance).Field<int>("neededCorrectDoors").Value = value;
        }

        [UsedImplicitly]
        public int allowedIncorrectDoors
        {
            get => Traverse.Create(instance).Field<int>("allowedIncorrectDoors").Value;
            set => Traverse.Create(instance).Field<int>("allowedIncorrectDoors").Value = value;
        }

        [UsedImplicitly]
        public int restScenePoint
        {
            get => Traverse.Create(instance).Field<int>("restScenePoint").Value;
            set => Traverse.Create(instance).Field<int>("restScenePoint").Value = value;
        }

        [UsedImplicitly]
        public string restSceneName
        {
            get => Traverse.Create(instance).Field<string>("restSceneName").Value;
            set => Traverse.Create(instance).Field<string>("restSceneName").Value = value;
        }

        [UsedImplicitly]
        public string exitSceneName
        {
            get => Traverse.Create(instance).Field<string>("exitSceneName").Value;
            set => Traverse.Create(instance).Field<string>("exitSceneName").Value = value;
        }

        [UsedImplicitly]
        public object[] entryMatchExit
        {
            get => Traverse.Create(instance).Field<object[]>("entryMatchExit").Value;
            set => Traverse.Create(instance).Field<object[]>("entryMatchExit").Value = value;
        }

        [UsedImplicitly]
        public bool isActive
        {
            get => Traverse.Create(instance).Field<bool>("isActive").Value;
            set => Traverse.Create(instance).Field<bool>("isActive").Value = value;
        }

        [UsedImplicitly]
        public List<TransitionPoint> correctDoors
        {
            get => Traverse.Create(instance).Field<List<TransitionPoint>>("correctDoors").Value;
            set => Traverse.Create(instance).Field<List<TransitionPoint>>("correctDoors").Value = value;
        }
    }
}