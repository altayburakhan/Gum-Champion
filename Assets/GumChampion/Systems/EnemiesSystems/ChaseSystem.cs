using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class ChaseSystem : IExecuteSystem // Chasing the player.
{
    
    private readonly IGroup<GameEntity> _chasers;    // Declare a private readonly field to hold the group of entities that are enemies (_chasers)
    

    public ChaseSystem(Contexts contexts)  // Initialize the _chasers field by getting the group of entities that have Target, Position, and Speed components
    {
        _chasers = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Target, GameMatcher.Position, GameMatcher.Speed));
    }

    public void Execute()
    {
        
        foreach (var e in _chasers.GetEntities())// Iterate over all entities in the _chasers group
        {
           
            var target = e.target.target; // Get the target entity of the current entity
    
            
            if (target != null && target.hasPosition)// Check if the target entity exists and has a position
            {
                
                var step = e.speed.value * Time.deltaTime;// Calculate the step size based on the entity's speed and the time since the last frame

                e.ReplacePosition(Vector3.MoveTowards(e.position.value, target.position.value, step));// Move the entity towards the target's position by the calculated step size
                
            }
        }
        
    }
}
