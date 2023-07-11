using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float rateOfFire = 0.3f;

    [SerializeField] private Transform gunTransform;

    [SerializeField] private GameObject bulletPrefabECS;
    [SerializeField] private GameObject bulletPrefabNonECS;

    [Header("Effects")] 
    [SerializeField] private AudioSource soundFXSource;

    private EntityManager entityManager;
    private Entity bulletEntityPrefab;
    
    private float shotTimer;

    private void Update()
    {
        if (Game.Instance.gameState == GameState.Playing)
        {
            shotTimer += Time.deltaTime;
            if (shotTimer >= rateOfFire)
            {
                FireBullet();
                shotTimer = 0f;
            }
        }
    }

    public virtual void SetConfig()
    {
        if (Game.ECSActive)
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefabECS, settings);
        }
    }

    public virtual void FireBullet()
    {
        if (Game.ECSActive)
        {
            Entity bullet = entityManager.Instantiate(bulletEntityPrefab);
            entityManager.SetComponentData(bullet, new Translation { Value = gunTransform.position });
            entityManager.SetComponentData(bullet, new Rotation { Value = gunTransform.rotation });
        }
        else
        {
            Instantiate(bulletPrefabNonECS, gunTransform.position, gunTransform.rotation, null);
        }
        soundFXSource?.Play();
    }
}