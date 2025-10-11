using System.Collections;
using AlwaysMist.Datas;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlwaysMist.Behaviour;

public class AlwaysMistController : MonoBehaviour
{
    private const string ChangeScene = "Shadow_04";
    private const string ChangeScene2 = "Dust_05";
    public const string LeftTransitionPoint = "left1";
    public const string RightTransitionPoint = "right1";

    private static readonly string[] SceneNames =
    [
        "Dust_Maze_01", "Dust_Maze_02", "Dust_Maze_03", "Dust_Maze_04", "Dust_Maze_05", "Dust_Maze_06", "Dust_Maze_07",
        "Dust_Maze_08"
    ];

    private Scene _currentScene;

    public static AlwaysMistController? Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void GetSaveData(out AlwaysMistData data)
    {
        var gameManager = GameManager.instance;
        if (!gameManager)
        {
            data = new();
            return;
        }

        if (!SaveDatas.ContainsKey(gameManager.profileID))
            SaveDatas.Add(gameManager.profileID, new());
        data = SaveDatas[gameManager.profileID];
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Utils.Logger.Debug($"OnSceneLoaded: {scene.name}");
        CurrentSceneName = scene.name;
        _currentScene = scene;

        TurnOnRestBench();
        StartCoroutine(OnSceneLoadedDelay());
    }

    private IEnumerator OnSceneLoadedDelay()
    {
        yield return new WaitForSeconds(0.1f);

        EnterMazeHandle();

        if (Configs.TrueAlwaysMist)
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
                        switch (TargetEntryDoorDir)
                        {
                            case "left":
                                ChangeTransitionScene(RightTransitionPoint, EnterSceneName, EnterDoorName);
                                break;
                            case "right":
                                ChangeTransitionScene(LeftTransitionPoint, EnterSceneName, EnterDoorName);
                                break;
                        }

                        break;
                    case MazeExitSceneName:
                        Utils.Logger.Debug($"TargetSceneName: {TargetSceneName}");
                        Utils.Logger.Debug($"TargetExitDoorDir: {TargetExitDoorDir}");
                        if (string.IsNullOrEmpty(TargetSceneName)) break;
                        FixExitTransition(); // Still need to fix entryPoint
                        break;
                }
        }
        else if (CurrentSceneName is ChangeScene or ChangeScene2)
        {
            EnterSceneName = CurrentSceneName;
            ChangeTransitionScene(LeftTransitionPoint);
        }
    }

    private void TurnOnRestBench()
    {
        if (CurrentSceneName != MazeEntranceSceneName || (!Configs.RestBenchInMist && !Configs.TrueAlwaysMist)) return;
        var restBenches = FindObjectsByType<RestBench>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        Utils.Logger.Debug($"Find RestBenchs: {restBenches}");

        foreach (var restBench in restBenches)
        {
            var obj = restBench.gameObject;

            if (obj.tag != "RespawnPoint") continue;

            Utils.Logger.Debug($"Got RestBench: {obj}");

            if (obj && !obj.activeSelf)
                obj.SetActive(true);
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
        var point = TransitionPoint.TransitionPoints.FirstOrDefault(door =>
            door.gameObject.scene == _currentScene && door.gameObject.name == doorName);
        if (!point) return;

        var targetScene = point.targetScene;

        Utils.Logger.Info($"Target Scene: {targetScene}");

        if (targetScene != changeScene)
        {
            Utils.Logger.Info($"Change target scene to: {changeScene}");
            point.SetTargetScene(changeScene);
        }

        if (!string.IsNullOrEmpty(exitDoorName) && point.entryPoint != exitDoorName)
            point.SetTargetDoor(exitDoorName);

        if (Configs.ResetMazeSaveData || Configs.TrueAlwaysMist)
            MazeController.ResetSaveData();
    }

    private void FixExitTransition()
    {
        var point = TransitionPoint.TransitionPoints.FirstOrDefault(door =>
            door.gameObject.scene == _currentScene && door.targetScene == TargetSceneName);
        if (!point) return;
        point.SetTargetDoor(TargetExitDoorName);
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

        if (Configs.ResetMazeSaveData || Configs.TrueAlwaysMist)
            MazeController.ResetSaveData();
    }

    // ReSharper disable MemberCanBePrivate.Global
    public const string MazeEntranceSceneName = "Dust_Maze_09_entrance";
    public const string MazeExitSceneName = "Dust_Maze_Last_Hall";

    public const string MazeRestSceneName = "Dust_Maze_crossing";
    // ReSharper restore MemberCanBePrivate.Global

    // ReSharper disable MemberCanBePrivate.Global
    public string CurrentSceneName { get; private set; } = "";

    public string TargetSceneName
    {
        get
        {
            GetSaveData(out var data);
            return data.TargetSceneName;
        }
        set
        {
            GetSaveData(out var data);
            data.TargetSceneName = value;
        }
    }

    public string TargetEntryDoorDir
    {
        get
        {
            GetSaveData(out var data);
            return data.TargetEntryDoorDir;
        }
        set
        {
            GetSaveData(out var data);
            data.TargetEntryDoorDir = value;
        }
    }

    public string TargetExitDoorDir
    {
        get
        {
            GetSaveData(out var data);
            return data.TargetExitDoorDir;
        }
        set
        {
            GetSaveData(out var data);
            data.TargetExitDoorDir = value;
        }
    }

    public string TargetExitDoorName
    {
        get
        {
            GetSaveData(out var data);
            return data.TargetExitDoorName;
        }
        set
        {
            GetSaveData(out var data);
            data.TargetExitDoorName = value;
        }
    }

    public string EnterSceneName
    {
        get
        {
            GetSaveData(out var data);
            return data.EnterSceneName;
        }
        set
        {
            GetSaveData(out var data);
            data.EnterSceneName = value;
        }
    }

    public string EnterDoorName
    {
        get
        {
            GetSaveData(out var data);
            return data.EnterDoorName;
        }
        set
        {
            GetSaveData(out var data);
            data.EnterDoorName = value;
        }
    }

    public bool IsEnteredMaze
    {
        get
        {
            GetSaveData(out var data);
            return data.IsEnteredMaze;
        }
        set
        {
            GetSaveData(out var data);
            data.IsEnteredMaze = value;
        }
    }

    public bool IsOutsideMaze => !IsEnteredMaze && CurrentSceneName != MazeExitSceneName &&
                                 CurrentSceneName != MazeRestSceneName &&
                                 !SceneNames.Contains(CurrentSceneName);

    public int LastRandomValue
    {
        get
        {
            GetSaveData(out var data);
            return data.LastRandomValue;
        }
        set
        {
            GetSaveData(out var data);
            data.LastRandomValue = value;
        }
    }

    public readonly Dictionary<TransitionPoint, string> ChangedTransitionPoint = new();

    public readonly Dictionary<int, AlwaysMistData> SaveDatas = new();
    // ReSharper restore MemberCanBePrivate.Global
}