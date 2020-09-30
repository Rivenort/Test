using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Singleton]
/// Defines ways to manipulate Settlers.
/// @author Rivenort
/// </summary>
public class M_SettlersManager : ITimeTriggerable, UT_IClearable
{
    private static M_SettlersManager s_instance = null;
    private static readonly object s_lock = new object();

    private List<ISettler> m_settlers;

    private DAT_SettlerNames m_namesDatabase = null;
    private DAT_SettlerPortraits m_portraitsDatabase = null;

    private M_SettlersManager() 
    {
        m_settlers = new List<ISettler>();
    }

    public static M_SettlersManager Instance
    {
        get
        {
            lock (s_lock)
            {
                if (s_instance == null)
                    s_instance = new M_SettlersManager();
                return s_instance;
            }
        }
    }

    public void Clear()
    {
        m_namesDatabase = null;
        m_portraitsDatabase = null;
        m_settlers = new List<ISettler>();
    }



    public void TriggerDay(int day)
    {
        foreach (ISettler s in m_settlers)
        {
            s.TriggerDay(day);
            
        }
    }

    public void TriggerMonth(int month)
    {
        foreach (ISettler settler in m_settlers)
        {
            settler.TriggerMonth(month);
        }
    }

    public void TriggerYear(int year)
    {

    }

    public static void STriggerDay(int day)
    {
        if (s_instance != null)
            s_instance.TriggerDay(day);
    }

    public static void STriggerMonth(int month)
    {
        if (s_instance != null)
            s_instance.TriggerMonth(month);
    }

    public void SetNamesData(DAT_SettlerNames names)
    {
        if (names == null)
            Debug.Log(typeof(DAT_SettlerNames).Name + " cannot be null!");
        if (m_namesDatabase != null)
            Debug.LogWarning("Names database was already initialized!");
        m_namesDatabase = names;
    }

    public void SetPortraitsData(DAT_SettlerPortraits portraits)
    {
        if (portraits == null)
            Debug.Log(typeof(DAT_SettlerPortraits).Name + " cannot be null!");
        if (m_portraitsDatabase != null)
            Debug.LogWarning("Names database was already initialized!");
        m_portraitsDatabase = portraits;
    }

    public string GetRandName()
    {
        if (m_namesDatabase.names != null)
            return m_namesDatabase.names[UnityEngine.Random.Range(0, m_namesDatabase.names.Length)];
        return "Bob";
    }

    public int GetRandPortrait()
    {
        if (m_portraitsDatabase != null)
            return UnityEngine.Random.Range(0, m_portraitsDatabase.portraits.Length);
        return 0;
    }

    public static string SGetRandName()
    {
        if (s_instance != null)
        {
            return s_instance.GetRandName();
        }
        return string.Empty;
    }

    public static int SGetRandPortrait()
    {
        if (s_instance != null)
        {
            return s_instance.GetRandPortrait();
        }
        return 0;
    }

    /// <summary> Creates a new settler </summary>
    /// <returns>Guid of a new settler</returns>
    public Guid CreateSettler(Guid houseId)
    {
        Settler settler = new Settler(houseId);
        settler.SetName(GetRandName());
        settler.SetPortrait(GetRandPortrait());

        List<FavoriteBuilding> favoriteBuildings = new List<FavoriteBuilding>();
        List<FavoriteWare> favoriteWares = new List<FavoriteWare>();
        int FAVORITE_BUILDINGS = 3;
        int FAVORITE_WARES = 3;

        M_BuildingManager.SEnableUniqueRandomizing(FAVORITE_BUILDINGS);
        for (int i = 0; i < FAVORITE_BUILDINGS; i++)
        {
            FavoriteBuilding temp = new FavoriteBuilding();
            temp.maxMood = 20;
            temp.buildingTemplId = M_BuildingManager.SGetRandProductionBuildingTempl();
            temp.active = false;
            favoriteBuildings.Add(temp);
        }


        M_WaresManager.SEnableUniqueWareRandomizing(FAVORITE_WARES);
        for (int i = 0; i < FAVORITE_WARES; i++)
        {
            FavoriteWare temp = new FavoriteWare();
            temp.mood = UnityEngine.Random.Range(10, 21); ;
            temp.ware = M_WaresManager.SGetRandWare();
            temp.active = false;
            favoriteWares.Add(temp);
        }

        settler.SetFavoriteBuildings(favoriteBuildings);
        settler.SetFavortiteWares(favoriteWares);


        m_settlers.Add(settler);
        Debug.Log("CreateSettler id: " + settler.GetId());
        return settler.GetId();
    }


    /// <summary> Creates a new settler </summary>
    /// <returns>Guid of a new settler</returns>
    public static Guid SCreateSettler(Guid houseId)
    {
        if (s_instance == null) return Guid.Empty;
        return s_instance.CreateSettler(houseId);
        
    }

    public float GetUpkeep()
    {
        float res = 0;
        foreach (Settler s in m_settlers)
        {
            res += s.GetUpkeep();
        }
        return res;
    }

    public static int SGetSettlersCount()
    {
        if (s_instance == null) return 0;
        return s_instance.m_settlers.Count;
    }

    public static float SGetUpkeep()
    {
        if (s_instance == null) return 0;
        return s_instance.GetUpkeep();
    }

    public static List<ISettler> SGetSettlers()
    {
        if (s_instance == null) return null;
        return s_instance.m_settlers;
    }

