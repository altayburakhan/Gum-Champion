using Entitas;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;// Serialized field to assign the Enemy prefab in the Unity Inspector
    [SerializeField] private GameObject gumPrefab;// Serialized field to assign the Gum prefab in the Unity Inspector
    public GameObject player; // Reference to the Player GameObject
    private Systems _systems; // Reference to the Systems object that will manage all game systems
    public Joystick joystick; // Reference to the Joystick component for player input
    public Animator _animator;
   // private Contexts _contexts;
    void Start()
    {
        var contexts = Contexts.sharedInstance;
        _systems = new Feature("Systems")
            .Add(new InputSystem(contexts, joystick)) // Pass the Joystick script to the InputSystem
            .Add(new MoveSystem(contexts)) // Move System that moving the character.
            .Add(new RenderPositionSystem(contexts))
            .Add(new SpawnSystem(contexts, enemyPrefab))
            .Add(new ChaseSystem(contexts))
            .Add(new GumSpawnSystem(contexts, gumPrefab)) // Add this line
            .Add(new GumMoveSystem(contexts))
            .Add(new DamageSystem(contexts))
            .Add(new PlayerDamageSystem(contexts));
        
       _systems.Initialize();
       
        var playerEntity = contexts.game.CreateEntity();
        var cameraEntity = contexts.game.CreateEntity();
        var enemyEntity = contexts.game.CreateEntity();
        var gumEntity = contexts.game.CreateEntity();
        playerEntity.AddPosition(Vector3.zero);
        playerEntity.AddSpeed(5f);
        playerEntity.isPlayer = true;
        playerEntity.AddView(player);
        gumEntity.AddTarget(enemyEntity);
        cameraEntity.AddCamera(Camera.main);
        enemyEntity.AddTarget(playerEntity);
        gumEntity.isHasCollided = true;
        playerEntity.AddHealthComp(100);
        _animator = player.GetComponent<Animator>();

    }
    
    void Update()
    {
        _systems.Execute();

    }

}



