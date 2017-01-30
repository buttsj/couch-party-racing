﻿using System;
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
}
