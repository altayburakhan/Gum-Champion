using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public class PlayerComponent : IComponent
{
}

public class PlayerCmp : IComponent {
    public Vector3 position;
}