using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class MoveFrostboltSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _frostbolts;// Declare a private read-only variable to hold a group of frostbolt entities

    
    public MoveFrostboltSystem(Contexts contexts) // Constructor for the MoveFrostboltSystem class

    {
        
        _frostbolts = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Speed, GameMatcher.Direction));// Get a group of all entities that have Position, Speed, and Direction components
    }

    public void Execute()
    {
        
        foreach (var e in _frostbolts.GetEntities())// Iterate over each frostbolt entity
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
}

