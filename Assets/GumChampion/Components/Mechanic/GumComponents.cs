using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;


public class IsTargetableComponent : IComponent {}

public class GumSkill : IComponent
{
    
}
public sealed class GumComponent : IComponent
{
   
    public float damage;
}

[Game]
public class GumSpeedComponent : IComponent
{
    public float speedDecrease; 
    
}
