using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmHandler : MonoBehaviour
{
    void Awake()
    {
        if (!GameMgr.IsBgmReady)
        {
            GameMgr.IsBgmReady = true;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}