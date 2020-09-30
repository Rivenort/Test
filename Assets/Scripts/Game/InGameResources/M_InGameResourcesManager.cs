using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Singleton
/// @author Rivenort
/// </summary>
public sealed class M_InGameResourcesManager : UT_IClearable
{
    private static M_InGameResourcesManager s_instance = null;
    private static readonly object s_lock = new object(); // for thread safety

    private DAT_Resources m_data;
    // References to gui objects;
    private TMP_Text m_foodText;
    private TMP_Text m_dayText;
    private TMP_Text m_researchText;

    M_InGameResourcesManager()
    {
    }

    public static M_InGameResourcesManager GetInstance(TMP_Text foodText, TMP_Text yearText, TMP_Text researchText)
    {
        lock (s_lock)
        {
            if (s_instance == null)
                s_instance = new M_InGameResourcesManager();
            s_instance.m_foodText = foodText;
            s_instance.m_dayText = yearText;
            s_instance.m_researchText = researchText;
            return s_instance;
        }
    }

    public void Clear()
    {
        m_data = null;
    }

    public void Update()
    {
        // game time handling
        if (m_data == null) return;
        m_data.m_timeElapsed += Time.deltaTime;
        if (m_data.m_timeElapsed > m_data.secondsPerDay)
        {
            m_data.days++;
            if (m_data.days % m_data.daysPerMonth == 0)
            {
                M_SettlersManager.STriggerMonth(m_data.days / m_data.daysPerMonth);
                M_BuildingManager.STriggerMonth(m_data.days / m_data.daysPerMonth);
            }
            if (m_data.days > m_data.daysPerMonth * m_data.monthsPerSeason * 4 
                && m_data.year < m_data.maxYears)
            {
                m_data.year++;
                m_data.days = 0;
                
                M_BuildingManager.STriggerYear(m_data.year);
            }
            M_SettlersManager.STriggerDay(m_data.days);
            M_BuildingManager.STriggerDay(m_data.days);

            m_dayText.text = m_data.days.ToString();
            m_data.m_timeElapsed -= m_data.secondsPerDay;
        }
    }

    public DAT_Resources ResourcesData
    {
        get => m_data;
        set
        {
            m_data = value;
            refreshGUI();
        }
    }


    /// <param name="food">Can be negative.</param>
    public void AddFood(float food)
    {
        if (m_data.food + food > -1)
        {
            m_data.food += food;
            m_foodText.text = ((int)(m_data.food)).ToString();
        }
    }

    /// <param name="points">Can be negative.</param>
    public void AddResearchPoints(float points)
    {
        if (m_data.researchPoints + points > -1)
        {
            m_data.researchPoints += points;
            m_researchText.text = ((int) m_data.researchPoints).ToString();
        }
    }

    public static void SAddFood(float food)
    {
        if (s_instance != null)
        {
            s_instance.AddFood(food);
        }
    }

    public static void SAddResearch(float points)
    {
        if (s_instance != null)
        {
            s_instance.AddResearchPoints(points);
        }
    }

    public static float GetFood()
    {
        if (s_instance != null)
            return s_instance.m_data.food;
        return -1;
    }

    public static float GetResearchPoints()
    {
        if (s_instance != null)
            return s_instance.m_data.researchPoints;
        return -1;
    }

    private void refreshGUI()
    {
        m_foodText.text = m_data.food.ToString();
        m_researchText.text = m_data.researchPoints.ToString();
    }

    public DAT_Resources GetData()
    {
        return m_data;
    }

    public static DAT_Resources SGetData()
    {
        if (s_instance != null)
        {
            return s_instance.GetData();
        }
        return null;
    }

    public void ApplySavedData(SAV_InGameResources save)
    {
        m_data.year = save.year;
        m_data.days = save.days;
        m_data.m_timeElapsed = save.timeElapsed;
        m_data.researchPoints = save.researchPoints;
        m_data.food = save.food;

        refreshGUI();
    }

    public static void SApplySavedData(SAV_InGameResources save)
    {
        if (s_instance != null)
        {
            s_instance.ApplySavedData(save);
        }
    }

}
