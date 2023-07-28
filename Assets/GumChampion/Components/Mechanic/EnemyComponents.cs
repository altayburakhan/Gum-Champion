using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

[Game]
public class EnemyComponent : IComponent
{
    public float health;
    public float damage;
}
[Game]
public class SpawnComponent : IComponent
{
    public Vector3 position;
}
[Game]
public class PrefabComponent : IComponent
{
    public GameObject prefab;
}
public class EnemyCmp : IComponent
{
    public float health;
    public Vector3 position;
}
[Game]
public class EnemyDropsComponent : IComponent
{
    public bool dropsBuff;
}



