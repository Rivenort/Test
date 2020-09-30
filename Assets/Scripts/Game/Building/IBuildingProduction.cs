using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the operations that can be done with a production building.
/// @author Rivenort
/// </summary>
public interface IBuildingProduction : IBuilding
{

    /// <returns> Food produced by this building </returns>
    int GetProducedFood();

    /// <returns> Research producedd by this building </returns>
    int GetProducedResearch();

    /// <returns>Ware produced by this building </returns>
    DAT_Ware.Type GetProducedWareType();

    /// <returns> Id of a settler working in this building</returns>
    Guid GetWorker();

    /// <param name="newWorker">id of a new worker - Settler</param>
    /// <returns> id of a previous worker </returns>
    Guid SetWorker(Guid newWorker);

    void SetData(DAT_BuildingProd data);

    int GetMood();

    bool IsWorking();
}
