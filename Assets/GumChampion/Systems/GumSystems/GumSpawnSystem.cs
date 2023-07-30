using Entitas;
using UnityEngine;

public class GumSpawnSystem : IExecuteSystem
{
        private readonly GameContext _context;
    private IGroup<GameEntity> _players;
    private IGroup<GameEntity> _cameras;
    private GameObject _gumPrefab;
    private float _spawnTimer = 0;
    private float _spawnInterval = 1; // Spawn a frostbolt every second(it will updated)

    public GumSpawnSystem(Contexts contexts, GameObject gumPrefab)// System Definitions there will be as a bridge for GameController.
    {
        _context = contexts.game; // Set the game context, which is used to create and manage entities in the game
        _players = _context.GetGroup(GameMatcher.Player); // Get a group of all entities that match the Player component
        _cameras = _context.GetGroup(GameMatcher.Camera); // Get a group of all entities that match the Camera component
        _gumPrefab = gumPrefab; // Set the frostbolt prefab, which is used to instantiate new frostbolt entities

    }

    public void Execute()
{
    _spawnTimer += Time.deltaTime; // Increase the spawn timer by the time passed since the last frame
    if (_spawnTimer >= _spawnInterval) // Check if the spawn timer has reached or exceeded the spawn interval
    {
        _spawnTimer = 0; // Reset the spawn timer
        var player = _players.GetSingleEntity(); // Get the player entity
        var camera = _cameras.GetSingleEntity().camera.camera; // Get the camera entity
        if (player != null && IsEnemyInCameraView(camera)) // Check if the player exists and if there's an enemy in the camera view
        {
            var gum = _context.CreateEntity(); // Create a new frostbolt entity
            var closestEnemy = GetClosestEnemy(player.position.value); // Get the closest enemy to the player
            gum.AddPrefab(_gumPrefab); // Add the frostbolt prefab to the frostbolt entity
            gum.AddPosition(player.position.value); // Set the position of the frostbolt to the player's position
            gum.AddSpeed(10f); // Set the speed of the frostbolt
            gum.AddGum(50,0f,0f); // Add a Frostbolt component with a damage of 50
            var frostboltGameObject = GameObject.Instantiate(gum.prefab.prefab, gum.position.value, Quaternion.identity); // Instantiate the frostbolt GameObject
            var direction = (closestEnemy.position.value - player.position.value).normalized; // Calculate the direction towards the closest enemy
            gum.AddDirection(direction); // Add the calculated direction to the frostbolt entity
            gum.AddView(frostboltGameObject); // Add a View component that references the frostbolt GameObject
            gum.isGumSkill = true; // Mark the frostbolt as a skill
            // frostbolt.AddTarget(closestEnemy); // Add a Target component to the frostbolt (commented out)
        }
    }
}

   
GameEntity GetClosestEnemy(Vector3 position) // Method to get the enemy entity that is closest to a given position
{
    GameEntity closestEnemy = null; // Initialize the closest enemy as null
    float closestDistance = float.MaxValue; // Initialize the closest distance as the maximum possible float value

    var enemies = _context.GetGroup(GameMatcher.Enemy).GetEntities(); // Get all enemy entities
    foreach (var enemy in enemies) // Iterate over each enemy
    {
        float distance = Vector3.Distance(position, enemy.position.value); // Calculate the distance between the given position and the enemy
        if (distance < closestDistance) // If this enemy is closer than the previous closest enemy
        {
            closestDistance = distance; // Update the closest distance
            closestEnemy = enemy; // Update the closest enemy
        }
    }

    return closestEnemy; // Return the closest enemy
}


private bool IsEnemyInCameraView(Camera camera)// Method to check if any enemy is in the camera's view

{
    
    var enemies = _context.GetGroup(GameMatcher.Enemy).GetEntities();// Get all entities that match the Enemy component
    
    foreach (var enemy in enemies)// Iterate over each enemy
    {
        
        Vector3 viewportPosition = camera.WorldToViewportPoint(enemy.position.value);// Convert the enemy's world position to a viewport position
        
        if (viewportPosition.x >= 0 && viewportPosition.x <= 1 && 
            viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
            viewportPosition.z >= 0)// Check if the viewport position is within the camera's view
        {
            
            return true;// If the enemy is within the camera's view, return true
        }
    }
    
    return false;// If no enemies are within the camera's view, return false
}
}
