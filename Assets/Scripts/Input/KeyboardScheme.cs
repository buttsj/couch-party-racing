using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardScheme : IControlScheme {

    private Dictionary<string, IControl> bindings = new Dictionary<string, IControl>();

    public string Name { get; set; }

    public KeyboardScheme() {
        ResetBindingsToDefault();
    }

    public void ResetBindingsToDefault() {
        bindings.Clear();

        bindings.Add("Horizontal", new KeyboardControl(KeyCode.A, KeyCode.D));
        bindings.Add("Vertical", new KeyboardControl(KeyCode.Z, KeyCode.Q));
        bindings.Add("Accelerate", new KeyboardControl(KeyCode.W));
        bindings.Add("Reverse", new KeyboardControl(KeyCode.S));
        bindings.Add("Use PowerUp", new KeyboardControl(KeyCode.E));
        bindings.Add("Reset Rotation", new KeyboardControl(KeyCode.B));
        bindings.Add("Pause", new KeyboardControl(KeyCode.Return));
        bindings.Add("Bump Kart", new KeyboardControl(KeyCode.Space));
    }

    public float GetAxis(string axisName) {
        return bindings[axisName].GetValue();
    }

    public float GetAxisRaw(string axisName) {
        return bindings[axisName].GetValueRaw();
    }

    public bool GetButton(string buttonName) {
        return bindings[buttonName].IsHeldDown();
    }

    public bool GetButtonDown(string buttonName) {
        return bindings[buttonName].IsDown();
    }

    public bool GetButtonUp(string buttonName) {
        return bindings[buttonName].IsUp();
    }

    public IControl GetBinding(string bindingName) {
        return bindings[bindingName];
    }

    public List<IControl> GetAllInput() {
        List<IControl> list = new List<IControl>();

        foreach (var pair in bindings) {
            if(pair.Value.IsDown()) {
                list.Add(pair.Value);
            }
        }

        return list;
    }

    public bool GetAnyButton() {
        bool anyInput = false;

        foreach(var pair in bindings) {
            if(pair.Value.IsDown()) {
                anyInput = true;
                break;
            }
        }

        return anyInput;
    }
}
