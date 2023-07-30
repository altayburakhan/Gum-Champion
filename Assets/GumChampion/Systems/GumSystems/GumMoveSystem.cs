
using UnityEngine;
using Entitas;



public class GumMoveSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _gums;
    private readonly Contexts _contexts;

    public GumMoveSystem(Contexts contexts)
    {
        _gums = contexts.game.GetGroup(GameMatcher.Gum);
        _contexts = contexts;
    }

    public void Execute()
    {
        foreach (var e in _gums.GetEntities())
        {
            var direction = e.direction.value;

            var newPosition = e.position.value + direction * e.speed.value * Time.deltaTime;

            e.ReplacePosition(newPosition);

            if (!IsInsideViewport(newPosition))
            {
                if (!e.isIsDestroyed)
                {
                    e.isIsDestroyed = true; // Mark the gum entity as destroyed
                    e.AddDelayedDestroy(1f); // Add a DelayedDestroy component with a delay of 1 second
                }
            }
        }

        // Remove the destroyed gum entities from the context
        foreach (var e in _contexts.game.GetEntities(GameMatcher.DelayedDestroy))
        {
            if (e.delayedDestroy.delay <= 0)
            {
                if (e.hasView)
                {
                    GameObject.Destroy(e.view.gameObject); // Destroy the associated GameObject
                }
                e.Destroy(); // Destroy the gum entity
            }
            else
            {
                e.ReplaceDelayedDestroy(e.delayedDestroy.delay - Time.deltaTime);
            }
        }
    }

    private bool IsInsideViewport(Vector3 position)
    {
        Camera camera = Camera.main;
        Vector3 viewportPosition = camera.WorldToViewportPoint(position);

        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
               viewportPosition.z >= 0;
    }
}





