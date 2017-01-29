using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControl : IControl {

    private string virtualAction;
    private int joystickNumber;

    public JoystickControl(string buttonName, int joystickNumber) {
        this.joystickNumber = joystickNumber;

        virtualAction = ConvertToPlayerSpecific(buttonName);
    }

    public bool IsHeldDown() {
        return Input.GetKey(virtualAction);
    }

    public bool IsDown() {
        return Input.GetKeyDown(virtualAction);
    }

    public bool IsUp() {
        return Input.GetKeyUp(virtualAction);
    }

    public float GetValue() {
        return Input.GetAxis(virtualAction);
    }

    public float GetValueRaw() {
        return Input.GetAxisRaw(virtualAction);
    }

    public override string ToString() {
        return virtualAction;
    }

    private string ConvertToPlayerSpecific(string buttonName) {
        return "joystick " + joystickNumber + " " + buttonName;
    }
}
