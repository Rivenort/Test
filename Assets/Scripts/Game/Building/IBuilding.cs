using System;
using UnityEngine;

/// <summary>
/// Base interface for buildings. Defines operations
/// that can be done with a generic building. 
/// @author Rivenort
/// </summary>
public interface IBuilding : ITimeTriggerable
{
    /// <returns> Unique id of an instance </returns>
    Guid GetId();

    /// <returns> Building's name </returns>
    string GetName();


    /// <returns> Building's description </returns>
    string GetDesc();


    /// <returns> Name of the prefab in default directory for
    /// buildings i.e. "Prefabs/Game/Buildings/" </returns>
    string GetPrefab();


    /// <returns> Build time in days </returns>
    int GetBuildTime();


    /// <returns> Building's food cost </returns>
    int GetCostFood();

    /// <returns> Building's research points cost </returns>
    int GetCostResearch();

    /// <returns> Value that describes how fast the cost grows along
    /// new built buildings. </returns>
    float GetCostFactor();

    /// <returns> True if building is available on start </returns>
    bool IsAvailableOnStart();

    /// <returns> Buillding's icon </returns>
    Sprite GetIcon();

    /// <summary> If Logic is supposed to be processed. </summary>
    void Pause(bool val);


    /// <returns> Building physical representation </returns>
    BuildingPhysics GetPhysics();

    DAT_Building GetData();

    GameObject GetBuildButton();

    void StartBuilding();

    void SetActionOnBuildingFinished(Action<IBuilding> func);

}
