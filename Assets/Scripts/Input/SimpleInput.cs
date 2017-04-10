using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SimpleInput {
    private static List<ControlScheme> storedPrefSchemes = new List<ControlScheme> {
        new ControlScheme("Keyboard1"),
        new ControlScheme("Keyboard2"),
        new ControlScheme("Keyboard2"),
        new ControlScheme("Xbox", 1)
    };

    private static List<ControlScheme> playerSchemes = new List<ControlScheme>(storedPrefSchemes);

    public static int NumberOfPlayers { get { return playerSchemes.Count; } }

    /// <summary>
    /// Returns true while the virtual button identified by buttonName for any player is held down.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButton(string buttonName) {
        bool isPressed = false;

        for (int i = 0; !isPressed && i < playerSchemes.Count; i++) {
            isPressed = playerSchemes[i].GetButton(buttonName);
        }

        return isPressed;
    }

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
    /// Returns true during the frame any user pressed down the virtual button identified by buttonName.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButtonDown(string buttonName) {
        bool isPressed = false;

        for (int i = 0; !isPressed && i < playerSchemes.Count; i++) {
            isPressed = playerSchemes[i].GetButtonDown(buttonName);
        }

        return isPressed;
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
    /// Returns true the first frame any user releases the virtual button identified by buttonName.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButtonUp(string buttonName) {
        bool isPressed = false;

        for (int i = 0; !isPressed && i < playerSchemes.Count; i++) {
            isPressed = playerSchemes[i].GetButtonUp(buttonName);
        }

        return isPressed;
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
    /// Returns the value of any virtual axis identified by axisName.
    /// </summary>
    /// <param name="axisName"></param>
    /// <returns></returns>
    public static float GetAxis(string axisName) {
        float isPressed = 0.0f;

        for (int i = 0; isPressed == 0.0f && i < playerSchemes.Count; i++) {
            isPressed = playerSchemes[i].GetAxis(axisName);
        }

        return isPressed;
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
    /// Returns the value of any virtual axis identified by axisName with no smoothing filtering applied.
    /// </summary>
    /// <param name="axisName"></param>
    /// <returns></returns>
    public static float GetAxisRaw(string axisName) {
        float isPressed = 0.0f;

        for (int i = 0; isPressed == 0.0f && i < playerSchemes.Count; i++) {
            isPressed = playerSchemes[i].GetAxisRaw(axisName);
        }

        return isPressed;
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

    /// <summary>
    /// Returns true if any player is currently pressing any button.
    /// </summary>
    /// <returns></returns>
    public static bool GetAnyButton() {
        bool isPressed = false;

        for (int i = 0; !isPressed && i < playerSchemes.Count; i++) {
            isPressed = playerSchemes[i].GetAnyButton();
        }

        return isPressed;
    }

    /// <summary>
    /// Returns true if playerNumber is currently pressing any button.
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static bool GetAnyButton(int playerNumber) {
        return playerSchemes[playerNumber - 1].GetAnyButton();
    }

    /// <summary>
    /// Returns true during the frame any player pressed down any button.
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static bool GetAnyButtonDown() {
        bool isPressed = false;

        for (int i = 0; !isPressed && i < playerSchemes.Count; i++) {
            isPressed = playerSchemes[i].GetAnyButtonDown();
        }

        return isPressed;
    }

    /// <summary>
    /// Returns true during the frame playerNumber pressed down any button.
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static bool GetAnyButtonDown(int playerNumber) {
        return playerSchemes[playerNumber - 1].GetAnyButtonDown();
    }

    /// <summary>
    /// Returns the system control name associated to the playerNumber's actionName.
    /// </summary>
    /// <param name="actionName"></param>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static string GetControlName(string actionName, int playerNumber) {
        return playerSchemes[playerNumber - 1].GetBinding(actionName).ToString();
    }

    /// <summary>
    /// Clears the current mapping between players and their devices.
    /// </summary>
    public static void ClearCurrentPlayerDevices() {
        playerSchemes.Clear();
    }

    /// <summary>
    /// Binds <paramref name="deviceNumber"/>'s device to the correct playerNumber.
    /// </summary>
    /// <param name="deviceNumber"></param>
    public static void MapPlayerToDevice(int deviceNumber) {
        playerSchemes.Add(storedPrefSchemes[deviceNumber - 1]);
    }

    /// <summary>
    /// Binds the player preferenced devices to their default player.
    /// </summary>
    public static void MapPlayersToDefaultPref() {
        if (!PlayerPrefs.HasKey("Player 1 Controls")) {
            PlayerPrefs.SetString("Player 1 Controls", "Keyboard1");
            PlayerPrefs.SetString("Player 2 Controls", "Xbox");
            PlayerPrefs.SetString("Player 3 Controls", "Xbox");
            PlayerPrefs.SetString("Player 4 Controls", "Xbox");
            PlayerPrefs.Save();
        }

        ClearCurrentPlayerDevices();

        LoadPrefsControlSchemes();

        playerSchemes = storedPrefSchemes.ToList();
    }

    private static void LoadPrefsControlSchemes() {
        int currentJoystick = 1;

        for (int i = 1; i <= NumberOfPlayers; i++) {
            string prefValue = PlayerPrefs.GetString("Player " + i + " Controls");

            if (prefValue.Contains("Keyboard")) {
                storedPrefSchemes.Add(new ControlScheme(prefValue));
            } else {
                storedPrefSchemes.Add(new ControlScheme(prefValue, currentJoystick));
                currentJoystick++;
            }
        }
    }
}
