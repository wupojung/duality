using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GameMgr
{
    //config
    public static bool IsBgmReady = false;
    public static bool IsGameStart = false;
    public static bool IsGameOver = false;

    //cache
    public static AudioMgr Audio;

    //debug mode 
    public static bool IsDebug = false;
}