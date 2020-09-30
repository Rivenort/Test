
using UnityEngine;
/// <summary>
/// Stores data in order to create a house.
/// @author Rivenort
/// </summary>
[System.Serializable]
public class DAT_BuildingHouse : DAT_Building
{
    public int settlersSupplied; // number of supplied settlers

    public override GameObject InstantiateBuilding(Vector3 pos, M_GameHelper.Group group)
    {
        UnityEngine.Object prefab = Resources.Load(PATH_PREFAB + prefabName);

        GameObject obj = (GameObject)GameObject.Instantiate(prefab, pos, Quaternion.identity);
        BuildingHouse building = obj.GetComponent<BuildingHouse>();

        building.SetData(this);

        M_GameHelper.AddToGroup(obj, group);
        return obj;
    }
}
