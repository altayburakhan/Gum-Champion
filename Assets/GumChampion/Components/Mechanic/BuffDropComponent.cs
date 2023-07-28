using Entitas;

[Game]
public class BuffDropComponent : IComponent
{
    public BuffType buffType;
}
public enum BuffType
{
    AreaDamage,
    Knockback,
    Slowdown,
    DamageIncrease
}
