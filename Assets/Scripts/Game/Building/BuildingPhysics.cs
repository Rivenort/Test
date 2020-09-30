using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingPhysics
{
    [SerializeField] private bool[] m_area;
    [SerializeField] private int m_width;
    [SerializeField] private int m_height;

    public bool[] GetArea()
    {
        return m_area;
    }

    public int GetWidth()
    {
        return m_width;
    }

    public int GetHeight()
    {
        return m_height;
    }
}
