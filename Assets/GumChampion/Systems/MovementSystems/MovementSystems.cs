
using Entitas;
using UnityEngine;

public class InputSystem : IExecuteSystem
{
    
    private readonly InputContext _context; // Declare a private variable to hold the InputContext
    private readonly Joystick _joystick;    // Declare a private variable to hold the Joystick


    
    public InputSystem(Contexts contexts, Joystick joystick)  // Constructor for the InputSystem class
    {
        
        _context = contexts.ınput;  // Initialize the InputContext from the provided contexts
        _joystick = joystick;  // Initialize the Joystick from the provided joystick

    }

    
    public void Execute() // Method to execute the system's logic
    {
        
        Vector2 joystickInput = _joystick.Direction;// Get the direction of the joystick input
       
        _context.ReplaceJoystickInput(joystickInput); // Replace the current joystick input in the context with the new input
    }
}


public class MoveSystem : IExecuteSystem
{
    
    private readonly GameContext _context;// Declare a private variable to hold the GameContext
   
    private readonly InputContext _inputContext; // Declare a private variable to hold the InputContext
   
    private IGroup<GameEntity> _players; // Declare a private variable to hold a group of player entities

   
    public MoveSystem(Contexts contexts) // Constructor for the MoveSystem class
    {
        _context = contexts.game;   // Initialize the GameContext from the provided contexts
        
        _inputContext = contexts.ınput;// Initialize the InputContext from the provided contexts
        
        _players = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.Position, GameMatcher.Speed));// Get a group of all entities that match the Player, Position, and Speed components
    }

   
    public void Execute() // Method to execute the system's logic
    {
       
        if (_inputContext.hasJoystickInput) // If there is joystick input
        {
            
            Vector2 joystickInput = _inputContext.joystickInput.value;// Get the value of the joystick input

           
            foreach (var e in _players.GetEntities()) // Iterate over each player entity
            {
               
                Vector3 newPosition = e.position.value + new Vector3(joystickInput.x, 0, joystickInput.y) * e.speed.value * Time.deltaTime; // Calculate the new position based on the joystick input, speed of the entity, and the time since the last frame
                
                e.ReplacePosition(newPosition);// Replace the current position of the entity with the new position
            }
        }
    }
}


public class RenderPositionSystem : IExecuteSystem
{
    
    private readonly IGroup<GameEntity> _entities;// Declare a private variable to hold a group of game entities

   
    public RenderPositionSystem(Contexts contexts) // Constructor for the RenderPositionSystem class
    {
       
        _entities = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.View)); // Get a group of all entities that match the Position and View components
    }

    
    public void Execute()// Method to execute the system's logic
    {
        
        foreach (var e in _entities.GetEntities())// Iterate over each entity in the group
        {
            
            e.view.gameObject.transform.position = e.position.value;// Update the position of the entity's associated GameObject to match the entity's position
        }
    }
}





