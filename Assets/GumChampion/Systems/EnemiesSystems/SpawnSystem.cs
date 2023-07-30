using UnityEngine;
using Entitas;

public class SpawnSystem : IExecuteSystem
{
   
    private readonly GameContext _context; // Declare a private variable to hold the GameContext
    private IGroup<GameEntity> _cameras;// Declare a private variable to hold a group of camera entities
    private GameObject[] _enemyPrefab;// Declare a private variable to hold the enemy prefab
    private float _spawnTimer = 0;// Declare a private variable to hold the spawn timer
    private float _spawnInterval = 4;// Declare a private variable to hold the spawn interval (in seconds)
    private int _spawnCount = 0;// Declare a private variable to hold the spawn count
    
    public SpawnSystem(Contexts contexts, GameObject[] enemyPrefab)// Constructor for the SpawnSystem class
    {
        _context = contexts.game;        // Initialize the GameContext from the provided contexts
        _cameras = _context.GetGroup(GameMatcher.Camera);// Get a group of all entities that match the Camera component
        _enemyPrefab = enemyPrefab;// Initialize the enemy prefab from the provided prefab
    }

    public void Execute()
    {
        
        _spawnTimer += Time.deltaTime;// Increase the spawn timer by the time since the last frame
        
        if (_spawnTimer >= _spawnInterval)// If the spawn timer is greater than or equal to the spawn interval
        {
           
            _spawnTimer = 0; // Reset the spawn timer
            
            var camera = _cameras.GetSingleEntity().camera.camera;  // Get the camera entity
            var enemy = _context.CreateEntity();  // Create a new enemy entity
            
            enemy.AddPrefab(_enemyPrefab[Random.Range(0,_enemyPrefab.Length)]);// Add the enemy prefab to the enemy entity
            enemy.AddPosition(GetRandomPositionOutsideViewport(camera));// Add a position to the enemy entity, the position is randomly generated outside the viewport
            enemy.AddEnemy(50, 10);// Add an Enemy component to the enemy entity with a health of 50 and damage of 10
            enemy.AddHealthComp(50);// Add a HealthComponent to the enemy entity with a health value of 50
            enemy.AddDamage(10);// Add a DamageComponent to the enemy entity with a damage value of 10
            var enemyGameObject = GameObject.Instantiate(enemy.prefab.prefab, enemy.position.value, Quaternion.identity);// Instantiate a new GameObject from the enemy prefab at the enemy's position with no rotation
            enemy.AddView(enemyGameObject); // Add a ViewComponent that references the enemy GameObject
            enemy.AddSpeed(2f); // Add a SpeedComponent with a speed of 5
            var player = _context.GetGroup(GameMatcher.Player).GetSingleEntity();// Get the player entity
            
            if (player != null)// If the player entity exists
            {
                enemy.AddTarget(player);// Add a Chase component to the enemy, setting the player as the target
            }
            
            _spawnCount++;// Increment the spawn count
            
            if (_spawnCount % 4 == 0 && _spawnInterval > 1)// If the spawn count is a multiple of 4 and the spawn interval is greater than 1
            {
                _spawnInterval -= 0.5f;  // Decrease the spawn interval by 0.5
            }

        }
    }
    private Vector3 GetRandomPositionOutsideViewport(Camera camera)// Method to get a random position outside the viewport
    {
        Vector3 position;
        do
        {
            Vector3 randomVector = new Vector3(// Create a random vector with x and z values between -20 and 20, and y value of 0
                Random.Range(-20, 20), // Adjust these values to fit your game
                0, // Set y to 0
                Random.Range(-20, 40)); // Adjust these values to fit your game
            
            position = new Vector3(camera.transform.position.x, 0f, camera.transform.position.z) + randomVector;// Calculate the position by adding the random vector to the camera's position
            
        } while (IsInsideViewport(camera, position));// Repeat the process until the position is outside the viewport
        
        return position;
    }

    
    private bool IsInsideViewport(Camera camera, Vector3 position)// Method to check if a position is inside the viewport
    {
        
        Vector3 viewportPosition = camera.WorldToViewportPoint(position);// Convert the position from world space to viewport space

        // Check if the viewport position is within the bounds of the viewport and in front of the camera
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
               viewportPosition.z >= 0; // Check if the position is in front of the camera
    }
    
}
