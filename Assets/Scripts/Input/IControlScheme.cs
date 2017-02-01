using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControlScheme {

    string Name { get; set; }

    void ResetBindingsToDefault();

    bool GetButton(string buttonName);

    bool GetButtonDown(string buttonName);

    bool GetButtonUp(string buttonName);

    float GetAxis(string axisName);

    float GetAxisRaw(string axisName);

    IControl GetBinding(string bindingName);

    List<IControl> GetAllInput();

    bool GetAnyButton();
}
