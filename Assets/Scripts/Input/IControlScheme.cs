using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControlScheme {

    string Name { get; set; }

    bool GetButton(string buttonName);

    bool GetButtonDown(string buttonName);

    bool GetButtonUp(string buttonName);

    float GetAxis(string axisName);

    float GetAxisRaw(string axisName);

    IControl GetBinding(string bindingName);

    void ResetBindingsToDefault();
}
