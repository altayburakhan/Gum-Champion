using Entitas;
using UnityEngine;

public class PlayerDamageSystem : IExecuteSystem
{
    private readonly GameContext _context;// To hold the GameContext
    private IGroup<GameEntity> _players;// To hold a group of player entities
    private IGroup<GameEntity> _enemies;// To hold a group of enemy entities
    private float _damageInterval = 1f; // Damage the player every second
    public bool isDead;
    public bool survived;
    private float _survivalTime = 40f;
    private float _currentTime = 0f;

    public PlayerDamageSystem(Contexts contexts)// Constructor for the PlayerDamageSystem class
    {
       
        _context = contexts.game; // Initialize the GameContext from the provided contexts
       
        _players = _context.GetGroup(GameMatcher.Player); // Get a group of all entities that match the Player component
       
        _enemies = _context.GetGroup(GameMatcher.Enemy); // Get a group of all entities that match the Enemy component
    }


    
    public void Execute()// Method to execute the system's logic
    {
       
        var player = _players.GetSingleEntity(); // Get the single player entity
    
        
        if (player != null)// If a player entity exists
        {
           
            foreach (var enemy in _enemies.GetEntities()) // Iterate over each enemy entity
            {
               
                if (IsColliding(player, enemy)) // If the player and enemy are colliding
                {
                   
                    if (!player.hasLastDamageTime || Time.time - player.lastDamageTime.value >= _damageInterval) // If the player hasn't been damaged recently or the time since the last damage is greater than or equal to the damage interval
                    {
                        
                        player.ReplaceHealthComp(player.healthComp.value - enemy.damage.value);// Reduce the player's health by the enemy's damage value

                        if (player.healthComp.value == 0)
                        {
                            isDead = true;
                        }
                        player.ReplaceLastDamageTime(Time.time); // Update the time the player was last damaged
                    }
                }
            }
        }
        _currentTime += Time.deltaTime;

        if (_currentTime >= _survivalTime && !isDead)
        {
            survived = true;
        }
    }


    private bool IsColliding(GameEntity player, GameEntity enemy)
    {
        float collisionThreshold = 1f; // Adjust this value based on your needs

        // Get the positions of the player and enemy
        Vector3 playerPosition = player.position.value;
        Vector3 enemyPosition = enemy.position.value;

        
        float distance = Vector3.Distance(playerPosition, enemyPosition);// Calculate the distance between the player and enemy

        
        return distance < collisionThreshold;// If the distance is less than a certain threshold, they are colliding
    }
}