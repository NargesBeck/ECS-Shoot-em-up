using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Mathematics;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameState
{
    Ready,
    Starting,
    Playing,
    Over
}

public class Game : MonoBehaviour
{
    public static Game Instance;
    public static bool ECSActive = false;

    // simple UI elements to transition and show score/time
    [SerializeField] private Image screenFader;

    [SerializeField] private float delay = 2f;

    // current game state
    public GameState gameState;

    private Transform player;
    public EnemySpawner enemySpawner;
    private PlayerManager playerManager;
    public CrystalSpawner crystalSpawner;

    public int playerLevel = 0;
    private int collectedGemCount = 0;
    private int gemCountForNextLevel = 0;
    public AnimationCurve gemAmountPerLevel;
    private float LevelProgress => (float) collectedGemCount / gemCountForNextLevel;

    public int EnemyCount
    {
        get
        {
            if (ECSActive)
                return _queryEnemy.CalculateEntityCount();
            return BulletNonECSManager.all.Count + EnemyNonECSManager.all.Count;
        }
    }

    EntityManager _entityManager;
    EntityQuery _queryEnemy;

    private void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _queryEnemy = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<EnemyTag>());
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        playerManager = FindObjectOfType<PlayerManager>();
        player = playerManager.transform;
        enemySpawner = FindObjectOfType<EnemySpawner>();

        gameState = GameState.Ready;
    }

    public float PlayerSpeed
    {
        get => playerManager.MoveSpeed;
        set => playerManager.SetMoveSpeed(value);
    }

    public void StartGame()
    {
        gameState = GameState.Starting;
        StartCoroutine(MainGameLoopRoutine());
    }

    // main game loop
    private IEnumerator MainGameLoopRoutine()
    {
        enemySpawner.StartSpawn();
        yield return StartCoroutine(StartGameRoutine());
        yield return StartCoroutine(PlayGameRoutine());
        yield return StartCoroutine(EndGameRoutine());
    }

    // fade in and enable the player to start the game
    private IEnumerator StartGameRoutine()
    {
        playerManager.Setup();
        PlayerManager.EnablePlayer(true);
        screenFader?.CrossFadeAlpha(0f, delay, true);

        yield return new WaitForSeconds(delay);

        gameState = GameState.Playing;

        Instance.collectedGemCount = 0;
        Instance.gemCountForNextLevel = (int) Instance.gemAmountPerLevel.Evaluate(Instance.playerLevel + 1);
        UiGamePanel.UpdateLevelBar(Instance.playerLevel, Instance.LevelProgress);
    }

    // wait until EndGame is invoked externally
    private IEnumerator PlayGameRoutine()
    {
        enemySpawner?.StartSpawn();

        while (gameState == GameState.Playing)
            yield return null;
    }

    private IEnumerator EndGameRoutine()
    {
        // fade to black and wait
        screenFader?.CrossFadeAlpha(1f, delay, true);
        yield return new WaitForSeconds(delay);

        gameState = GameState.Ready;

        // restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // static methods to send messages to Entities
    public static Vector3 GetPlayerPosition()
    {
        if (Instance == null)
        {
            return Vector3.zero;
        }

        return (Instance.player != null) ? Instance.player.position : Vector3.zero;
    }

    // end the game (player has died)
    public static void EndGame()
    {
        if (Instance == null)
        {
            return;
        }

        PlayerManager.EnablePlayer(false);
        Instance.gameState = GameState.Over;
    }

    // is the game over?
    public static bool IsGameOver()
    {
        if (Instance == null)
        {
            return false;
        }

        return (Instance.gameState == GameState.Over);
    }

    public static void CreateCrystal(float3 position)
    {
        Instance.crystalSpawner.SpawnCrystal(position);
    }

    public static void CollectCrystal()
    {
        Instance.collectedGemCount++;
        if (Instance.collectedGemCount >= Instance.gemCountForNextLevel)
        {
            Instance.playerLevel++;
            Instance.collectedGemCount = 0;
            Instance.gemCountForNextLevel = (int) Instance.gemAmountPerLevel.Evaluate(Instance.playerLevel + 1);
        }

        UiGamePanel.UpdateLevelBar(Instance.playerLevel, Instance.LevelProgress);
    }
}