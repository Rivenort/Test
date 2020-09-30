using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a Settler.
/// @author Rivenort
/// </summary>
[System.Serializable]
public class Settler : ISettler
{

    public static int UPKEEP_ON_INIT = 1;

    protected Guid   m_id;
    protected string m_name;
    protected int    m_upkeep;
    protected int    m_portraitId;
    protected int    m_mood = 0;
    protected int    m_moodBuildings = 0;
    protected int    m_moodWares = 0;
    protected Guid   m_house;
    protected Guid   m_workplace;

    protected List<FavoriteBuilding> m_favoriteBuildings;
    protected List<FavoriteWare>     m_favoriteWares;


    public Settler(Guid houseId)
    {
        m_id = System.Guid.NewGuid();
        m_name = "";
        m_house = houseId;
        m_workplace = Guid.Empty;
        m_favoriteBuildings = new List<FavoriteBuilding>();
        m_favoriteWares = new List<FavoriteWare>();
        m_upkeep = UPKEEP_ON_INIT;

    }

    public Guid SetWorkplace(Guid newWorkplace)
    {
        Guid temp = m_workplace;
        m_workplace = newWorkplace;
        CheckFavoriteBuildings();
        return temp;
    }

    public Guid RemoveWorkplace()
    {
        Guid temp = m_workplace;
        m_workplace = Guid.Empty;
        CheckFavoriteBuildings();
        return temp;
    }

    
    private void CheckFavoriteBuildings()
    {
        if (m_favoriteBuildings == null) return;

        int templId = 0;
        if (m_workplace != Guid.Empty)
            templId = M_BuildingManager.SGetTemplIdFromBuilding(m_workplace);

        for (int i = 0; i < m_favoriteBuildings.Count; i++)
        {
            FavoriteBuilding fav = m_favoriteBuildings[i];


            if (!fav.active && fav.buildingTemplId == templId)
            {
                fav.active = true;
                fav.currentDays = 0;
                fav.currentMood = 0;
            } else if (fav.active && fav.buildingTemplId != templId)
            {
                fav.active = false;
                fav.currentMood = 0;
            }
        }
        
    } 

    public void UpdateFavoriteWares()
    {
        for (int i = 0; i < m_favoriteWares.Count; i++)
        {
            FavoriteWare fav = m_favoriteWares[i];
            bool res = M_WaresManager.SIsWareProduced(fav.ware);

            if (res && !fav.active)
            {
                m_moodWares += fav.mood;
                fav.active = true;
            } else if (!res && fav.active)
            {
                m_moodWares -= fav.mood;
                fav.active = false;
            }
        }

    }

    public Guid GetWorkplace()
    {
        return m_workplace;
    }

    public Guid GetHouse()
    {
        return m_house;
    }

    public int GetPortraitId()
    {
        return m_portraitId;
    }

    public void SetPortrait(int id)
    {
        m_portraitId = id;
    }

    public string GetName()
    {
        return m_name;
    }

    public void SetName(string name)
    {
        this.m_name = name;
    }

    public void SetMood(int mood)
    {
        this.m_mood = mood;
    }

    public int GetMood()
    {
        return m_mood;
    }

    public void SetUpkeep(int upkeep)
    {
        this.m_upkeep = upkeep;
    }

    public void TriggerDay(int day)
    {
        int mood = 0;
        foreach (FavoriteBuilding fav in m_favoriteBuildings)
        {
            if (fav.active)
            {
                fav.currentDays++;
                if (fav.currentDays % fav.daysPerMoodIncrease == 0 && fav.currentMood < fav.maxMood)
                    fav.currentMood++;
            }
            mood += fav.currentMood;
        }

        m_moodBuildings = mood;

        UpdateFavoriteWares();

        m_mood = m_moodBuildings + m_moodWares;
    }

    public void TriggerMonth(int month)
    {

    }

    public void TriggerYear(int year)
    {

    }

    public List<FavoriteBuilding> GetFavoriteBuildings()
    {
        return m_favoriteBuildings;
    }

    public List<FavoriteWare> GetFavoriteWares()
    {
        return m_favoriteWares;
    }

    public void SetFavortiteWares(List<FavoriteWare> favs)
    {
        m_favoriteWares = favs;
    }

    public void SetFavoriteBuildings(List<FavoriteBuilding> favs)
    {
        m_favoriteBuildings = favs;
    }

    public Guid GetId()
    {
        return m_id;
    }

    public int GetUpkeep()
    {
        return m_upkeep;
    }

    public Guid SetHouse(Guid newHouse)
    {
        Guid temp = m_house;
        m_house = newHouse;
        return temp;
    }

    public int GetMoodBuilding()
    {
        return m_moodBuildings;
    }

    public int GetMoodWares()
    {
        return m_moodWares;
    }
}
