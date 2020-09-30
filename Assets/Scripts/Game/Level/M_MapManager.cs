using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

/// <summary>
/// Singleton
/// @author Rivenort
/// </summary>
public sealed class M_MapManager : UT_IClearable
{
    private static M_MapManager s_instance = null;
    private static readonly object s_lock = new object();
    public static int TILE_W = 7;
    public static int TILE_H = 8;

    public static float DEFAULT_OBJECT_Y = 0.2f;
    private static float DEPTH_FACTOR = 0.1f;

    private DAT_TiledMap m_tiledMap;
    M_MapManager()
    {
    }

    public static M_MapManager Instance
    {
        get
        {
            lock (s_lock)
            {
                if (s_instance == null)
                    s_instance = new M_MapManager();
                return s_instance;
            }
        }
    }

    public void Clear()
    {
        
    }


    public void SetTiledMap(DAT_TiledMap tiledMap)
    {
        m_tiledMap = tiledMap;
    }

    public DAT_TiledMap GetTiledMap()
    {
        return m_tiledMap;
    }

    public static DAT_TiledMap SGetTiledMap()
    {
        if (s_instance != null)
        {
            return s_instance.GetTiledMap();
        }
        return null;
    }

    public static GameObject SGetTileObject(int i, int j)
    {
        if (s_instance != null)
        {
            return s_instance.m_tiledMap.GetTileObject(i, j);
        }
        return null;
    }

    public static void HighlightOccupied(bool val)
    {
        if (s_instance != null)
        {
            s_instance.m_tiledMap.HighlightOccupied(val);
        }
    }

    public static float SGetYDepthValue(float zVar)
    {
        return DEFAULT_OBJECT_Y + (1f / zVar) * DEPTH_FACTOR;
    }

    public static float SGetHighYDepthValue(float zVar)
    {
        return DEFAULT_OBJECT_Y + (1f / zVar) * DEPTH_FACTOR + DEFAULT_OBJECT_Y / 2;
    }

}
