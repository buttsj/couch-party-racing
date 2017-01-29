using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControl {

    float GetValue();

    float GetValueRaw();

    bool IsHeldDown();

    bool IsDown();

    bool IsUp();

    string ToString();
}
