using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public class HealthComponent : IComponent
{
    
}

[Game, Unique]
public sealed class HealthComp : IComponent
{
    public float value;
}


