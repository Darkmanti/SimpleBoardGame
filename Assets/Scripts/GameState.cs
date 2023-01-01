using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static int numberOfPlayers = 1;

    public static void SetNumberOfPlayers(int number)
    {
        numberOfPlayers = number;
        Debug.Log($"players: {number}");
    }
}
