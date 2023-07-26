using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
[Input, Unique]
public class JoystickInputComponent : IComponent
{
    public Vector2 value;
}


[Game,Unique]
public class PositionComponent : IComponent
{
    public Vector3 value;
}

[Game]
public class SpeedComponent : IComponent
{
    public float value;
}
[Game]
public class ViewComponent : IComponent
{
    public GameObject gameObject;
}

[Game, Unique]
public class MovableComponent : IComponent
{
}
[Game, Unique]
public class InteractiveComponent : IComponent
{
}
