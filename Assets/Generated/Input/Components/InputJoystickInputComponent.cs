//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class InputContext {

    public InputEntity joystickInputEntity { get { return GetGroup(InputMatcher.JoystickInput).GetSingleEntity(); } }
    public JoystickInputComponent joystickInput { get { return joystickInputEntity.joystickInput; } }
    public bool hasJoystickInput { get { return joystickInputEntity != null; } }

    public InputEntity SetJoystickInput(UnityEngine.Vector2 newValue) {
        if (hasJoystickInput) {
            throw new Entitas.EntitasException("Could not set JoystickInput!\n" + this + " already has an entity with JoystickInputComponent!",
                "You should check if the context already has a joystickInputEntity before setting it or use context.ReplaceJoystickInput().");
        }
        var entity = CreateEntity();
        entity.AddJoystickInput(newValue);
        return entity;
    }

    public void ReplaceJoystickInput(UnityEngine.Vector2 newValue) {
        var entity = joystickInputEntity;
        if (entity == null) {
            entity = SetJoystickInput(newValue);
        } else {
            entity.ReplaceJoystickInput(newValue);
        }
    }

    public void RemoveJoystickInput() {
        joystickInputEntity.Destroy();
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
public partial class InputEntity {

    public JoystickInputComponent joystickInput { get { return (JoystickInputComponent)GetComponent(InputComponentsLookup.JoystickInput); } }
    public bool hasJoystickInput { get { return HasComponent(InputComponentsLookup.JoystickInput); } }

    public void AddJoystickInput(UnityEngine.Vector2 newValue) {
        var index = InputComponentsLookup.JoystickInput;
        var component = (JoystickInputComponent)CreateComponent(index, typeof(JoystickInputComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceJoystickInput(UnityEngine.Vector2 newValue) {
        var index = InputComponentsLookup.JoystickInput;
        var component = (JoystickInputComponent)CreateComponent(index, typeof(JoystickInputComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveJoystickInput() {
        RemoveComponent(InputComponentsLookup.JoystickInput);
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
public sealed partial class InputMatcher {

    static Entitas.IMatcher<InputEntity> _matcherJoystickInput;

    public static Entitas.IMatcher<InputEntity> JoystickInput {
        get {
            if (_matcherJoystickInput == null) {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.JoystickInput);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherJoystickInput = matcher;
            }

            return _matcherJoystickInput;
        }
    }
}
