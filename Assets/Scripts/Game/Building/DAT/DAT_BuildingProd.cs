
using UnityEngine;
/// <summary>
/// Stores data need to create a production building.
/// @author Rivenort
/// </summary>
[System.Serializable]
public class DAT_BuildingProd : DAT_Building
{

    public int producedFood;
    public int producedResearch;
    public DAT_Ware.Type producedWare;

    public override GameObject InstantiateBuilding(Vector3 pos, M_GameHelper.Group group)
    {
        UnityEngine.Object prefab = Resources.Load(PATH_PREFAB + prefabName);

        GameObject obj = (GameObject)GameObject.Instantiate(prefab, pos, Quaternion.identity);
        BuildingProd building = obj.GetComponent<BuildingProd>();

        building.SetData(this);

        M_GameHelper.AddToGroup(obj, group);
        return obj;
    }

}
