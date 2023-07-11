using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class MovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        float3 playerPos = Game.GetPlayerPosition();
        Entities.WithAll<MoveForward>().ForEach(
            (ref Translation trans, ref Rotation rot, ref MoveForward moveForward, ref LocalToWorld localToWorld) =>
            {
                trans.Value = math.lerp(trans.Value, playerPos, moveForward.speed * Time.DeltaTime * 0.3f);
            }
        );
    }
}