using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GameMgr
{
    //config
    public static bool IsBgmReady = false;
    public static bool IsGameStart = false;
    public static bool IsGameOver = false;
    public static bool IsTimesUp = false;

    public static void ResetGame()
    {
        IsGameOver = false;
        IsGameStart = false;
        IsTimesUp = false;
    }

    //cache
    public static AudioMgr Audio;

    //debug mode 
    public static bool IsDebug = false;
}