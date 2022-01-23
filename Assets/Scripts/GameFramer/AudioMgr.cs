using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class AudioMgr : MonoBehaviour
{
    public AudioSource source;

    public AudioClip clickClip;

    public AudioClip outofscreenClip;

    public AudioClip changeColorClip;

    public AudioClip[] speedUpClips;
    
    public void PlayClick()
    {
        source.PlayOneShot(clickClip);
    }

    public void PlayOutOfScreen()
    {
        source.PlayOneShot(outofscreenClip);
    }

    public void PlayChangeColor()
    {
        source.PlayOneShot(changeColorClip);
    }

    public void PlaySpeedUp()
    {
        int random = Random.Range(0, speedUpClips.Length);
        source.PlayOneShot(speedUpClips[random]);
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
