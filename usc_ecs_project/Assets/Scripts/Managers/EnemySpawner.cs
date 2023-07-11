using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private int spawnCount = 30;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnRadius = 30f;

    // extra enemy increase each wave
    [SerializeField] private int difficultyBonus = 5;

    [Header("Enemy")]
    [SerializeField] float minSpeed = 4f;
    [SerializeField] float maxSpeed = 12f;

    public float Speed
    {
        get => (minSpeed + maxSpeed) / 2;
        set
        {
            minSpeed = math.abs(value - 2);
            maxSpeed = math.abs(value + 2);
        }
    }

    public int SpawnCount
    {
        get => spawnCount;
        set => spawnCount = value;
    }

    public float SpawnInterval
    {
        get => spawnInterval;
        set => spawnInterval = value;
    }

    private float spawnTimer;

    private bool canSpawn;

    private Entity enemyEntityPrefab;
    private EntityManager entityManager;

    [SerializeField] private GameObject quadEnemy;
    [SerializeField] private GameObject quadEnemyPrefabNonECS;

    private void SetConfig()
    {
        if (Game.ECSActive)
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);


            // convert the GameObject prefab into an Entity prefab and store it
            enemyEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(quadEnemy, settings);
        }
    }

    // spawns enemies in a ring around the player
    private void SpawnWave()
    {
        if (Game.ECSActive)
        {
            NativeArray<Entity> enemyArray = new NativeArray<Entity>(spawnCount, Allocator.Temp);

            for (int i = 0; i < enemyArray.Length; i++)
            {
                enemyArray[i] = entityManager.Instantiate(enemyEntityPrefab);
                entityManager.SetComponentData(enemyArray[i], new Translation
                {
                    Value = RandomPointOnCircle(spawnRadius)
                });

                entityManager.SetComponentData(enemyArray[i], new MoveForward
                {
                    speed = Random.Range(minSpeed, maxSpeed)
                });
            }

            enemyArray.Dispose();
        }
        else
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPoint = RandomPointOnCircle(spawnRadius);
                GameObject enemyInstance = Instantiate(quadEnemyPrefabNonECS, spawnPoint, quaternion.identity);

                EnemyNonECS enemyNonECS = enemyInstance.GetComponent<EnemyNonECS>();
                enemyNonECS.SetMoveSpeed(Random.Range(minSpeed, maxSpeed));
                EnemyNonECSManager.Add(enemyNonECS);
            }
        }
        spawnCount += difficultyBonus;
    }

    private float3 RandomPointOnCircle(float radius)
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * radius;
        return new float3(randomPoint.x, 0, randomPoint.y) + (float3) Game.GetPlayerPosition();
    }

    // signal from GameManager to begin spawning
    public void StartSpawn()
    {
        spawnTimer = spawnInterval;
        SetConfig();
        canSpawn = true;
    }

    private void Update()
    {
        if (!canSpawn || Game.IsGameOver())
            return;

        spawnTimer += Time.deltaTime;

        // spawn and reset timer
        if (spawnTimer > spawnInterval)
        {
            SpawnWave();
            spawnTimer = 0;
        }
    }
}