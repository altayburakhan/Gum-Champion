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

// Check if the spawn timer has reached the spawn interval
if (_spawnTimer >= _spawnInterval)
{
    _spawnTimer = 0;

    // Get the player entity
    var player = _players.GetSingleEntity();

    // Get the camera entity and its associated camera component
    var camera = _cameras.GetSingleEntity().camera.camera;

    // Check if the player entity exists and if there is an enemy in the camera view
    if (player != null && IsEnemyInCameraView(camera))
    {
        // Create a new gum entity
        var gum = _context.CreateEntity();

        // Check if the player has the DamageBuff component
        if (player.hasDamageBuff)
        {
            // Add the DamageBuff component to the gum entity with the same value as the player's DamageBuff component
            gum.AddDamageBuff(player.damageBuff.value);
            damageBuffValue = gum.damageBuff.value;
        }

        // Check if the player has the SlowDownBuff component
        if (player.hasSlowDownBuff)
        {
            // Add the SlowDownBuff component to the gum entity with the same value as the player's SlowDownBuff component
            gum.AddSlowDownBuff(player.slowDownBuff.value);
            slowBuffValue = gum.slowDownBuff.value;
        }

        // Check if the player has the KnockBackBuff component
        if (player.hasKnockBackBuff)
        {
            // Add the KnockBackBuff component to the gum entity with the same value as the player's KnockBackBuff component
            gum.AddKnockBackBuff(player.knockBackBuff.value);
            knockBuffValue = gum.knockBackBuff.value;
        }

        // Get the closest enemy to the player's position
        var closestEnemy = GetClosestEnemy(player.position.value);

        // Add the prefab component to the gum entity with the gum prefab
        gum.AddPrefab(_gumPrefab);

        // Add the position component to the gum entity with the player's position
        gum.AddPosition(player.position.value);

        // Add the speed component to the gum entity with a value of 10f
        gum.AddSpeed(10f);

        // Add the gum component to the gum entity with the gum damage, slowdown amount, and knockback force values
        gum.AddGum(gumDamage + damageBuffValue, gumSlowdownAmount + slowBuffValue, gumKnockbackForce + knockBuffValue);

        // Instantiate the gum prefab at the gum's position
        var gumGameObject = GameObject.Instantiate(gum.prefab.prefab, gum.position.value, Quaternion.identity);

        // Calculate the direction from the player to the closest enemy and normalize it
        var direction = (closestEnemy.position.value - player.position.value).normalized;

        // Add the direction component to the gum entity with the calculated direction
        gum.AddDirection(direction);

        // Add the view component to the gum entity with the instantiated gum game object
        gum.AddView(gumGameObject);

        // Set the gum entity as a gum skill entity
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
