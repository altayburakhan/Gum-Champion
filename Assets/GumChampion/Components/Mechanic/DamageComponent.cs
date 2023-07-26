using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique]
public sealed class DamageComponent : IComponent
{
    public float value;
}

