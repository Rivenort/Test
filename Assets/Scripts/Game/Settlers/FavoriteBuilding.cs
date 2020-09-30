using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FavoriteBuilding
{
    public static int DEFAULT_DAYS_PER_INCREASE = 1;

    public int buildingTemplId;
    public int maxMood;
    public int currentMood;
    public int daysPerMoodIncrease;
    public int currentDays;
    public bool active;

    public FavoriteBuilding(int buildingTemplId,
                            int maxMood, bool active)
    {
        this.buildingTemplId = buildingTemplId;
        this.maxMood = maxMood;
        this.active = active;
        currentMood = 0;
        daysPerMoodIncrease = DEFAULT_DAYS_PER_INCREASE;
        currentDays = 0;
    }

    public FavoriteBuilding() {
        currentMood = 0;
        daysPerMoodIncrease = DEFAULT_DAYS_PER_INCREASE;
        currentDays = 0;
    }
}
