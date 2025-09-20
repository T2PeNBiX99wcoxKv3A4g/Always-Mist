using BepInExUtils.Proxy;
using HarmonyLib;
using TeamCherry.SharedUtils;

namespace AlwaysMist.Proxy;

public class MazeControllerEntryMatchProxy : ClassProxy
{
    public MazeControllerEntryMatchProxy() : base("MazeControllerEntryMatchProxy")
    {
    }

    public MazeControllerEntryMatchProxy(object obj) : base(obj, "MazeControllerEntryMatchProxy")
    {
    }

    public TraverseEX<string> EntryScene() => new(Traverse.Create(Instance).Field<string>(nameof(EntryScene)));
    public TraverseEX<string> EntryDoorDir() => new(Traverse.Create(Instance).Field<string>(nameof(EntryDoorDir)));
    public TraverseEX<string> ExitDoorDir() => new(Traverse.Create(Instance).Field<string>(nameof(ExitDoorDir)));

    public TraverseEX<MinMaxFloat> FogRotationRange() =>
        new(Traverse.Create(Instance).Field<MinMaxFloat>(nameof(FogRotationRange)));
    
    public object? GetObject() => Instance;
}