using Entitas;
using UnityEngine;

public class GumSpawnSystem : IExecuteSystem
{
    private readonly GameContext _context;
    private IGroup<GameEntity> _players;
    private IGroup<GameEntity> _cameras;
    private readonly IGroup<GameEntity> _buffdrops;
    private GameObject _gumPrefab;
    private float _spawnTimer = 0;
    private float _spawnInterval = 1; // Spawn a gum every second (it will be updated)
    private int gumDamage = 40;
    private float gumSlowdownAmount = 0;
    private float gumKnockbackForce = 0;
    private int damageBuffValue;
    private float slowBuffValue;
    private float knockBuffValue;

    public GumSpawnSystem(Contexts contexts, GameObject gumPrefab)
    {
        _context = contexts.game;
        _players = _context.GetGroup(GameMatcher.Player);
        _cameras = _context.GetGroup(GameMatcher.Camera);
        _buffdrops = _context.GetGroup(GameMatcher.BuffDrop);
        _gumPrefab = gumPrefab;
    }

    public void Execute()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0;
            var player = _players.GetSingleEntity();
            var camera = _cameras.GetSingleEntity().camera.camera;
            if (player != null && IsEnemyInCameraView(camera))
            {
                var gum = _context.CreateEntity();
                if (player.hasDamageBuff)
                {
                    gum.AddDamageBuff(player.damageBuff.value);
                    damageBuffValue = gum.damageBuff.value;
                }
                if (player.hasSlowDownBuff)
                {
                    gum.AddSlowDownBuff(player.slowDownBuff.value);
                    slowBuffValue = gum.slowDownBuff.value;
                }
                if (player.hasKnockBackBuff)
                {
                    gum.AddKnockBackBuff(player.knockBackBuff.value);
                    knockBuffValue = gum.knockBackBuff.value;
                }

                var closestEnemy = GetClosestEnemy(player.position.value);
                gum.AddPrefab(_gumPrefab);
                gum.AddPosition(player.position.value);
                gum.AddSpeed(10f);
                gum.AddGum(gumDamage + damageBuffValue, gumSlowdownAmount + slowBuffValue, gumKnockbackForce + knockBuffValue);
                var gumGameObject = GameObject.Instantiate(gum.prefab.prefab, gum.position.value, Quaternion.identity);
                var direction = (closestEnemy.position.value - player.position.value).normalized;
                gum.AddDirection(direction);
                gum.AddView(gumGameObject);
                gum.isGumSkill = true;
            }
        }
    }

    GameEntity GetClosestEnemy(Vector3 position)
    {
        GameEntity closestEnemy = null;
        float closestDistance = float.MaxValue;

        var enemies = _context.GetGroup(GameMatcher.Enemy).GetEntities();
        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(position, enemy.position.value);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private bool IsEnemyInCameraView(Camera camera)
    {
        var enemies = _context.GetGroup(GameMatcher.Enemy).GetEntities();

        foreach (var enemy in enemies)
        {
            Vector3 viewportPosition = camera.WorldToViewportPoint(enemy.position.value);

            if (viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                viewportPosition.z >= 0)
            {
                return true;
            }
        }

        return false;
    }
}
