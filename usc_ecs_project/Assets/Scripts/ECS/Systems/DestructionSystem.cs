using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class DestructionSystem : ComponentSystem
{
    private float distanceThreshold = 2f;

    protected override void OnUpdate()
    {
        if (Game.IsGameOver())
            return;

        Entities.WithAll<EnemyTag>().ForEach((Entity enemy, ref Translation enemyPos) =>
        {
            float3 enemyPosition = enemyPos.Value;

            Entities.WithAll<BulletTag>().ForEach((Entity bullet, ref Translation bulletPos) =>
            {
                enemyPosition = new float3(enemyPosition.x, 0, enemyPosition.z);
                bulletPos.Value = new float3(bulletPos.Value.x, 0, bulletPos.Value.z);
                if (math.distance(enemyPosition, bulletPos.Value) <= distanceThreshold)
                {
                    PostUpdateCommands.DestroyEntity(enemy);
                    PostUpdateCommands.DestroyEntity(bullet);
                    Game.CreateCrystal(enemyPosition);
                }
            });
        });
    }
}