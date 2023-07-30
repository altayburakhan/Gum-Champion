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

public class GumTypesComponent : IComponent
{
    public float slowdownAmount;
    public float knockbackForce;
}

