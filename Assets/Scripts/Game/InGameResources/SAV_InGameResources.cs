using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SAV_InGameResources
{
    public int year;
    public int days;
    public float timeElapsed;

    public float food;
    public float researchPoints;

    public SAV_InGameResources(DAT_Resources res)
    {
        year = res.year;
        days = res.days;
        timeElapsed = res.m_timeElapsed;
        food = res.food;
        researchPoints = res.researchPoints;
    }
}
