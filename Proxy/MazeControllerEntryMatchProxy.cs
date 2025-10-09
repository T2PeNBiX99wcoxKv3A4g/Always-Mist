using BepInExUtils.Proxy;
using HarmonyLib;
using JetBrains.Annotations;
using TeamCherry.SharedUtils;

namespace AlwaysMist.Proxy;

/// <summary>
///     <see cref="MazeController" />.EntryMatch
/// </summary>
[UsedImplicitly]
public class MazeControllerEntryMatchProxy : ClassProxy
{
    /// <summary>
    ///     <see cref="MazeController" />.EntryMatch
    /// </summary>
    public MazeControllerEntryMatchProxy() : base("MazeController+EntryMatch")
    {
    }

    /// <summary>
    ///     <see cref="MazeController" />.EntryMatch
    /// </summary>
    public MazeControllerEntryMatchProxy(object obj) : base(obj, "MazeController+EntryMatch")
    {
    }

    public string EntryScene
    {
        get => Traverse.Create(Instance).Field<string>(nameof(EntryScene)).Value;
        set => Traverse.Create(Instance).Field<string>(nameof(EntryScene)).Value = value;
    }

    public string EntryDoorDir
    {
        get => Traverse.Create(Instance).Field<string>(nameof(EntryDoorDir)).Value;
        set => Traverse.Create(Instance).Field<string>(nameof(EntryDoorDir)).Value = value;
    }

    public string ExitDoorDir
    {
        get => Traverse.Create(Instance).Field<string>(nameof(ExitDoorDir)).Value;
        set => Traverse.Create(Instance).Field<string>(nameof(ExitDoorDir)).Value = value;
    }

    public MinMaxFloat FogRotationRange
    {
        get => Traverse.Create(Instance).Field<MinMaxFloat>(nameof(FogRotationRange)).Value;
        set => Traverse.Create(Instance).Field<MinMaxFloat>(nameof(FogRotationRange)).Value = value;
    }

    public object? GetObject() => Instance;
}