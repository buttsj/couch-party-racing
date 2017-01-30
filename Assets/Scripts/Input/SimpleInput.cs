using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SimpleInput {

    private static IControlScheme[] playerSchemes = new IControlScheme[11] { new KeyboardScheme(), new Xbox360Scheme(1), new Xbox360Scheme(2), new Xbox360Scheme(3), null, null, null, null, null, null , null};

    /// <summary>
    /// Returns true while the virtual button identified by playerNumber's buttonName is held down.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static bool GetButton(string buttonName, int playerNumber) {
        return playerSchemes[playerNumber - 1].GetButton(buttonName);
    }

    /// <summary>
    /// Returns true during the frame the user pressed down the virtual button identified by playerNumber's buttonName.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static bool GetButtonDown(string buttonName, int playerNumber) {
        return playerSchemes[playerNumber - 1].GetButtonDown(buttonName);
    }

    /// <summary>
    /// Returns true the first frame the user releases the virtual button identified by playerNumber's buttonName.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static bool GetButtonUp(string buttonName, int playerNumber) {
        return playerSchemes[playerNumber - 1].GetButtonUp(buttonName);
    }

    /// <summary>
    /// Returns the value of the virtual axis identified by playerNumber's axisName.
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static float GetAxis(string axisName, int playerNumber) {
        return playerSchemes[playerNumber - 1].GetAxis(axisName);
    }

    /// <summary>
    /// Returns the value of the virtual axis identified by playerNumber's axisName with no smoothing filtering applied.
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static float GetAxisRaw(string axisName, int playerNumber) {
        return playerSchemes[playerNumber - 1].GetAxisRaw(axisName);
    }

    public static bool ListenForPlayer(int playerNumber) {
        bool listened = false;

        if(Input.anyKey) {
            playerSchemes[playerNumber - 1] = new KeyboardScheme();
            listened = true;
        } else {
            for (int joystickNumber = 1; joystickNumber < 5; joystickNumber++) {
                for (int i = 0; i < 20; i++) {
                    if (Input.GetKeyDown("joystick " + joystickNumber + " button " + i)) {
                        if (playerSchemes[playerNumber - 1] == null) {
                            playerSchemes[playerNumber - 1] = new Xbox360Scheme(joystickNumber);
                            listened = true;
                            break;
                        }
                    }
                }
            }
        }

        if(listened) {
            Debug.Log("Listened " + playerNumber);
        }

        return listened;
    }

    public static bool GetAnyButton() {
        bool anyJoystick = false;

        for (int i = 0; i < 20; i++) {
            if (Input.GetKeyDown("joystick button " + i)) {
                anyJoystick = true;
                break;
            }
        }

        return anyJoystick || Input.anyKey;
    }
}
