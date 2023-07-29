//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public GumTypesComponent gumTypes { get { return (GumTypesComponent)GetComponent(GameComponentsLookup.GumTypes); } }
    public bool hasGumTypes { get { return HasComponent(GameComponentsLookup.GumTypes); } }

    public void AddGumTypes(float newSlowdownAmount) {
        var index = GameComponentsLookup.GumTypes;
        var component = (GumTypesComponent)CreateComponent(index, typeof(GumTypesComponent));
        component.slowdownAmount = newSlowdownAmount;
        AddComponent(index, component);
    }

    public void ReplaceGumTypes(float newSlowdownAmount) {
        var index = GameComponentsLookup.GumTypes;
        var component = (GumTypesComponent)CreateComponent(index, typeof(GumTypesComponent));
        component.slowdownAmount = newSlowdownAmount;
        ReplaceComponent(index, component);
    }

    public void RemoveGumTypes() {
        RemoveComponent(GameComponentsLookup.GumTypes);
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

    static Entitas.IMatcher<GameEntity> _matcherGumTypes;

    public static Entitas.IMatcher<GameEntity> GumTypes {
        get {
            if (_matcherGumTypes == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.GumTypes);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherGumTypes = matcher;
            }

            return _matcherGumTypes;
        }
    }
}
