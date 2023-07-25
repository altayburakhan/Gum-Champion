//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public HitEnemiesComponent hitEnemies { get { return (HitEnemiesComponent)GetComponent(GameComponentsLookup.HitEnemies); } }
    public bool hasHitEnemies { get { return HasComponent(GameComponentsLookup.HitEnemies); } }

    public void AddHitEnemies(System.Collections.Generic.List<GameEntity> newValue) {
        var index = GameComponentsLookup.HitEnemies;
        var component = (HitEnemiesComponent)CreateComponent(index, typeof(HitEnemiesComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceHitEnemies(System.Collections.Generic.List<GameEntity> newValue) {
        var index = GameComponentsLookup.HitEnemies;
        var component = (HitEnemiesComponent)CreateComponent(index, typeof(HitEnemiesComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveHitEnemies() {
        RemoveComponent(GameComponentsLookup.HitEnemies);
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

    static Entitas.IMatcher<GameEntity> _matcherHitEnemies;

    public static Entitas.IMatcher<GameEntity> HitEnemies {
        get {
            if (_matcherHitEnemies == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.HitEnemies);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherHitEnemies = matcher;
            }

            return _matcherHitEnemies;
        }
    }
}
