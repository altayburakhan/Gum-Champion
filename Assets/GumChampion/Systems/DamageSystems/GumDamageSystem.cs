using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class DamageSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _gums;
    private readonly IGroup<GameEntity> _enemies;
    private readonly IGroup<GameEntity> _buffdrops;
    private IGroup<GameEntity> _players;
    private readonly GameContext _context;
    private readonly float _dropChance = 0.5f;

    public DamageSystem(Contexts contexts)
    {
        _gums = contexts.game.GetGroup(GameMatcher.Gum);    // Get the group of Gum entities from the game context
        _enemies = contexts.game.GetGroup(GameMatcher.Enemy);    // Get the group of Enemy entities from the game context
        _buffdrops = contexts.game.GetGroup(GameMatcher.BuffDrop);    // Get the group of BuffDrop entities from the game context
        _context = contexts.game;    // Set the context to the game context
        _players = _context.GetGroup(GameMatcher.Player);// Get the group of Player entities from the game context
    }

    public void Execute()
    {
         // Iterate over each Gum entity in the _gums group
    foreach (var gum in _gums.GetEntities())
    {
        // Check if the gum entity does not have the HitEnemies component
        if (!gum.hasHitEnemies)
        {
            // Add an empty list of GameEntity to the HitEnemies component of the gum entity
            gum.AddHitEnemies(new List<GameEntity>());
        }

        // Iterate over each Enemy entity in the _enemies group
        foreach (var enemy in _enemies.GetEntities())
        {
            // Check if the gum entity is colliding with the enemy entity and the enemy is not already hit by the gum
            if (IsColliding(gum, enemy) && !gum.hitEnemies.value.Contains(enemy))
            {
                // Reduce the health of the enemy by the damage value of the gum
                enemy.ReplaceHealthComp(enemy.healthComp.value - gum.gum.damage);

                // Calculate the direction from the gum's position to the enemy's position and normalize it
                var direction = enemy.position.value - gum.position.value;
                direction.Normalize();

                // Reduce the speed of the enemy by the slowdownAmount value of the gum
                enemy.speed.value -= gum.gum.slowdownAmount;

                // Apply knockback force to the enemy by updating its position
                enemy.position.value = enemy.position.value + direction * gum.gum.knockbackForce;

                // Check if the enemy's health is less than or equal to 0
                if (enemy.healthComp.value <= 0)
                {
                    // Generate a random value
                    float random = Random.value;

                    // Check if the random value is less than or equal to the _dropChance value
                    if (random <= _dropChance)
                    {
                        // Drop a random attribute at the enemy's position
                        DropRandomAttribute(enemy.position.value);
                    }

                    // Mark the enemy as destroyed
                    enemy.isIsDestroyed = true;

                    // Destroy the enemy's view game object
                    GameObject.Destroy(enemy.view.gameObject);

                    // Remove the View component from the enemy entity
                    enemy.RemoveView();

                    // Destroy the enemy entity
                    enemy.Destroy();
                }

                // Add the enemy to the list of hit enemies in the gum entity
                gum.hitEnemies.value.Add(enemy);
            }
        }
    }

        foreach (var player in _players.GetEntities())
{
    // Iterate over each BuffDrop entity in the _buffdrops group
    foreach (var buff in _buffdrops.GetEntities())
    {
        // Check if the player entity is colliding with the buff entity
        if (IsColliding(player, buff))
        {
            // Apply the buff effect based on the buff type
            switch (buff.buffDrop.buffType)
            {
                // If the buff type is DamageIncrease
                case BuffType.DamageIncrease:
                    // Check if the player already has the DamageBuff component
                    if (player.hasDamageBuff)
                    {
                        // Increase the value of the player's DamageBuff component by 5
                        player.ReplaceDamageBuff(player.damageBuff.value + 5);
                    }
                    else
                    {
                        // Add a new DamageBuff component to the player entity with a value of 5
                        player.AddDamageBuff(5);
                    }
                    break;

                // If the buff type is Slowdown
                case BuffType.Slowdown:
                    // Check if the player already has the SlowDownBuff component
                    if (player.hasSlowDownBuff)
                    {
                        // Increase the value of the player's SlowDownBuff component by 0.5f
                        player.ReplaceSlowDownBuff(player.slowDownBuff.value + 0.5f);
                    }
                    else
                    {
                        // Add a new SlowDownBuff component to the player entity with a value of 0.1f
                        player.AddSlowDownBuff(0.1f);
                    }
                    break;

                // If the buff type is Knockback
                case BuffType.Knockback:
                    // Check if the player already has the KnockBackBuff component
                    if (player.hasKnockBackBuff)
                    {
                        // Increase the value of the player's KnockBackBuff component by 1
                        player.ReplaceKnockBackBuff(player.knockBackBuff.value + 1);
                    }
                    else
                    {
                        // Add a new KnockBackBuff component to the player entity with a value of 1
                        player.AddKnockBackBuff(1);
                    }
                    break;
            }

            // Destroy the buff's view game object
            GameObject.Destroy(buff.view.gameObject);

            // Destroy the buff entity
            buff.Destroy();
        }
    }
}

    }

    private bool IsColliding(GameEntity player, GameEntity buff)
    {
        // Set the collision threshold distance
        float collisionThreshold = 1.0f;

        // Get the position of the player entity
        Vector3 playerPosition = player.position.value;

        // Get the position of the buff entity
        Vector3 buffPosition = buff.position.value;

        // Calculate the distance between the player and buff positions
        float distance = Vector3.Distance(playerPosition, buffPosition);

        // Check if the distance is less than the collision threshold
        return distance < collisionThreshold;
    }

    private void DropRandomAttribute(Vector3 position)
    {
        // Get all possible buff types
        BuffType[] buffTypes = (BuffType[])System.Enum.GetValues(typeof(BuffType));

        // Select a random buff type from the available types
        BuffType randomBuffType = buffTypes[Random.Range(0, buffTypes.Length)];

        // Create a new buff entity in the game context
        var buffEntity = _context.CreateEntity();

        // Set the position component of the buff entity
        buffEntity.AddPosition(position);

        // Set the buff drop component of the buff entity with the random buff type
        buffEntity.AddBuffDrop(randomBuffType);

        // Get the prefab associated with the random buff type
        GameObject prefab = GetPrefabForBuffType(randomBuffType);

        // Check if a valid prefab is found
        if (prefab != null)
        {
            // Instantiate the prefab at the specified position
            GameObject buffObject = GameObject.Instantiate(prefab, position, Quaternion.identity);

            // Add the view component to the buff entity with the instantiated buff object
            buffEntity.AddView(buffObject);
        }
    }


    private GameObject GetPrefabForBuffType(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.DamageIncrease:
                return Resources.Load<GameObject>("DamageBuff");

            case BuffType.Slowdown:
                return Resources.Load<GameObject>("SlowBuff");

            case BuffType.Knockback:
                return Resources.Load<GameObject>("KnockBuff");
            default:
                return null;
        }
    }
}
