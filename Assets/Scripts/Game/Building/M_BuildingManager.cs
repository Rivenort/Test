using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [Singleton]
/// Defines opreations preformed on buildings.
/// @author Rivenort
/// </summary>
public sealed class M_BuildingManager : ITimeTriggerable, UT_IClearable
{
    private static M_BuildingManager s_instance = null;
    private static readonly object s_lock = new object();



    private List<IBuilding> m_buildings = null;
    //
    private DAT_BuildingTemplates m_buildingTemplates;
    private UT_BuildingMechanics m_mechanics;
    private UT_BuildSystem m_buildSystem;

    // ------------------
    // For randomizing utility
    private static int s_uniqueRandomizingTimes;
    private static List<int> s_randomized = new List<int>();


    private M_BuildingManager()
    {
        m_buildings = new List<IBuilding>();
        m_buildSystem = new UT_BuildSystem(OnBuildingPlacedAction);
    }


    public static M_BuildingManager Instance
    {
        get
        {
            lock(s_lock)
            {
                if (s_instance == null)
                    s_instance = new M_BuildingManager();
                return s_instance;
            }
        }
    }

    public void Clear()
    {
        m_buildings = new List<IBuilding>();
        m_buildingTemplates = null;
    }


    public void Update()
    {
    }

    public void FixedUpdate()
    {
        m_buildSystem.FixedUpdate();
    }




    public void AddBuildings(List<IBuilding> buildings)
    {
        foreach (IBuilding building in buildings)
        {
            m_buildings.Add(building);
        }
    }

    public static void SAddBuildings(List<IBuilding> buildings)
    {
        if (s_instance != null)
        {
            s_instance.AddBuildings(buildings);
        }
    }

    /// <summary> Starts building process.
    /// Building is supposed to be placed.</summary>
    private void StartBuilding(DAT_Building buildingData)
    {
        m_buildSystem.StartBuilding(buildingData);
    }

    public static void SStartBuilding(DAT_Building buildingData)
    {
        if (s_instance != null)
            s_instance.StartBuilding(buildingData);
    }

   

    public static void SQuitBuilding()
    {
        if (s_instance != null)
        {
            s_instance.m_buildSystem.QuitBuilding();
        }
    }

    public static bool IsBuildingStarted()
    {
        if (s_instance == null)
            return false;
        return s_instance.m_buildSystem.IsBuildingStarted();
    }




    private void OnBuildingDragStart()
    {
        M_WorldUIManager.ShowSmallBuildButton(false);
    }

    private void OnBuildingDragStop()
    {/*
        Vector3 pos = m_tempBuildingObj.transform.position;
        pos.z += m_buttonYOffset;
        M_WorldUIManager.SetPositionSmallBuildButton(pos);
        M_WorldUIManager.ShowSmallBuildButton(true); */
    }

    public void TriggerDay(int day)
    {
        foreach (IBuilding building in m_buildings)
        {
            building.TriggerDay(day);
        }
        m_mechanics.UpdatePerDay();
    }

    public void TriggerMonth(int month)
    {

    }

    public void TriggerYear(int year)
    {

    }

    public static void STriggerDay(int day)
    {
        if (s_instance != null)
        {
            s_instance.TriggerDay(day);
        }
    }

    public static void STriggerMonth(int month)
    {
        if (s_instance != null)
        {
            s_instance.TriggerMonth(month);
        }
    }

    public static void STriggerYear(int year)
    {
        if (s_instance != null)
        {
            s_instance.TriggerYear(year);
        }
    }

    public int GetFoodPerDay()
    {
        int res = 0;
        foreach (IBuilding building in m_buildings)
            if (building is IBuildingProduction)
                res += (building as IBuildingProduction).GetProducedFood();
        return res;
    }

    public static int SGetFoodPerDay()
    {
        if (s_instance != null)
            return s_instance.GetFoodPerDay();
        return -1;
    }

