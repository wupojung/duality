using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioMgr : MonoBehaviour
{
    public AudioSource source;

    public AudioClip clickClip;

    public void PlayClick()
    {
        source.PlayOneShot(clickClip);
    }

    private void Awake()
    {
        if (!GameMgr.Audio)
        {
            GameMgr.Audio = this;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
