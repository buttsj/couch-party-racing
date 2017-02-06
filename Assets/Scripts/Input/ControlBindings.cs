using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBindings {
    private static IControl NULL_CONTROL = new KeyboardControl(KeyCode.None);

    // Dictionary of all actions, initially all mapped to null
    private Dictionary<string, IControl> bindings = new Dictionary<string, IControl> {
        { "Horizontal",     NULL_CONTROL },
        { "Vertical",       NULL_CONTROL },
        { "Accelerate",     NULL_CONTROL },
        { "Reverse",        NULL_CONTROL },
        { "Use PowerUp",    NULL_CONTROL },
        { "Reset Rotation", NULL_CONTROL },
        { "Pause",          NULL_CONTROL },
        { "Bump Kart",      NULL_CONTROL },
        { "Boost",          NULL_CONTROL }
    };

    private int joystickNumber;

    public ControlBindings() { }
    public ControlBindings(int joystickNumber) {
        this.joystickNumber = joystickNumber;
    }

    public Dictionary<string, IControl> Xbox {
        get {
            bindings["Horizontal"] = new JoystickControl(XBOX.LStickX, joystickNumber);
            bindings["Vertical"] = new JoystickControl(XBOX.LStickY, joystickNumber);
            bindings["Accelerate"] = new JoystickControl(XBOX.RTrigger, joystickNumber);
            bindings["Reverse"] = new JoystickControl(XBOX.LTrigger, joystickNumber);
            bindings["Use PowerUp"] = new JoystickControl(XBOX.B, joystickNumber);
            bindings["Reset Rotation"] = new JoystickControl(XBOX.Back, joystickNumber);
            bindings["Pause"] = new JoystickControl(XBOX.Start, joystickNumber);
            bindings["Bump Kart"] = new JoystickControl(XBOX.A, joystickNumber);
            bindings["Boost"] = new JoystickControl(XBOX.X, joystickNumber);

            return bindings;
        }
    }

    public Dictionary<string, IControl> Keyboard1 {
        get {
            bindings["Horizontal"] = new KeyboardControl(KeyCode.A, KeyCode.D);
            bindings["Vertical"] = new KeyboardControl(KeyCode.Z, KeyCode.Q);
            bindings["Accelerate"] = new KeyboardControl(KeyCode.W);
            bindings["Reverse"] = new KeyboardControl(KeyCode.S);
            bindings["Use PowerUp"] = new KeyboardControl(KeyCode.E);
            bindings["Reset Rotation"] = new KeyboardControl(KeyCode.B);
            bindings["Pause"] = new KeyboardControl(KeyCode.Return);
            bindings["Bump Kart"] = new KeyboardControl(KeyCode.Space);
            bindings["Boost"] = new KeyboardControl(KeyCode.LeftShift);

            return bindings;
        }
    }

    public Dictionary<string, IControl> Keyboard2 {
        get {
            bindings["Horizontal"] = new KeyboardControl(KeyCode.LeftArrow, KeyCode.RightArrow);
            bindings["Vertical"] = new KeyboardControl(KeyCode.PageDown, KeyCode.PageUp);
            bindings["Accelerate"] = new KeyboardControl(KeyCode.UpArrow);
            bindings["Reverse"] = new KeyboardControl(KeyCode.DownArrow);
            bindings["Use PowerUp"] = new KeyboardControl(KeyCode.Keypad1);
            bindings["Reset Rotation"] = new KeyboardControl(KeyCode.End);
            bindings["Pause"] = new KeyboardControl(KeyCode.Home);
            bindings["Bump Kart"] = new KeyboardControl(KeyCode.Keypad0);
            bindings["Boost"] = new KeyboardControl(KeyCode.Keypad2);

            return bindings;
        }
    }

    public static class XBOX {
        public const string A = "button 0";
        public const string B = "button 1";
        public const string X = "button 2";
        public const string Y = "button 3";
        public const string LBumper = "button 4";
        public const string RBumper = "button 5";
        public const string Back = "button 6";
        public const string Start = "button 7";
        public const string LStickClick = "button 8";
        public const string RStickClick = "button 9";
        public const string LStickX = "X Axis";
        public const string LStickY = "Y Axis";
        public const string LTrigger = "9th Axis";
        public const string RTrigger = "10th Axis";
        public const string Triggers = "3rd Axis";
        public const string RStickX = "4th Axis";
        public const string RStickY = "5th Axis";
        public const string DPadX = "6th Axis";
        public const string DPadY = "7th Axis";
    }
}
