using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Data class (more like structure).
/// DAT_Building stores generic data about building. It doesn't represent
/// properties specific for an instance. You should think about this class as
/// a template to create a Building instance.
/// @author Rivenort
/// </summary>
[System.Serializable]
public class DAT_Building
{
    public static string PATH_PREFAB = "Prefabs/Game/Buildings/";

    public int id;
    public string name;
    public string desc;
    public string prefabName;

    public int buildingTime; // days
    public int costFood;
    public int costResearch;
    public float costFactor;
    public bool availableOnStart; // if we can build a building on a game start

    [System.Serializable]
    public class BuildDependency
    {
        public int unlocksBuilding;
        public float requiredMood;
    }

    public List<BuildDependency> dependencies;

    public virtual GameObject InstantiateBuilding(Vector3 pos, M_GameHelper.Group group) {
        return null;
    }
}
