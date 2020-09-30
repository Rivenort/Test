using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Asset file. (Won't be changed by an application)
/// @author Rivenort
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "resData", menuName = M_GameHelper.EDITOR_MENU_ITEM + "ResourcesData" )]
public class DAT_Resources : ScriptableObject
{
    public int year;
    public int maxYears;
    public int days;
    public int daysPerMonth;
    public int monthsPerSeason = 3;
    public float secondsPerDay;
    public float m_timeElapsed = 0f;

    [Space(10)]
    public float food;
    public float maxFood;

    [Space(10)]
    public float researchPoints;
    public float maxResearchPoints;

}
