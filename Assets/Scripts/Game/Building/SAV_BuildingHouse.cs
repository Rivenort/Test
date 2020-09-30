using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SAV_BuildingHouse
{
    public Vector3 pos;
    public Guid id;
    public List<Guid> settlers;
    public DAT_BuildingHouse data;
    public BuildingHouse.State state;
    public float timeElapsed;

    public SAV_BuildingHouse(BuildingHouse buildingHouse)
    {
        pos = buildingHouse.gameObject.transform.position;
        id = buildingHouse.GetId();
        settlers = buildingHouse.GetSettlers();
        data = (DAT_BuildingHouse)buildingHouse.GetData();
        state = buildingHouse.GetState();
        timeElapsed = buildingHouse.GetTimeElapsed();
    }

    public BuildingHouse Instantiate()
    {
        GameObject newBuilding = null;
        if (this.state == BuildingHouse.State.ON_MOVING)
            newBuilding = this.data.InstantiateBuilding(this.pos, M_GameHelper.Group.TEMP);
        else
            newBuilding = this.data.InstantiateBuilding(this.pos, M_GameHelper.Group.BUILDINGS);

        BuildingHouse comp = newBuilding.GetComponent<BuildingHouse>();

        comp.SetId(id);
        comp.SetTimeElapsed(timeElapsed);
        comp.SetData(data);
        comp.SetState(state);
        comp.AssignSettles(settlers);

        return comp;
    }

    public static List<BuildingHouse> Instantiate(List<SAV_BuildingHouse> saves)
    {
        List<BuildingHouse> res = new List<BuildingHouse>();
        foreach (SAV_BuildingHouse sav in saves)
        {
            res.Add(sav.Instantiate());
        }
        return res;
    }

    public static List<IBuilding> ToIBuilding(List<SAV_BuildingHouse> saves)
    {
        List<IBuilding> res = new List<IBuilding>();
        foreach (SAV_BuildingHouse sav in saves)
        {
            res.Add(sav.Instantiate());
        }
        return res;
    }
}

