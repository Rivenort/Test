using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SAV_BuildingProd
{
    public Vector3 pos;
    public Guid id;
    public Guid worker;
    public DAT_BuildingProd data;
    public BuildingProd.State state;
    public float timeElapsed;

    public SAV_BuildingProd(BuildingProd buildingProd)
    {
        pos = buildingProd.gameObject.transform.position;
        id = buildingProd.GetId();
        worker = buildingProd.GetWorker();
        data = (DAT_BuildingProd) buildingProd.GetData();
        state = buildingProd.GetState();
        timeElapsed = buildingProd.GetTimeElapsed();
    }

    public BuildingProd Instantiate()
    {
        GameObject newBuilding = null;
        if (this.state == BuildingProd.State.ON_MOVING)
            newBuilding = this.data.InstantiateBuilding(this.pos, M_GameHelper.Group.TEMP);
        else
            newBuilding = this.data.InstantiateBuilding(this.pos, M_GameHelper.Group.BUILDINGS);

        BuildingProd comp = newBuilding.GetComponent<BuildingProd>();

        comp.SetId(id);
        comp.SetTimeElapsed(timeElapsed);
        comp.SetData(data);
        comp.SetState(state);
        comp.SetWorker(worker);

        return comp;
    }

    public static List<BuildingProd> Instantiate(List<SAV_BuildingProd> saves)
    {
        List<BuildingProd> res = new List<BuildingProd>();
        foreach (SAV_BuildingProd sav in saves)
        {
            res.Add(sav.Instantiate());
        }
        return res;
    }

    public static List<IBuilding> ToIBuilding(List<SAV_BuildingProd> saves)
    {
        List<IBuilding> res = new List<IBuilding>();
        foreach(SAV_BuildingProd sav in saves)
        {
            res.Add(sav.Instantiate());
        }
        return res;
    }
}