    public int GetResearchPerDay()
    {
        int res = 0;
        foreach (IBuilding building in m_buildings)
        {
            if (building is IBuildingProduction)
                res += (building as IBuildingProduction).GetProducedResearch();
        }
        return res;
    }

    public static int SGetResearchPerDay()
    {
        if (s_instance != null)
            return s_instance.GetResearchPerDay();
        return -1;
    }

    public static int SGetBuildingsCount(int id)
    {
        if (s_instance != null)
        {
            if (s_instance.m_mechanics != null)
                return s_instance.m_mechanics.GetCount(id);
        }
        return 0;
    }

    public static DAT_BuildingTemplates SGetBuildingsTemplates()
    {
        if (s_instance != null)
        {
            return s_instance.m_buildingTemplates;
        }
        return null;
    }

    public void SetBuildingTemplates(DAT_BuildingTemplates data)
    {
        m_buildingTemplates = data;
        m_mechanics = new UT_BuildingMechanics(data.GetBuildings());
    }


    public int GetRandBuildingId()
    {
        int val = UnityEngine.Random.Range(0, m_buildingTemplates.production.Count);
        return m_buildingTemplates.production[val].id;
    }

    public static int SGetRandBuildingId()
    {
        if (s_instance != null)
        {
            return s_instance.GetRandBuildingId();
        }
        return -1;
    }

    public int GetRandBuildingTempl()
    {
        List<DAT_Building> availableBuildings = null;
        if (m_mechanics != null)
            availableBuildings = m_mechanics.GetAvailableBuildings();
        else
            availableBuildings = m_buildingTemplates.GetBuildings();

        int val = 0;
        if (s_randomized.Count < s_uniqueRandomizingTimes)
        {
            val = UnityEngine.Random.Range(0, availableBuildings.Count);
            while (s_randomized.Contains(val))
            {
                val++;
                if (val >= availableBuildings.Count)
                    val = 0;
            }
        } else
        {
            val = UnityEngine.Random.Range(0, availableBuildings.Count);
        }
        return availableBuildings[val].id;
    }

    public int GetRandProductionBuildingTempl()
    {
        List<DAT_Building> availableBuildings = null;
        if (m_mechanics != null)
            availableBuildings = m_mechanics.GetAvailableBuildings();
        else
            availableBuildings = m_buildingTemplates.GetBuildings();

        int val = 0;
        if (s_randomized.Count < s_uniqueRandomizingTimes)
        {
            val = UnityEngine.Random.Range(0, availableBuildings.Count);
            while (s_randomized.Contains(val) || !(availableBuildings[val] is DAT_BuildingProd))
            {
                val++;
                if (val >= availableBuildings.Count)
                    val = 0;
            }
            s_randomized.Add(val);
        }
        else
        {
            val = UnityEngine.Random.Range(0, availableBuildings.Count);
            while (!(availableBuildings[val] is DAT_BuildingProd))
            {
                val++;
                if (val >= availableBuildings.Count)
                    val = 0;
            }
        }
        return availableBuildings[val].id;
    }

    public static int SGetRandBuildingTempl()
    {
        if (s_instance != null)
        {
            return s_instance.GetRandBuildingTempl();
        }
        return 0;
    }

    public static int SGetRandProductionBuildingTempl()
    {
        if (s_instance != null)
        {
            return s_instance.GetRandProductionBuildingTempl();
        }
        return 0;
    }


    public static void SEnableUniqueRandomizing(int times)
    {
        s_uniqueRandomizingTimes = times;
        s_randomized.Clear();
    }

    public static void SDisableUniqueRandomizing()
    {
        s_uniqueRandomizingTimes = 0;
        s_randomized.Clear();
    }

    public DAT_Building GetDatBuilding(int id)
    {
        List<DAT_Building> buildings = m_buildingTemplates.GetBuildings();
        if (id < buildings.Count && buildings[id].id == id)
            return buildings[id];
        else
        {
            foreach (DAT_Building building in buildings)
            {
                if (building.id == id)
                    return building;
            }
        }
        return null;
    }

