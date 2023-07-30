//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public GumComponent gum { get { return (GumComponent)GetComponent(GameComponentsLookup.Gum); } }
    public bool hasGum { get { return HasComponent(GameComponentsLookup.Gum); } }

    public void AddGum(float newDamage, float newSlowdownAmount, float newKnockbackForce) {
        var index = GameComponentsLookup.Gum;
        var component = (GumComponent)CreateComponent(index, typeof(GumComponent));
        component.damage = newDamage;
        component.slowdownAmount = newSlowdownAmount;
        component.knockbackForce = newKnockbackForce;
        AddComponent(index, component);
    }

    public void ReplaceGum(float newDamage, float newSlowdownAmount, float newKnockbackForce) {
        var index = GameComponentsLookup.Gum;
        var component = (GumComponent)CreateComponent(index, typeof(GumComponent));
        component.damage = newDamage;
        component.slowdownAmount = newSlowdownAmount;
        component.knockbackForce = newKnockbackForce;
        ReplaceComponent(index, component);
    }

    public void RemoveGum() {
        RemoveComponent(GameComponentsLookup.Gum);
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

    static Entitas.IMatcher<GameEntity> _matcherGum;

    public static Entitas.IMatcher<GameEntity> Gum {
        get {
            if (_matcherGum == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Gum);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherGum = matcher;
            }

            return _matcherGum;
        }
    }
}
