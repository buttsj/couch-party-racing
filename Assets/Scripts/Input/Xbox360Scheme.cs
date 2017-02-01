using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xbox360Scheme : IControlScheme {

    public string A { get { return "button 0"; }}
    public string B { get { return "button 1"; }}
    public string X { get { return "button 2"; }}
    public string Y { get { return "button 3"; }}
    public string LBumper { get { return "button 4"; }}
    public string RBumper { get { return "button 5"; }}
    public string Back { get { return "button 6"; }}
    public string Start { get { return "button 7"; }}
    public string LStickClick { get { return "button 8"; }}
    public string RStickClick { get { return "button 9"; }}
    public string LStickX { get { return "X Axis"; }}
    public string LStickY { get { return "Y Axis"; }}
    public string LTrigger { get { return "9th Axis"; } }
    public string RTrigger { get { return "10th Axis"; } }
    public string Triggers { get { return "3rd Axis"; } }
    public string RStickX { get { return "4th Axis"; }}
    public string RStickY { get { return "5th Axis"; }}
    public string DPadX { get { return "6th Axis"; } }
    public string DPadY { get { return "7th Axis"; } }


    private Dictionary<string, IControl> bindings = new Dictionary<string, IControl>();
    private int joystickNumber;

    public string Name { get; set; }

    public Xbox360Scheme(int joystickNumber) {
        this.joystickNumber = joystickNumber;
        ResetBindingsToDefault();
    }

    public void ResetBindingsToDefault() {
        bindings.Clear();

        bindings.Add("Horizontal", new JoystickControl(LStickX, joystickNumber));
        bindings.Add("Vertical", new JoystickControl(LStickY, joystickNumber));
        bindings.Add("Accelerate", new JoystickControl(A, joystickNumber));
        bindings.Add("Reverse", new JoystickControl(X, joystickNumber));
        bindings.Add("Use PowerUp", new JoystickControl(B, joystickNumber));
        bindings.Add("Reset Rotation", new JoystickControl(Back, joystickNumber));
        bindings.Add("Pause", new JoystickControl(Start, joystickNumber));
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
