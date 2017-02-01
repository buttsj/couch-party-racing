using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : IControl {

    private KeyCode positiveKey;
    private KeyCode negativeKey;
    private string axis;

    public KeyboardControl(KeyCode key) {
        positiveKey = key;
        negativeKey = KeyCode.None;
        axis = "";
    }

    public KeyboardControl(string axisName) {
        positiveKey = KeyCode.None;
        negativeKey = KeyCode.None;
        axis = axisName;
    }

    public KeyboardControl(KeyCode negativeKey, KeyCode positiveKey) {
        this.negativeKey = negativeKey;
        this.positiveKey = positiveKey;
        axis = "";
    }


    public bool IsHeldDown() {
        return Input.GetKey(positiveKey) || Input.GetKey(negativeKey);
    }

    public bool IsDown() {
        return Input.GetKeyDown(positiveKey) || Input.GetKeyDown(negativeKey);
    }

    public bool IsUp() {
        return Input.GetKeyUp(positiveKey) || Input.GetKeyUp(negativeKey);
    }

    public float GetValue() {
        float value = 0.0f;

        if (axis.Length == 0) {
            value = Convert.ToSingle(Input.GetKey(positiveKey)) - Convert.ToSingle(Input.GetKey(negativeKey));
        } else {
            value = Input.GetAxis(axis);
        }

        return value;
    }

    public float GetValueRaw() {
        float value = 0.0f;

        if (axis.Length == 0) {
            value = Convert.ToSingle(Input.GetKey(positiveKey)) - Convert.ToSingle(Input.GetKey(negativeKey));
        } else {
            value = Input.GetAxisRaw(axis);
        }

        return value;
    }

    public override string ToString() {
        string value;

        if (axis.Length != 0) {
            value = axis;
        } else {
            value = negativeKey.ToString();
            
            if (negativeKey != KeyCode.None) {
                value += " , " + positiveKey.ToString();
            }
        }

        return value;
    }
}
