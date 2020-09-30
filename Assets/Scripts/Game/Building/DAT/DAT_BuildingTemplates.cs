using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAT_BuildingTemplates : ScriptableObject
{
    public List<DAT_BuildingHouse> house;
    public List<DAT_BuildingProd> production;

    public List<DAT_Building> GetBuildings()
    {
        List<DAT_Building> buildings = new List<DAT_Building>();
        if (house != null)
            buildings.AddRange(house);
        if (production != null)
            buildings.AddRange(production);
        return buildings;
    }
}
