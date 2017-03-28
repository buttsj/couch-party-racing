using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBindings {
    private static IControl NULL_CONTROL = new KeyboardControl(KeyCode.None);

    // Dictionary of all actions, initially all mapped to null
    private Dictionary<string, IControl> bindings = new Dictionary<string, IControl> {
        { "Horizontal",         NULL_CONTROL },
        { "Vertical",           NULL_CONTROL },
        { "Accelerate",         NULL_CONTROL },
        { "Reverse",            NULL_CONTROL },
        { "Use PowerUp",        NULL_CONTROL },
        { "Reset Rotation",     NULL_CONTROL },
        { "Pause",              NULL_CONTROL },
        { "Bump Kart",          NULL_CONTROL },
        { "Front Flip",          NULL_CONTROL },
        { "Back Flip",          NULL_CONTROL },
        { "Right Roll",          NULL_CONTROL },
        { "Left Roll",          NULL_CONTROL },
        { "Boost",              NULL_CONTROL },
        { "Cancel",             NULL_CONTROL },
        { "Rotate",             NULL_CONTROL },
        { "Next Track",         NULL_CONTROL },
        { "Previous Track",     NULL_CONTROL },
        { "Delete Track",       NULL_CONTROL }
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
            bindings["Accelerate"] = new JoystickControl(XBOX.RBumper, joystickNumber);
            bindings["Reverse"] = new JoystickControl(XBOX.LBumper, joystickNumber);
            bindings["Use PowerUp"] = new JoystickControl(XBOX.X, joystickNumber);
            bindings["Reset Rotation"] = new JoystickControl(XBOX.Back, joystickNumber);
            bindings["Pause"] = new JoystickControl(XBOX.Start, joystickNumber);
            bindings["Bump Kart"] = new JoystickControl(XBOX.A, joystickNumber);
            bindings["Front Flip"] = new JoystickControl(XBOX.DPadY, XBOX.DPadY, true, joystickNumber);
            bindings["Back Flip"] = new JoystickControl(XBOX.DPadY, XBOX.DPadY, false, joystickNumber);
            bindings["Right Roll"] = new JoystickControl(XBOX.DPadX, XBOX.DPadX, true, joystickNumber);
            bindings["Left Roll"] = new JoystickControl(XBOX.DPadX, XBOX.DPadX, false, joystickNumber);
            bindings["Boost"] = new JoystickControl(XBOX.B, joystickNumber);
            bindings["Cancel"] = new JoystickControl(XBOX.B, joystickNumber);

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
            bindings["Front Flip"] = new KeyboardControl(KeyCode.F);
            bindings["Back Flip"] = new KeyboardControl(KeyCode.G);
            bindings["Right Roll"] = new KeyboardControl(KeyCode.C);
            bindings["Left Roll"] = new KeyboardControl(KeyCode.V);
            bindings["Boost"] = new KeyboardControl(KeyCode.LeftShift);
            bindings["Cancel"] = new KeyboardControl(KeyCode.Escape);
            bindings["Rotate"] = new KeyboardControl(KeyCode.R);
            bindings["Next Track"] = new KeyboardControl(KeyCode.RightArrow);
            bindings["Previous Track"] = new KeyboardControl(KeyCode.LeftArrow);
            bindings["Delete Track"] = new KeyboardControl(KeyCode.Delete);

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
            bindings["Front Flip"] = new KeyboardControl(KeyCode.Keypad8);
            bindings["Back Flip"] = new KeyboardControl(KeyCode.Keypad5);
            bindings["Left Roll"] = new KeyboardControl(KeyCode.Keypad4);
            bindings["Right Roll"] = new KeyboardControl(KeyCode.Keypad6);
            bindings["Boost"] = new KeyboardControl(KeyCode.Keypad2);
            bindings["Cancel"] = new KeyboardControl(KeyCode.Backspace);

            return bindings;
        }
    }

    public Dictionary<string, IControl> Playstation4 {
        get {
            bindings["Horizontal"] = new JoystickControl(PS4.LStickX, joystickNumber);
            bindings["Vertical"] = new JoystickControl(PS4.LStickY, joystickNumber);
            bindings["Accelerate"] = new JoystickControl(PS4.R2Axis, joystickNumber);
            bindings["Reverse"] = new JoystickControl(PS4.L2Axis, joystickNumber);
            bindings["Use PowerUp"] = new JoystickControl(PS4.Square, joystickNumber);
            bindings["Reset Rotation"] = new JoystickControl(PS4.Share, joystickNumber);
            bindings["Pause"] = new JoystickControl(PS4.Playstation, joystickNumber);
            bindings["Bump Kart"] = new JoystickControl(PS4.X, joystickNumber);
            bindings["Boost"] = new JoystickControl(PS4.Circle, joystickNumber);
            bindings["Cancel"] = new JoystickControl(PS4.Circle, joystickNumber);

            return bindings;
        }
    }

    private static class XBOX {
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

    private static class PS4 {
        public const string Square = "button 0";
        public const string X = "button 1";
        public const string Circle = "button 2";
        public const string Triangle = "button 3";
        public const string L1 = "button 4";
        public const string R1 = "button 5";
        public const string L2 = "button 6";
        public const string R2 = "button 7";
        public const string Share = "button 8";
        public const string Options = "button 9";
        public const string L3 = "button 10";
        public const string R3 = "button 11";
        public const string Playstation = "button 12";
        public const string PadPress = "button 13";

        public const string LStickX = "X Axis";
        public const string LStickY = "Y Axis";
        public const string RStickX = "3rd Axis";
        public const string RStickY = "4th Axis";
        public const string L2Axis = "4th Axis";
        public const string R2Axis = "5th Axis";
        public const string DPadX = "7th Axis";
        public const string DPadY = "8th Axis";
    }
}
