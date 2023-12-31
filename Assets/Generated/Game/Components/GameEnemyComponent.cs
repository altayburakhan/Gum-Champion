//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public EnemyComponent enemy { get { return (EnemyComponent)GetComponent(GameComponentsLookup.Enemy); } }
    public bool hasEnemy { get { return HasComponent(GameComponentsLookup.Enemy); } }

    public void AddEnemy(float newHealth, float newDamage) {
        var index = GameComponentsLookup.Enemy;
        var component = (EnemyComponent)CreateComponent(index, typeof(EnemyComponent));
        component.health = newHealth;
        component.damage = newDamage;
        AddComponent(index, component);
    }

    public void ReplaceEnemy(float newHealth, float newDamage) {
        var index = GameComponentsLookup.Enemy;
        var component = (EnemyComponent)CreateComponent(index, typeof(EnemyComponent));
        component.health = newHealth;
        component.damage = newDamage;
        ReplaceComponent(index, component);
    }

    public void RemoveEnemy() {
        RemoveComponent(GameComponentsLookup.Enemy);
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

    static Entitas.IMatcher<GameEntity> _matcherEnemy;

    public static Entitas.IMatcher<GameEntity> Enemy {
        get {
            if (_matcherEnemy == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Enemy);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherEnemy = matcher;
            }

            return _matcherEnemy;
        }
    }
}
