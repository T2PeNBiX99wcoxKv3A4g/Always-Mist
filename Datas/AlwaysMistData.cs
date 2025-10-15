using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace AlwaysMist.Datas;

[JsonObject]
[Serializable]
[SuppressMessage("ReSharper", "InconsistentNaming")]
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