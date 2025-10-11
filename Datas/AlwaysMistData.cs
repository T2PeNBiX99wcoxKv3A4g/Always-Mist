using Newtonsoft.Json;

// ReSharper disable InconsistentNaming

namespace AlwaysMist.Datas;

[JsonObject]
[Serializable]
public class AlwaysMistData
{
    public string TargetSceneName = "";
    public string TargetEntryDoorDir = "";
    public string TargetExitDoorDir = "";
    public string TargetExitDoorName = "";
    public string EnterSceneName = "";
    public string EnterDoorName = "";
    public bool IsEnteredMaze;
    public int LastRandomValue = -1;
}