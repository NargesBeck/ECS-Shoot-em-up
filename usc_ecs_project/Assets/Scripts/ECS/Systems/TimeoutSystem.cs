using Unity.Entities;

public class TimeoutSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<Lifetime>().ForEach((Entity entity, ref Lifetime lifetime) =>
        {
            lifetime.Value -= Time.DeltaTime;
            if (lifetime.Value <= 0)
                PostUpdateCommands.DestroyEntity(entity);
        });
    }
}