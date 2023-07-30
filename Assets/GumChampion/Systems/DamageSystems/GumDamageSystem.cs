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
    private readonly float _dropChance = 0.05f; // 5% chance to drop an attribute
    private int slowAmount;
    private float KnockbackAmount;

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
                    var (speed, position) = (enemy.speed.value, enemy.position.value);
                    var direction = position - gum.position.value;
                    direction.Normalize();
                    var knockbackForce = gum.gum.knockbackForce;
                    enemy.speed.value -= gum.gum.slowdownAmount; // Decrease the enemy's speed by the gum's speed decrease value
                    enemy.position.value += direction * knockbackForce; // Apply the knockback force to the enemy's position
                    
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

        // Create the prefab based on the buff type
        GameObject prefab = GetPrefabForBuffType(randomBuffType);
        if (prefab != null)
        {
            GameObject buffObject = GameObject.Instantiate(prefab, position, Quaternion.identity);
            buffEntity.AddView(buffObject);

            // Apply the buff effect based on the buff type
            switch (randomBuffType)
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
                    var gumSlowEntity = _context.GetGroup(GameMatcher.Gum).GetSingleEntity();
                    if (gumSlowEntity != null)
                    {
                        var gumComponent = gumSlowEntity.gum;
                        gumComponent.knockbackForce += 10f; // Increase the gum's knockback force by 10
                    }

                    break;

                case BuffType.Knockback:
                    var gumKnockEntity = _context.GetGroup(GameMatcher.Gum).GetSingleEntity();
                    if (gumKnockEntity != null)
                    {
                        var gum = gumKnockEntity.gum;
                        gum.knockbackForce += 10f; // Increase the gum's knockback force by 10
                    }
                    break;

                // Add cases for other buff types if needed

                default:
                    break;
            }
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
            
            case BuffType.Knockback:
                return Resources.Load<GameObject>("KnockBuff");
            default:
                return null; // Return null if no prefab is found for the buff type
        }
    }
    
}
