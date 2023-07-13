using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

public class CrystalSpawner : MonoBehaviour
{
    private EntityManager entityManager;
    private Entity crystalEntityPrefab;

    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private GameObject crystalPrefabNonECS;


    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // settings used to convert GameObject prefab
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        crystalEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(crystalPrefab, settings);
    }

    public void SpawnCrystal(float3 pos)
    {
        if (Game.ECSActive)
        {
            NativeArray<Entity> enemyArray = new NativeArray<Entity>(1, Allocator.Temp);
            for (int i = 0; i < enemyArray.Length; i++)
            {
                enemyArray[i] = entityManager.Instantiate(crystalEntityPrefab);
                entityManager.SetComponentData(enemyArray[i], new Translation
                {
                    Value = pos
                });
            }        
            enemyArray.Dispose();
        }
        else
        {
            Instantiate(crystalPrefabNonECS).transform.position = pos;
        }
    }
}