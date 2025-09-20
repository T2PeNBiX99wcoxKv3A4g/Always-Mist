using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlwaysMist;

public class AlwaysMistController : MonoBehaviour
{
    private const string ChangeScene = "Shadow_04";
    private const string ChangeScene2 = "Dust_05";
    private const string LeftTransitionPoint = "left1";
    private const string RightTransitionPoint = "right1";

    // ReSharper disable MemberCanBePrivate.Global
    public const string MazeEntranceSceneName = "Dust_Maze_09_entrance";
    public const string MazeExitSceneName = "Dust_Maze_Last_Hall";
    public const string MazeRestSceneName = "Dust_Maze_crossing";
    public const string MazeOldTargetSceneName = "Dust_09";
    // ReSharper restore MemberCanBePrivate.Global

    // ReSharper disable MemberCanBePrivate.Global
    public string CurrentSceneName { get; private set; } = "";
    public string TargetSceneName { get; set; } = "";
    public string TargetEntryDoorDir { get; set; } = "";
    public string TargetExitDoorDir { get; set; } = "";
    public string EnterSceneName { get; private set; } = "";
    public string EnterDoorName { get; set; } = "";
    public bool IsEnteredMaze { get; private set; }

    public bool IsOutsideMaze => !IsEnteredMaze && CurrentSceneName != MazeExitSceneName &&
                                 CurrentSceneName != MazeRestSceneName &&
                                 !SceneNames.Contains(CurrentSceneName);

    public int LastRandomValue { get; set; } = -1;

    public readonly Dictionary<TransitionPoint, string> ChangedTransitionPoint = new();
    // ReSharper restore MemberCanBePrivate.Global

    private Scene _currentScene;

    private static readonly string[] SceneNames =
    [
        "Dust_Maze_01", "Dust_Maze_02", "Dust_Maze_03", "Dust_Maze_04", "Dust_Maze_05", "Dust_Maze_06", "Dust_Maze_07",
        "Dust_Maze_08"
    ];

    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static AlwaysMistController? Instance { get; private set; }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Utils.Logger.Debug($"OnSceneLoaded: {scene.name}");
        CurrentSceneName = scene.name;
        _currentScene = scene;
        StartCoroutine(OnSceneLoadedDelay());
    }

    private IEnumerator OnSceneLoadedDelay()
    {
        yield return new WaitForSeconds(0.2f);

        EnterMazeHandle();

        if (Main.TrueAlwaysMist is { Value: true })
        {
            if (IsOutsideMaze)
            {
                EnterSceneName = CurrentSceneName;
                ChangeAllTransitionScene();
            }
            else
                switch (CurrentSceneName)
                {
                    case MazeEntranceSceneName:
                        Utils.Logger.Debug($"EnterSceneName: {EnterSceneName}");
                        Utils.Logger.Debug($"EnterDoorName: {EnterDoorName}");
                        ChangeTransitionScene(RightTransitionPoint, EnterSceneName, EnterDoorName);
                        break;
                    case MazeExitSceneName:
                        Utils.Logger.Debug($"TargetSceneName: {TargetSceneName}");
                        Utils.Logger.Debug($"TargetExitDoorDir: {TargetExitDoorDir}");
                        if (string.IsNullOrEmpty(TargetSceneName)) break;
                        ChangeExitTransitionScene(TargetSceneName, TargetExitDoorDir);
                        break;
                }
        }
        else if (CurrentSceneName is ChangeScene or ChangeScene2)
        {
            EnterSceneName = CurrentSceneName;
            ChangeTransitionScene(LeftTransitionPoint);
        }
    }

    private void EnterMazeHandle()
    {
        if (CurrentSceneName == MazeEntranceSceneName)
            IsEnteredMaze = true;
        else if (IsEnteredMaze)
        {
            if (SceneNames.Contains(CurrentSceneName) || CurrentSceneName == MazeEntranceSceneName ||
                CurrentSceneName == MazeExitSceneName || CurrentSceneName == MazeRestSceneName) return;
            IsEnteredMaze = false;
            LastRandomValue = -1;
            TargetSceneName = "";
            TargetEntryDoorDir = "";
            TargetExitDoorDir = "";
        }
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void ChangeTransitionScene(string doorName, string changeScene = MazeEntranceSceneName,
        string? exitDoorName = null)
    {
        var points = TransitionPoint.TransitionPoints.Where(door =>
            door.gameObject.scene == _currentScene && door.gameObject.name == doorName).ToList();
        if (points.Count < 1) return;

        var point = points[0];
        var targetScene = point.targetScene;

        Utils.Logger.Info($"Target Scene: {targetScene}");

        if (targetScene != changeScene)
        {
            Utils.Logger.Info($"Change target scene to: {changeScene}");
            point.SetTargetScene(changeScene);
        }

        if (!string.IsNullOrEmpty(exitDoorName) && point.entryPoint != exitDoorName)
            point.entryPoint = exitDoorName;

        if (Main.ResetMazeSaveData is { Value: true } || Main.TrueAlwaysMist is { Value: true })
            MazeController.ResetSaveData();
    }

    private void ChangeExitTransitionScene(string changeScene, string exitDoorName)
    {
        var points = TransitionPoint.TransitionPoints.Where(door =>
            door.gameObject.scene == _currentScene && door.targetScene == MazeOldTargetSceneName).ToList();
        if (points.Count < 1) return;

        var point = points[0];
        var targetScene = point.targetScene;

        Utils.Logger.Info($"Target Scene: {targetScene}");

        if (targetScene != changeScene)
        {
            Utils.Logger.Info($"Change target scene to: {changeScene}");
            point.SetTargetScene(changeScene);
        }

        if (!string.IsNullOrEmpty(exitDoorName) && point.entryPoint != exitDoorName)
            point.entryPoint = exitDoorName;

        if (Main.ResetMazeSaveData is { Value: true } || Main.TrueAlwaysMist is { Value: true })
            MazeController.ResetSaveData();
    }

    private void ChangeAllTransitionScene()
    {
        var points = TransitionPoint.TransitionPoints.Where(door => door.gameObject.scene == _currentScene).ToList();
        if (points.Count < 1) return;

        foreach (var point in points)
        {
            var targetScene = point.targetScene;

            Utils.Logger.Info($"Target Scene: {targetScene}");

            if (targetScene is MazeEntranceSceneName or MazeExitSceneName) continue;

            Utils.Logger.Info($"Change target scene to: {MazeEntranceSceneName}");
            point.SetTargetScene(MazeEntranceSceneName);

            if (!ChangedTransitionPoint.ContainsKey(point))
                ChangedTransitionPoint.Add(point, targetScene);
        }

        if (Main.ResetMazeSaveData is { Value: true } || Main.TrueAlwaysMist is { Value: true })
            MazeController.ResetSaveData();
    }
}