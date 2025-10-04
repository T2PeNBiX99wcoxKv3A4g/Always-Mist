global using BepInExUtils.EX;
global using AlwaysMist.EX;
using Logger = BepInExUtils.Logger;

namespace AlwaysMist;

public static class Utils
{
    internal const string Guid = "io.github.ykysnk.AlwaysMist";
    internal const string Name = "Always Mist";
    internal const string FolderName = "AlwaysMist";
    internal const string Version = "0.0.12";

    private static Logger? _logger;
    public static Logger Logger => _logger ??= new(Name);

    // ReSharper disable once MemberCanBePrivate.Global
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

    // ReSharper disable once UnusedMember.Global
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