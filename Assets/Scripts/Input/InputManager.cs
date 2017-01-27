using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {

    /// <summary>
    /// Returns true while the virtual button identified by buttonName is held down.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButton(string buttonName) {
        return Input.GetButton(buttonName);
    }

    /// <summary>
    /// Returns true while the virtual button identified by playerNumber's buttonName is held down.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static bool GetButton(string buttonName, int playerNumber) {
        return Input.GetButton(buttonName + GetPlayerExtension(playerNumber));
    }

    /// <summary>
    /// Returns true during the frame the user pressed down the virtual button identified by buttonName.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButtonDown(string buttonName) {
        return Input.GetButtonDown(buttonName);
    }

    /// <summary>
    /// Returns true during the frame the user pressed down the virtual button identified by playerNumber's buttonName.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static bool GetButtonDown(string buttonName, int playerNumber) {
        return Input.GetButtonDown(buttonName + GetPlayerExtension(playerNumber));
    }

    /// <summary>
    /// Returns true the first frame the user releases the virtual button identified by buttonName.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButtonUp(string buttonName) {
        return Input.GetButtonUp(buttonName);
    }

    /// <summary>
    /// Returns true the first frame the user releases the virtual button identified by playerNumber's buttonName.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static bool GetButtonUp(string buttonName, int playerNumber) {
        return Input.GetButtonUp(buttonName + GetPlayerExtension(playerNumber));
    }

    /// <summary>
    /// Returns the value of the virtual axis identified by axisName.
    /// </summary>
    /// <param name="axisName"></param>
    /// <returns></returns>
    public static float GetAxis(string axisName) {
        return Input.GetAxis(axisName);
    }

    /// <summary>
    /// Returns the value of the virtual axis identified by playerNumber's axisName.
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static float GetAxis(string axisName, int playerNumber) {
        return Input.GetAxis(axisName + GetPlayerExtension(playerNumber));
    }

    /// <summary>
    /// Returns the value of the virtual axis identified by axisName with no smoothing filtering applied.
    /// </summary>
    /// <param name="axisName"></param>
    /// <returns></returns>
    public static float GetAxisRaw(string axisName) {
        return Input.GetAxisRaw(axisName);
    }

    /// <summary>
    /// Returns the value of the virtual axis identified by playerNumber's axisName with no smoothing filtering applied.
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static float GetAxisRaw(string axisName, int playerNumber) {
        return Input.GetAxisRaw(axisName + GetPlayerExtension(playerNumber));
    }

    private static string GetPlayerExtension(int playerNumber) {
        return "(Player " + playerNumber + ")";
    }
}
