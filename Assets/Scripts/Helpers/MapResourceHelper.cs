using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class MapResourceHelper : MonoBehaviour
{
    public static IList<MapGroup> Data;


    private void Awake()
    {
        if (Data == null)
        {
            Data = new List<MapGroup>();
        }

        Init();
    }

    void Init()
    {
        for (int i = 0; i < 100; i++)
        {
            
            Sprite LD = Resources.Load<Sprite>(@$"Sprites\Maps\LD{i + 1}");
            Sprite RD = Resources.Load<Sprite>(@$"Sprites\Maps\RD{i + 1}");
            Sprite LN = Resources.Load<Sprite>(@$"Sprites\Maps\LN{i + 1}");
            Sprite RN = Resources.Load<Sprite>(@$"Sprites\Maps\RN{i + 1}");
            if (LD == null)
            {
                continue;
            }

            MapGroup group = new MapGroup(new EachDay(LD, RD), new EachDay(LN, RN));
            Data.Add(group);
        }


        Debug.Log(Data.Count);
    }
    void Start()
    {
       


    }

}