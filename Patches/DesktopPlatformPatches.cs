using AlwaysMist.Behaviour;
using AlwaysMist.Datas;
using HarmonyLib;

namespace AlwaysMist.Patches;

// Only desktop support
[HarmonyPatch(typeof(DesktopPlatform))]
internal class DesktopPlatformPatches
{
    private const string SaveFileName = "saveData{0}.json";
    private const string SaveFileBackupName = $"{SaveFileName}.bak";
    private const string SaveFileNewName = $"{SaveFileName}.new";

    [HarmonyPatch(nameof(DesktopPlatform.WriteSaveSlot))]
    [HarmonyPostfix]
    private static void WriteSaveSlot(DesktopPlatform __instance, int slotIndex, byte[] bytes, Action<bool> callback)
    {
        Utils.Logger.Debug($"DesktopPlatform.WriteSaveSlot: {slotIndex}");

        var controller = AlwaysMistController.Instance;

        if (!controller || !controller.SaveDatas.TryGetValue(slotIndex, out var data)) return;

        var savePath = __instance.GetSavePath();
        var filePath = Path.Combine(savePath, string.Format(SaveFileName, slotIndex));
        var newFilePath = Path.Combine(savePath, string.Format(SaveFileNewName, slotIndex));
        var backupFilePath = Path.Combine(savePath, string.Format(SaveFileBackupName, slotIndex));

        CoreLoop.InvokeNext(() =>
        {
            SaveDataUtility.SerializeToJsonAsync(data, (success, result) =>
            {
                if (!success) return;

                try
                {
                    File.WriteAllText(newFilePath, result);
                }
                catch (Exception ex)
                {
                    Utils.Logger.Error(ex);
                }

                try
                {
                    if (File.Exists(filePath))
                        File.Replace(newFilePath, filePath, backupFilePath);
                    else
                        File.Move(newFilePath, filePath);
                }
                catch (Exception ex)
                {
                    Utils.Logger.Error(ex);
                }
            });
        });
    }

    [HarmonyPatch(nameof(DesktopPlatform.ReadSaveSlot))]
    [HarmonyPostfix]
    private static void ReadSaveSlot(DesktopPlatform __instance, int slotIndex, Action<byte[]> callback)
    {
        Utils.Logger.Debug($"DesktopPlatform.ReadSaveSlot: {slotIndex}");

        var controller = AlwaysMistController.Instance;

        if (!controller) return;

        var savePath = __instance.GetSavePath();
        var filePath = Path.Combine(savePath, string.Format(SaveFileName, slotIndex));
        var saveJson = "";

        try
        {
            if (File.Exists(filePath))
                saveJson = File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Utils.Logger.Error(ex);
        }

        if (string.IsNullOrEmpty(saveJson)) return;

        CoreLoop.InvokeSafe(() =>
        {
            controller.SaveDatas[slotIndex] = SaveDataUtility.DeserializeSaveData<AlwaysMistData>(saveJson);
        });
    }
}