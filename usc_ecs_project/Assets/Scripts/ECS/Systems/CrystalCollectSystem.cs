using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class CrystalCollectSystem : ComponentSystem
{
    private float distanceThreshold = .5f;

    protected override void OnUpdate()
    {
        if (Game.IsGameOver()) 
            return;

        float3 playerPos = Game.GetPlayerPosition();
        Entities.WithAll<CrystalTag>().ForEach((Entity crystal, ref Translation position) =>
        {
            playerPos.y = position.Value.y;
            if (math.distance(position.Value, playerPos) <= distanceThreshold * 3)
            {
                PostUpdateCommands.DestroyEntity(crystal);
                Game.CollectCrystal();
            }
        });
    }
}