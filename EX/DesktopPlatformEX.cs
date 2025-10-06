using HarmonyLib;
using JetBrains.Annotations;

// ReSharper disable InconsistentNaming

namespace AlwaysMist.EX;

public static class DesktopPlatformEX
{
    extension(DesktopPlatform instance)
    {
        [UsedImplicitly]
        public string saveDirPath
        {
            get => Traverse.Create(instance).Field<string>("saveDirPath").Value;
            set => Traverse.Create(instance).Field<string>("saveDirPath").Value = value;
        }

        public string GetSavePath()
        {
            var modSavePath = Path.Combine(instance.saveDirPath, Utils.FolderName);

            if (!Directory.Exists(modSavePath))
                Directory.CreateDirectory(modSavePath);

            return modSavePath;
        }
    }
}