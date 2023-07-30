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
                    var direction = enemy.position.value - gum.position.value;
                    direction.Normalize();

                    if (enemy.healthComp.value <= 0)
                    {
                        float random = Random.value;
                        if (random <= _dropChance)
                        {
                            DropRandomAttribute(enemy.position.value);
                        }

                        enemy.isIsDestroyed = true;
                        GameObject.Destroy(enemy.view.gameObject);
                        enemy.RemoveView();
                        enemy.Destroy();
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
                    switch (buff.buffDrop.buffType)
                    {
                        case BuffType.DamageIncrease:
                            if (player.hasDamageBuff)
                            {
                                player.ReplaceDamageBuff(player.damageBuff.value + 5);
                            }
                            else
                            {
                                player.AddDamageBuff(5);
                            }
                            break;
                        case BuffType.Slowdown:
                            if (player.hasSlowDownBuff)
                            {
                                player.ReplaceSlowDownBuff(player.slowDownBuff.value + 0.1f);
                            }
                            else
                            {
                                player.AddSlowDownBuff(0.1f);
                            }
                            break;
                        case BuffType.Knockback:
                            if (player.hasKnockBackBuff)
                            {
                                player.ReplaceKnockBackBuff(player.knockBackBuff.value + 1);
                            }
                            else
                            {
                                player.AddKnockBackBuff(1);
                            }
                            break;
                    }

                    GameObject.Destroy(buff.view.gameObject);
                    buff.Destroy();
                }
            }
        }
    }

    private bool IsColliding(GameEntity player, GameEntity buff)
    {
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

        GameObject prefab = GetPrefabForBuffType(randomBuffType);
        if (prefab != null)
        {
            GameObject buffObject = GameObject.Instantiate(prefab, position, Quaternion.identity);
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
