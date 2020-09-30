using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SAV_Game
{
    public List<SAV_BuildingProd> buildingsProd;
    public List<SAV_BuildingHouse> buildingsHouse;
    public List<ISettler> settlers;
    public SAV_InGameResources resources;

    public SAV_Game(List<IBuilding> buildings, List<ISettler> settlers, SAV_InGameResources resources)
    {
        buildingsProd = new List<SAV_BuildingProd>();
        buildingsHouse = new List<SAV_BuildingHouse>();

        foreach (IBuilding building in buildings)
        {
            if (building is BuildingHouse)
            {
                buildingsHouse.Add(new SAV_BuildingHouse(((BuildingHouse) building)));
            } else if (building is BuildingProd)
            {
                buildingsProd.Add(new SAV_BuildingProd(((BuildingProd)building)));
            }
        }
        this.settlers = settlers;
        this.resources = resources;
    }

    public List<IBuilding> GetBuildings()
    {
        List<IBuilding> buildings = new List<IBuilding>();
        buildings.AddRange(SAV_BuildingProd.ToIBuilding(buildingsProd));
        buildings.AddRange(SAV_BuildingHouse.ToIBuilding(buildingsHouse));
        return buildings;
    }
}
