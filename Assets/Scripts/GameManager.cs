using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action OnGameEvent;
    public event Action OnGameOverEvent;

    [SerializeField] private List<InitializePoolData> _initializePool = new List<InitializePoolData>();
    [SerializeField] private Transform _parentForPool;

    [SerializeField] private CoinManager _coinSpawner;
    [SerializeField] private EnemySpawner _enemySpawner;

    private GameState _state = GameState.MainMenu;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PreparePool();
        PlayerManager.Instance.EndingTakeStartingPositionEvent += SpawnObjects;
    }

    public void SwitchStateTo(GameState state) => UpdateState(state);


    private void UpdateState(GameState state)
    {
        _state = state;

        switch (_state)
        {
            case GameState.MainMenu:
                OnMainMenu();
                break;
            case GameState.Game:
                OnGame();
                break;
            case GameState.GameOverMenu:
                OnDeathMenu();
                break;
        }
    }

    private void OnMainMenu()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnGame()
    {
        PlayerManager.Instance.TakeStartingPosition();
    }

    private void OnDeathMenu()
    {
        _enemySpawner.StopEnemy();
        PlayerManager.Instance.Death();
        OnGameOverEvent?.Invoke();
    }

    private void SpawnObjects()
    {
        _coinSpawner.SpawnRandomCoin();
        _enemySpawner.MoveOnPath();
        OnGameEvent?.Invoke();
        AudioManager.Instance.PlayMusic(MusicType.MainGameMusic);
    }

    private void PreparePool()
    {
        PoolManager.SetParentForPoolObjects(_parentForPool);
        foreach (var poolData in _initializePool)
            PoolManager.InitializePool(poolData.Prefab, poolData.Count);
    }

    ~GameManager()
    {
        if (OnGameEvent != null)
        {
            foreach (Delegate d in OnGameEvent.GetInvocationList())
                OnGameEvent -= (Action)d;
        }

        if (OnGameOverEvent != null)
        {
            foreach (Delegate d in OnGameOverEvent.GetInvocationList())
                OnGameOverEvent -= (Action)d;
        }
    }
}
