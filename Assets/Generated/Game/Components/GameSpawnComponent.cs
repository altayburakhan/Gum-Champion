//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public SpawnComponent spawn { get { return (SpawnComponent)GetComponent(GameComponentsLookup.Spawn); } }
    public bool hasSpawn { get { return HasComponent(GameComponentsLookup.Spawn); } }

    public void AddSpawn(UnityEngine.Vector3 newPosition) {
        var index = GameComponentsLookup.Spawn;
        var component = (SpawnComponent)CreateComponent(index, typeof(SpawnComponent));
        component.position = newPosition;
        AddComponent(index, component);
    }

    public void ReplaceSpawn(UnityEngine.Vector3 newPosition) {
        var index = GameComponentsLookup.Spawn;
        var component = (SpawnComponent)CreateComponent(index, typeof(SpawnComponent));
        component.position = newPosition;
        ReplaceComponent(index, component);
    }

    public void RemoveSpawn() {
        RemoveComponent(GameComponentsLookup.Spawn);
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

    static Entitas.IMatcher<GameEntity> _matcherSpawn;

    public static Entitas.IMatcher<GameEntity> Spawn {
        get {
            if (_matcherSpawn == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Spawn);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherSpawn = matcher;
            }

            return _matcherSpawn;
        }
    }
}
