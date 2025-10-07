using BepInExUtils.Attributes;

namespace AlwaysMist.Extensions;

[AccessExtensions]
[AccessInstance<DesktopPlatform>]
[AccessField<string>("saveDirPath")]
public static partial class DesktopPlatformExtensions
{
    extension(DesktopPlatform instance)
    {
        public string GetSavePath()
        {
            var modSavePath = Path.Combine(instance.saveDirPath, Utils.FolderName);

            if (!Directory.Exists(modSavePath))
                Directory.CreateDirectory(modSavePath);

            return modSavePath;
        }
    }
}