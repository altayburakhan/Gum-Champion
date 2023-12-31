//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity damageEntity { get { return GetGroup(GameMatcher.Damage).GetSingleEntity(); } }
    public DamageComponent damage { get { return damageEntity.damage; } }
    public bool hasDamage { get { return damageEntity != null; } }

    public GameEntity SetDamage(float newValue) {
        if (hasDamage) {
            throw new Entitas.EntitasException("Could not set Damage!\n" + this + " already has an entity with DamageComponent!",
                "You should check if the context already has a damageEntity before setting it or use context.ReplaceDamage().");
        }
        var entity = CreateEntity();
        entity.AddDamage(newValue);
        return entity;
    }

    public void ReplaceDamage(float newValue) {
        var entity = damageEntity;
        if (entity == null) {
            entity = SetDamage(newValue);
        } else {
            entity.ReplaceDamage(newValue);
        }
    }

    public void RemoveDamage() {
        damageEntity.Destroy();
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public DamageComponent damage { get { return (DamageComponent)GetComponent(GameComponentsLookup.Damage); } }
    public bool hasDamage { get { return HasComponent(GameComponentsLookup.Damage); } }

    public void AddDamage(float newValue) {
        var index = GameComponentsLookup.Damage;
        var component = (DamageComponent)CreateComponent(index, typeof(DamageComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceDamage(float newValue) {
        var index = GameComponentsLookup.Damage;
        var component = (DamageComponent)CreateComponent(index, typeof(DamageComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveDamage() {
        RemoveComponent(GameComponentsLookup.Damage);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherDamage;

    public static Entitas.IMatcher<GameEntity> Damage {
        get {
            if (_matcherDamage == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Damage);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherDamage = matcher;
            }

            return _matcherDamage;
        }
    }
}
