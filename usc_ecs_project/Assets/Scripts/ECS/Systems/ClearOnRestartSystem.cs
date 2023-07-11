using Unity.Entities;

namespace DroneSwarm
{
    public class ClearOnRestartSystem : ComponentSystem
    {
        // approximate the screen fader transition time
        private float endLifetime = 2f;

        protected override void OnUpdate()
        {
            // check if the game is over
            if (Game.IsGameOver())
            {
                // default EntityManager
                EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

                // find all enemies that currently do not have the Lifetime ComponentData
                Entities.WithAll<EnemyTag>().WithNone<Lifetime>().ForEach((Entity enemy) =>
                {
                    // add the Lifetime component to time out Entity automatically
                    entityManager.AddComponentData(enemy, new Lifetime {Value = endLifetime});
                });
            }
        }
    }
}