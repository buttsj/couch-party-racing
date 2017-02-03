using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScheme {

    private Dictionary<string, IControl> bindings = new Dictionary<string, IControl>();
    private string deviceName;
    private int joystickNumber;

    public string Name { get; set; }

    public ControlScheme(string deviceName) {
        this.deviceName = deviceName;
        ResetBindingsToDefault();
    }

    public ControlScheme(string deviceName, int joystickNumber) {
        this.deviceName = deviceName;
        this.joystickNumber = joystickNumber;
        ResetBindingsToDefault();
    }

    public void ResetBindingsToDefault() {
        if (deviceName == "Xbox") {
            bindings = new ControlBindings(joystickNumber).Xbox;
        } else if (deviceName == "Keyboard1") {
            bindings = new ControlBindings().Keyboard1;
        } else if (deviceName == "Keyboard2") {
            bindings = new ControlBindings().Keyboard2;
        }
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
            if (pair.Value.IsDown()) {
                list.Add(pair.Value);
            }
        }

        return list;
    }

    public bool GetAnyButton() {
        bool anyInput = false;

        foreach (var pair in bindings) {
            if (pair.Value.IsHeldDown()) {
                anyInput = true;
                break;
            }
        }

        return anyInput;
    }

    public bool GetAnyButtonDown() {
        bool anyInput = false;

        foreach (var pair in bindings) {
            if (pair.Value.IsDown()) {
                anyInput = true;
                break;
            }
        }

        return anyInput;
    }
}
