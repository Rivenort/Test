using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_WaresManager
{
    private static M_WaresManager s_instance = null;
    private static readonly object s_lock = new object();

    //--------------
    // For randomizing utility
    private static int s_uniqueRandomizingTimes = 0;
    private static List<int> s_randomized = new List<int>();


    public struct WareStats
    {
        public DAT_Ware.Type type;
        public List<Guid> buildingsProducing;
    }

    private WareStats[] m_wareStats;


    private M_WaresManager()
    {
        m_wareStats = new WareStats[System.Enum.GetValues(typeof(DAT_Ware.Type)).Length];

        foreach (DAT_Ware.Type ware in System.Enum.GetValues(typeof(DAT_Ware.Type)))
        {
            WareStats temp = new WareStats();
            temp.type = ware;
            temp.buildingsProducing = new List<Guid>();
            m_wareStats[(int)temp.type] = temp;
        }
    }

    public static M_WaresManager Instance
    {
        get
        {
            lock (s_lock)
            {
                if (s_instance == null)
                    s_instance = new M_WaresManager();
                return s_instance;
            }
        }
    }

    public void AddProductiveBuilding(Guid building, DAT_Ware.Type type)
    {
        m_wareStats[(int)type].buildingsProducing.Add(building);
    }

    public void RemoveProductiveBuilding(Guid building, DAT_Ware.Type type)
    {
        m_wareStats[(int)type].buildingsProducing.Remove(building);
    }

    public bool IsWareProduced(DAT_Ware.Type type)
    {
        return m_wareStats[(int)type].buildingsProducing.Count > 0;
    }

    public static void SAddProductiveBuilding(Guid building, DAT_Ware.Type type)
    {
        if (s_instance != null)
        {
            s_instance.AddProductiveBuilding(building, type);
        }
    }

    public static void SRemoveProductiveBuilding(Guid building, DAT_Ware.Type type)
    {
        if (s_instance != null)
        {
            s_instance.RemoveProductiveBuilding(building, type);
        }
    }

    public static bool SIsWareProduced(DAT_Ware.Type type)
    {
        if (s_instance != null)
        {
            return s_instance.IsWareProduced(type);
        }
        return false;
    }


    public DAT_Ware.Type GetRandWare()
    {
        List<DAT_Ware.Type> wares = M_BuildingManager.SGetAvailableWares();

        int count = wares.Count;
        int val = UnityEngine.Random.Range(1, count);

        if (s_randomized.Count < s_uniqueRandomizingTimes)
        {
            while (s_randomized.Contains(val))
            {
                val++;
                if (val >= count)
                    val = 1;
            }
            s_randomized.Add(val);
        }

        return wares[val];
    }

    public static void SEnableUniqueWareRandomizing(int times)
    {
        s_uniqueRandomizingTimes = times;
        s_randomized.Clear();
    }

    public static void SDisableUniqueWareRandomizing()
    {
        s_uniqueRandomizingTimes = 0;
        s_randomized.Clear();
    }

    public static DAT_Ware.Type SGetRandWare()
    {
        if (s_instance != null)
        {
            return s_instance.GetRandWare();
        }
        return DAT_Ware.Type.NOTHING;
    }

    public List<DAT_Ware.Type> GetAvailableWares()
    {
        List<DAT_Ware.Type> wares = new List<DAT_Ware.Type>();
        for (int i = 0; i < m_wareStats.Length; i++)
        {
            if (m_wareStats[i].buildingsProducing.Count > 0)
                wares.Add(m_wareStats[i].type);
        }
        return wares;
    }

    public static List<DAT_Ware.Type> SGetAvailableWares()
    {
        if (s_instance == null) return null;
        return s_instance.GetAvailableWares();
    }
}
