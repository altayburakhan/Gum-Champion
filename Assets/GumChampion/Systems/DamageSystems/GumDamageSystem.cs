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
    private readonly float _dropChance = 0.5f; // 5% chance to drop an attribute
    private int slowAmount;

    public DamageSystem(Contexts contexts)
    {
        _gums = contexts.game.GetGroup(GameMatcher.Gum);
        _enemies = contexts.game.GetGroup(GameMatcher.Enemy);
        _buffdrops = contexts.game.GetGroup(GameMatcher.BuffDrop);
        _context = contexts.game;
        _players = _context.GetGroup(GameMatcher.Player);
    }

    public void Execute()
    {
        foreach (var gum in _gums.GetEntities())
        {
            if (!gum.hasHitEnemies)
            {
                gum.AddHitEnemies(new List<GameEntity>());
            }
            
            foreach (var enemy in _enemies.GetEntities())
            {
                if (IsColliding(gum, enemy) && !gum.hitEnemies.value.Contains(enemy))
                {
                    enemy.ReplaceHealthComp(enemy.healthComp.value - gum.gum.damage);
                    enemy.ReplaceSpeed(enemy.speed.value - slowAmount);

                    if (enemy.healthComp.value <= 0)
                    {
                        float random = Random.value;
                        if (random <= _dropChance) // Check if the drop chance is met
                        {
                            DropRandomAttribute(enemy.position.value); // Drop a random attribute
                        }
                        
                        enemy.isIsDestroyed = true;
                        GameObject.Destroy(enemy.view.gameObject);
                        enemy.RemoveView();
                        enemy.Destroy();
                        //Debug.Log(random);
                    }

                    gum.hitEnemies.value.Add(enemy);

                    // Apply the gum effect based on the gum type
                    /*switch (gum.gum.buffType)
                    {
                        case BuffType.Slowdown:
                            var speed = enemy.speed;
                            speed.value -= gum.gum.slowdownAmount; // Decrease the enemy's speed by the specified amount
                            break;

                        // Add cases for other buff types if needed

                        default:
                            break;
                    }*/
                }
            }
        }
        foreach (var player in _players.GetEntities())
        {
            foreach (var buff in _buffdrops.GetEntities())
            {
                if (IsColliding(player, buff))
                {
                    // Apply the buff effect based on the buff type
                    switch (buff.buffDrop.buffType)
                    {
                        case BuffType.DamageIncrease:
                            var gumEntities = _context.GetGroup(GameMatcher.Gum).GetEntities();
                            if (gumEntities.Length > 0)
                            {
                                var randomGumEntity = gumEntities[Random.Range(0, gumEntities.Length)];
                                var gum = randomGumEntity.gum;
                                gum.damage += 5;
                            }
                            break;

                        case BuffType.Slowdown:
                            
                                slowAmount++;

                                break;

                        // Add cases for other buff types if needed

                        default:
                            break;
                    }

                    // Destroy the buff entity and its associated GameObject
                    GameObject.Destroy(buff.view.gameObject);
                    buff.Destroy();
                }
            }
        }
        
    }
    
    private bool IsColliding(GameEntity player, GameEntity buff)
    {
        // Check if the positions of the player and buff entities are close enough to be considered colliding
        float collisionThreshold = 1.0f;
        Vector3 playerPosition = player.position.value;
        Vector3 buffPosition = buff.position.value;
        float distance = Vector3.Distance(playerPosition, buffPosition);
        return distance < collisionThreshold;
    }
    
    private void DropRandomAttribute(Vector3 position)
    {
        BuffType[] buffTypes = (BuffType[])System.Enum.GetValues(typeof(BuffType));
        BuffType randomBuffType = buffTypes[Random.Range(0, buffTypes.Length)];

        var buffEntity = _context.CreateEntity();
        buffEntity.AddPosition(position);
        buffEntity.AddBuffDrop(randomBuffType);
        buffEntity.AddCollectible(true);

        // Create the prefab based on the buff type
        GameObject prefab = GetPrefabForBuffType(randomBuffType);
        if (prefab != null)
        {
            GameObject buffObject = GameObject.Instantiate(prefab, position, Quaternion.identity);
            buffEntity.AddView(buffObject);
        }
    }

    private GameObject GetPrefabForBuffType(BuffType buffType)
    {
        // Map each buff type to its corresponding prefab
        switch (buffType)
        {
            case BuffType.DamageIncrease:
                return Resources.Load<GameObject>("DamageBuff");

            case BuffType.Slowdown:
                return Resources.Load<GameObject>("SlowBuff");

            default:
                return null; // Return null if no prefab is found for the buff type
        }
    }
}
