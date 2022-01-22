using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGroup
{
    public EachDay Day;
    public EachDay Night;
    public MapGroup(EachDay day,EachDay night)
    {
        this.Day = day;
        this.Night = night;
    }
}
