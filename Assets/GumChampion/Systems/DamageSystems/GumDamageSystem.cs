using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class DamageSystem : IExecuteSystem
{
    // Declare private read-only variables to hold groups of frostbolt and enemy entities
    private readonly IGroup<GameEntity> _frostbolts;
    private readonly IGroup<GameEntity> _enemies;

    // Constructor for the DamageSystem class
    public DamageSystem(Contexts contexts)
    {
        // Get a group of all entities that match the Frostbolt component
        _frostbolts = contexts.game.GetGroup(GameMatcher.Gum);
        // Get a group of all entities that match the Enemy component
        _enemies = contexts.game.GetGroup(GameMatcher.Enemy);
    }
    
    
    public void Execute()// Method to execute the system's logic
    {
        
        foreach (var frostbolt in _frostbolts.GetEntities())// Iterate over each frostbolt entity
        {
            
            if (!frostbolt.hasHitEnemies)// If the frostbolt has not hit any enemies yet, add an empty list of hit enemies
            {
                frostbolt.AddHitEnemies(new List<GameEntity>());
            }
            
            foreach (var enemy in _enemies.GetEntities())// Iterate over each enemy entity
            {
                
                if (IsColliding(frostbolt, enemy) && !frostbolt.hitEnemies.value.Contains(enemy))// If the frostbolt is colliding with the enemy and the enemy is not already in the list of hit enemies
                {
                   
                    enemy.ReplaceHealthComp(enemy.healthComp.value - frostbolt.gum.damage); // Reduce the enemy's health by the frostbolt's damage
                    
                    if (enemy.healthComp.value <= 0)// If the enemy's health is 0 or less
                    {
                        
                        enemy.isIsDestroyed = true;// Mark the enemy as destroyed
                        
                        GameObject.Destroy(enemy.view.gameObject);// Destroy the enemy's associated GameObject
                       
                        enemy.RemoveView(); // Remove the View component from the enemy entity
                       
                        enemy.Destroy(); // Destroy the enemy entity
                    }
                    
                    frostbolt.hitEnemies.value.Add(enemy);// Add the enemy to the list of enemies hit by the frostbolt
                }
            }
        }
    }


    
    private bool IsColliding(GameEntity gum, GameEntity enemy)
    {
        
        float collisionThreshold = 1.0f;// Define a collision threshold for determining if the frostbolt and enemy are colliding

       
        Vector3 frostboltPosition = gum.position.value; // Get the position of the frostbolt entity

       
        Vector3 enemyPosition = enemy.position.value; // Get the position of the enemy entity

       
        float distance = Vector3.Distance(frostboltPosition, enemyPosition); // Calculate the distance between the frostbolt and enemy entities

        
        return distance < collisionThreshold;// Return true if the distance between the frostbolt and enemy is less than the collision threshold, indicating a collision
    }

}