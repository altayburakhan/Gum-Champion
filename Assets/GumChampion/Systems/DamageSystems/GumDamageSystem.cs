using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class DamageSystem : IExecuteSystem
{
    private readonly IGroup<GameEntity> _gums;
    private readonly IGroup<GameEntity> _enemies;
    private readonly GameContext _context;
    private readonly float _dropChance = 0.5f; // 5% chance to drop an attribute

    public DamageSystem(Contexts contexts)
    {
        _gums = contexts.game.GetGroup(GameMatcher.Gum);
        _enemies = contexts.game.GetGroup(GameMatcher.Enemy);
        _context = contexts.game;
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
    }

    private bool IsColliding(GameEntity gum, GameEntity enemy)
    {
        float collisionThreshold = 1.0f;
        Vector3 gumPosition = gum.position.value;
        Vector3 enemyPosition = enemy.position.value;
        float distance = Vector3.Distance(gumPosition, enemyPosition);
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
        }
    }

    private GameObject GetPrefabForBuffType(BuffType buffType)
    {
        // Map each buff type to its corresponding prefab
        switch (buffType)
        {
            case BuffType.AreaDamage:
                return Resources.Load<GameObject>("AreaBuff");
            case BuffType.Knockback:
                return Resources.Load<GameObject>("KnockBuff");
            case BuffType.Slowdown:
                return Resources.Load<GameObject>("SlowBuff");
            case BuffType.DamageIncrease:
                return Resources.Load<GameObject>("DamageBuff");
           
            default:
                return null; // Return null if no prefab is found for the buff type
        }
    }

}
