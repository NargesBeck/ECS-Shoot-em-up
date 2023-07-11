using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class FacePlayerSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (Game.IsGameOver())
            return;

        float3 playerPos = Game.GetPlayerPosition();

        Entities.WithAll<EnemyTag>().ForEach((Entity entity, ref Translation trans, ref Rotation rot) =>
        {
            float3 direction = playerPos - trans.Value;
            direction.y = 0;
            rot.Value = quaternion.LookRotation(direction, math.up());
        });
    }
}