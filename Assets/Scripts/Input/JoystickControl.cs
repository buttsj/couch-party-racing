using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControl : IControl {

    private string virtualAction;
    private string overloadedAction;
    private int joystickNumber;
    private bool isButton;
    private bool isPositiveOverloadedAxis;

    public JoystickControl(string virtualAction, int joystickNumber) {
        this.joystickNumber = joystickNumber;
        isButton = virtualAction.Contains("button");

        this.virtualAction = ConvertToPlayerSpecific(virtualAction);
    }

    public JoystickControl(string virtualAction, string overloadedAction, bool isPositiveOverloadedAxis, int joystickNumber) {
        this.joystickNumber = joystickNumber;
        isButton = virtualAction.Contains("button");

        this.virtualAction = ConvertToPlayerSpecific(virtualAction);
        this.overloadedAction = ConvertToPlayerSpecific(overloadedAction);
        this.isPositiveOverloadedAxis = isPositiveOverloadedAxis;
    }

    public bool IsHeldDown() {
        bool isInput = false;

        if (isButton) {
            isInput = Input.GetKey(virtualAction);
        } else {
        }

        return isInput;
    }

    public bool IsDown() {
        bool isInput = false;

        if (isButton) {
            isInput = Input.GetKeyDown(virtualAction);
        } else {
            isInput = Input.GetAxis(virtualAction) != 0.0f;
        }

        return isInput;
    }

    public bool IsUp() {
        bool isInput = false;

        if (isButton) {
            isInput = Input.GetKeyUp(virtualAction);
        } else {
            isInput = Input.GetAxis(virtualAction) != 0.0f;
        }

        return isInput;
    }

    public float GetValue() {
        float isInput = 0.0f;

        if (isButton) {
            isInput = Convert.ToSingle(Input.GetKey(virtualAction));
        } else {
            isInput = Input.GetAxis(virtualAction);
        }

        return isInput;
    }

    public float GetValueRaw() {
        float isInput = 0.0f;

        if (isButton) {
            isInput = Convert.ToSingle(Input.GetKey(virtualAction));
        } else {
            isInput = Input.GetAxisRaw(virtualAction);
        }

        return isInput;
    }

    public override string ToString() {
        return virtualAction;
    }

    private string ConvertToPlayerSpecific(string buttonName) {
        return "joystick " + joystickNumber + " " + buttonName;
    }

    private bool GetOverloadedAction() {
        bool isInput = false;

        if(isPositiveOverloadedAxis) {
            isInput = Input.GetAxis(overloadedAction) > 0.0f;
        } else {
            isInput = Input.GetAxis(overloadedAction) < 0.0f;
        }

        return isInput;
    }
}