    /// <summary> Expecting settlers from deserialization </summary>
    public void AddSettlers(List<ISettler> settlers)
    {
        foreach (Settler settler in settlers)
        {
            m_settlers.Add(settler);
        }
    }

    /// <summary> Expecting settlers from deserialization </summary>
    public static void SAddSettlers(List<ISettler> settlers)
    {
        if (s_instance == null) return;
        s_instance.AddSettlers(settlers);
    }

    public Sprite GetPortrait(int id)
    {
        return m_portraitsDatabase.portraits[id % m_portraitsDatabase.portraits.Length]; 
    }

    public static Sprite SGetPortrait(int id)
    {
        if (s_instance != null)
        {
            return s_instance.GetPortrait(id);
        }
        return null;
    }

    public int GetPortraitId(Sprite sprite)
    {
        for (int i = 0; i < m_portraitsDatabase.portraits.Length; i++)
        {
            Sprite temp = m_portraitsDatabase.portraits[i];
            if (temp == sprite) return i;
        }
        return 0;
    }

    public static int SGetProtraitId(Sprite sprite)
    {
        if (s_instance != null)
        {
            s_instance.GetPortraitId(sprite);
        }
        return 0;
    }

    /// <summary>
    /// Sets workplace into given Settler. Note that it won't handle any
    /// Building update.
    /// </summary>
    /// <returns> Id of previous workplace. </returns>
    public Guid AssignWorkplace(Guid settlerId, Guid buildingId)
    {
        ISettler settler = GetSettler(settlerId);
        if (settler == null)
        {
            Debug.LogWarning("Given settler's GUID is invalid.");
            return Guid.Empty;
        }
        return settler.SetWorkplace(buildingId);
    }

    /// <summary>
    /// Appropriate way to Remove workplace from Settler.
    /// Note that it won't handle any Building Update.
    /// </summary>
    /// <returns>Id of removed workplace</returns>
    public Guid RemoveWorkplace(Guid settlerId)
    {
        ISettler settler = GetSettler(settlerId);
        if (settler == null)
        {
            Debug.LogWarning("Given settler's GUID is invalid.");
            return Guid.Empty;
        }
        return settler.RemoveWorkplace();
    }
 
    /// <summary>
    /// Assings House into given Settler. Note that it won't handle any
    /// on-Building operaions like updates.
    /// </summary>
    /// <returns> Previous Settler's house </returns>
    public Guid AssignHouse(Guid settlerId, Guid buildingId)
    {
        ISettler settler = GetSettler(settlerId);
        if (settler == null)
        {
            Debug.LogWarning("Given settler's GUID is invalid.");
            return Guid.Empty;
        }
        return settler.SetHouse(buildingId);
    }


    /// <summary>
    /// Sets workplace into given Settler. Note that it won't handle any
    /// Building update.
    /// </summary>
    /// <returns> Id of previous workplace. </returns>
    public static Guid SAssignWorkplace(Guid settlerId, Guid buildingId)
    {
        if (s_instance == null) return Guid.Empty;
        return s_instance.AssignWorkplace(settlerId, buildingId);
    }

    public static Guid SRemoveWorkplace(Guid settlerId)
    {
        if (s_instance == null) return Guid.Empty;
        return s_instance.RemoveWorkplace(settlerId);
    }

    public ISettler GetSettler(Guid id)
    {
        foreach (ISettler settler in m_settlers)
        {
            if (settler.GetId() == id)
                return settler;
        }
        return null;
     }

    public static ISettler SGetSettler(Guid id)
    {
        if (s_instance == null) return null;
        return s_instance.GetSettler(id);
    }

    public List<ISettler> GetSettlers(List<Guid> ids)
    {
        List<ISettler> res = new List<ISettler>();
        foreach (ISettler settler in m_settlers)
        {
            foreach (Guid id in ids)
            {
                if (id == settler.GetId())
                {
                    res.Add(settler);
                    break;
                }
            }
        }
        return res;
    }

    public static List<ISettler> SGetSettlers(List<Guid> ids)
    {
        if (s_instance == null) return null;
        return s_instance.GetSettlers(ids);
    }

    public Sprite GetPortraitOfSettler(Guid settlerId)
    {
        return GetPortrait(GetSettler(settlerId).GetPortraitId());
    }

    public static Sprite SGetPortraitOfSettler(Guid settlerId)
    {
        if (s_instance == null) return null;
        return s_instance.GetPortraitOfSettler(settlerId);
    }

    public string GetNameOfSettler(Guid settlerId)
    {
        return GetSettler(settlerId).GetName();
    }

    public static string SGetNameOfSettler(Guid settlerId)
    {
        if (s_instance == null) return "None";
        return s_instance.GetNameOfSettler(settlerId);
    }

    public int GetMoodFavBuilding(Guid settlerId)
    {
        return GetSettler(settlerId).GetMoodBuilding();
    }

    public static int SGetMoodFavBuilding(Guid settlerId)
    {
        if (s_instance == null) return 0;
        return s_instance.GetMoodFavBuilding(settlerId);
    }

    public int GetMoodFavWares(Guid settlerId)
    {
        return GetSettler(settlerId).GetMoodWares();
    }

    public static int SGetMoodFavWares(Guid settlerId)
    {
        if (s_instance == null) return 0;
        return s_instance.GetMoodFavWares(settlerId);
    }
}
