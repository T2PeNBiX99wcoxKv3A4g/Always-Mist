using BepInExUtils.Proxy;
using JetBrains.Annotations;
using TeamCherry.SharedUtils;

namespace AlwaysMist.Proxy;

/// <summary>
///     <see cref="MazeController" />.EntryMatch
/// </summary>
[PublicAPI]
public class MazeControllerEntryMatchProxy : ClassProxy
{
    private const string ClassName = "MazeController+EntryMatch";

    /// <summary>
    ///     <see cref="MazeController" />.EntryMatch
    /// </summary>
    public MazeControllerEntryMatchProxy() : base(ClassName)
    {
    }

    /// <summary>
    ///     <see cref="MazeController" />.EntryMatch
    /// </summary>
    public MazeControllerEntryMatchProxy(object obj) : base(obj, ClassName)
    {
    }

    public string EntryScene
    {
        get => Native.GetFieldValue<string>(nameof(EntryScene))!;
        set => Native.SetFieldValue(nameof(EntryScene), value);
    }

    public string EntryDoorDir
    {
        get => Native.GetFieldValue<string>(nameof(EntryDoorDir))!;
        set => Native.SetFieldValue(nameof(EntryDoorDir), value);
    }

    public string ExitDoorDir
    {
        get => Native.GetFieldValue<string>(nameof(ExitDoorDir))!;
        set => Native.SetFieldValue(nameof(ExitDoorDir), value);
    }

    public MinMaxFloat FogRotationRange
    {
        get => Native.GetFieldValue<MinMaxFloat>(nameof(FogRotationRange));
        set => Native.SetFieldValue(nameof(FogRotationRange), value);
    }
}