    public static DAT_Building SGetDatBuilding(int id)
    {
        if (s_instance != null)
        {
            return s_instance.GetDatBuilding(id);
        }
        return null;
    }

    /*
    public List<DAT_BuildingTemplate> ComputeAvailableBuildings()
    {
        ResetOverallMood();
        M_SettlersManager.SApplyOverallMoodToDatBuildings();
        ComputeBuildingsToUnlock();
        m_availableBuildings.Clear();
        foreach (DAT_BuildingTemplate building in m_buildingTemplates.buildings)
        {
            if (building.availableToBuild >= building.dependencies.Count)
            {
                m_availableBuildings.Add(building);
            }
        }
        return m_availableBuildings;
    } */

    /*
    public static List<DAT_BuildingTemplate> SComputeAvailableBuildings()
    {
        if (s_instance != null)
        {
            return s_instance.ComputeAvailableBuildings();
        }
        return new List<DAT_BuildingTemplate>();
    } */


    public IBuilding GetBuilding(Guid id)
    {
        foreach (IBuilding building in m_buildings)
        {
            if (building.GetId() == id)
                return building;
        }
        return null;
    }

    public static IBuilding SGetBuilding(Guid id)
    {
        if (s_instance != null)
        {
            return s_instance.GetBuilding(id);
        }
        return null;
    }

    public int GetTemplIdFromBuilding(Guid buildingId)
    {
        foreach (IBuilding building in m_buildings)
        {
            if (building.GetId() == buildingId)
                return building.GetData().id;
        }
        return -1;
    }

    public static int SGetTemplIdFromBuilding(Guid buildingId)
    {
        if (s_instance == null) return -1;
        return s_instance.GetTemplIdFromBuilding(buildingId);
    }

    public List<IBuilding> GetBuildings()
    {
        return m_buildings;
    }

    public static List<IBuilding> SGetBuildings()
    {
        if (s_instance == null) return null;
        return s_instance.GetBuildings();
    }

    public static List<DAT_Building> SGetAvailableBuildings()
    {
        if (s_instance == null) return null;
        if (s_instance.m_mechanics == null) return null;
        return s_instance.m_mechanics.GetAvailableBuildings();
    }

    /// <summary>
    /// Assigns a settler to a given building.
    /// </summary>
    public Guid AssignSettler(Guid buildingId, Guid settlerId)
    {
        IBuilding building = GetBuilding(buildingId);
        if (building is IBuildingProduction)
            return (building as IBuildingProduction).SetWorker(settlerId);
        else if (building is IBuildingHouse)
            (building as IBuildingHouse).AssignSettler(settlerId);
        return Guid.Empty;
    }

    /// <summary>
    /// Removes a settler from a building
    /// </summary>
    /// <returns>id of removed settler</returns>
    public Guid RemoveSettler(Guid buildingId)
    {
        IBuilding building = GetBuilding(buildingId);
        if (building is IBuildingProduction)
            return (building as IBuildingProduction).SetWorker(Guid.Empty);
        return Guid.Empty;
    }

    public static void SAssignSettler(Guid buildingId, Guid settlerId)
    {
        if (s_instance == null) return;
        s_instance.AssignSettler(buildingId, settlerId);
    }

    public static Guid SRemoveSettler(Guid buildingId)
    {
        if (s_instance == null) return Guid.Empty;
        return s_instance.RemoveSettler(buildingId);
    }

    public void PauseBuildings(bool val)
    {
        foreach (IBuilding building in m_buildings)
        {
            building.Pause(val);
        }
    }

    /// <summary></summary>
    private void OnBuildingPlacedAction(IBuilding building)
    {
        m_buildings.Add(building);
    }

    public static List<DAT_Ware.Type> SGetAvailableWares()
    {
        if (s_instance == null) return null;
        return s_instance.m_mechanics.GetAvailableWares();
    }

    public static UT_Pair<int, int> SGetBuildingCost(int id)
    {
        if (s_instance == null) return new UT_Pair<int, int>();
        return s_instance.m_mechanics.GetBuildingCost(id);
    }
}
