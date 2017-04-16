using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CouchPartyManager  {

	
    public static bool IsCouchPartyMode { get; set; }

    public static int PlayerOneScore { get; set; }

    public static int PlayerTwoScore { get; set; }

    public static int PlayerThreeScore { get; set; }

    public static int PlayerFourScore { get; set; }

    public static bool IsLastRound { get; set; }

    public static void ResetScores() {
        PlayerOneScore = 0;
        PlayerTwoScore = 0;
        PlayerThreeScore = 0;
        PlayerFourScore = 0;
    }
}
