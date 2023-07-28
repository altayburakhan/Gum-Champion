using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

/*public class GumMoveSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _gums;// Declare a private read-only variable to hold a group of Gum entities

    
    public GumMoveSystem(Contexts contexts) // Constructor for the MoveGumSystem class

    {
        
        _gums = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Speed, GameMatcher.Direction));// Get a group of all entities that have Position, Speed, and Direction components
    }

    public void Execute()
    {
        
        foreach (var e in _gums.GetEntities())// Iterate over each gum entity
        {
            Vector3 direction;
           
            if (e.hasDirection) // If the entity has a Direction component, get its value
            {
                direction = e.direction.value;
            }
            else
            {
                
                continue;// Skip this iteration if the entity doesn't have a Direction component
            }
            
            var newPosition = e.position.value + direction * e.speed.value * Time.deltaTime;// Calculate the new position of the entity based on its current position, direction, speed, and the time since the last frame
            
            e.ReplacePosition(newPosition);// Replace the entity's current position with the new position
        }
    }
}*/
using UnityEngine;
using Entitas;

using UnityEngine;
using Entitas;

/*public class GumMoveSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _gums;
    private readonly GameContext _context;
    
    public GumMoveSystem(Contexts contexts)
    {
        _gums = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Speed, GameMatcher.Direction, GameMatcher.View));
        _context = contexts.game;
    }

    public void Execute()
    {
        foreach (var e in _gums.GetEntities())
        {
            Vector3 direction;
           
            if (e.hasDirection)
            {
                direction = e.direction.value;
            }
            else
            {
                continue;
            }
            
            var newPosition = e.position.value + direction * e.speed.value * Time.deltaTime;
            
            e.ReplacePosition(newPosition);
            
            if (!IsInsideViewport(newPosition))
            {
                if (e.hasView)
                {
                    GameObject.Destroy(e.view.gameObject, 1f); // Destroy the gum's associated GameObject after 1 second
                }
                
                e.isIsDestroyed = true; // Mark the gum entity as destroyed
            }
        }
        
        // Remove the destroyed gum entities from the context
        foreach (var e in _context.GetEntities(GameMatcher.IsDestroyed))
        {
            /*_context.Destroy(e);#1#
            e.Destroy();
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
}*/
using UnityEngine;
using Entitas;

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





