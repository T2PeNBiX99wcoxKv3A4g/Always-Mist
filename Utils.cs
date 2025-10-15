global using AlwaysMist.Extensions;

namespace AlwaysMist;

public partial class Utils
{
    internal const string FolderName = "AlwaysMist";
    internal const string GameName = "Hollow Knight Silksong.exe";

    public static string? GetDoorDir(string doorName)
    {
        if (doorName.StartsWith("left"))
            return "left";
        if (doorName.StartsWith("right"))
            return "right";
        if (doorName.StartsWith("top"))
            return "top";
        return doorName.StartsWith("bot") ? "bot" : null;
    }

    public static string? GetDoorDirMatch(string doorName)
    {
        var dir = GetDoorDir(doorName);
        if (dir == null) return null;
        return dir switch
        {
            "left" => "right",
            "right" => "left",
            "top" => "bot",
            "bot" => "top",
            _ => null
        };
    }

    public static string? GetEntryDoorDir(string doorName)
    {
        var dir = GetDoorDir(doorName);
        if (dir == null) return null;
        return dir switch
        {
            "left" => "left",
            "right" => "right",
            "top" => "left",
            "bot" => "right",
            _ => null
        };
    }
}