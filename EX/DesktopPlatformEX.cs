using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace AlwaysMist.EX;

public static class DesktopPlatformEX
{
    public static TraverseEX<string> saveDirPath(this DesktopPlatform instance) =>
        new(Traverse.Create(instance).Field<string>(nameof(saveDirPath)));

    public static string GetSavePath(this DesktopPlatform instance)
    {
        var gameSavePath = instance.saveDirPath();
        var modSavePath = Path.Combine(gameSavePath, Utils.FolderName);

        if (!Directory.Exists(modSavePath))
            Directory.CreateDirectory(modSavePath);

        return modSavePath;
    }
}