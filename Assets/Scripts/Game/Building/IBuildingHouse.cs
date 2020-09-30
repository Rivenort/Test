using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Defines operations that can be done with a house building.
/// @author Rivenort
/// </summary>
public interface IBuildingHouse : IBuilding
{
    /// <returns> Number of settlers provided by this building </returns>
    int GetProvidedSettlers();

    /// <summary> Assigns new Settler to the house </summary>
    void AssignSettler(Guid newSettler);

    /// <returns>Returns ids of the settlers that live in this house.</returns>
    List<Guid> GetSettlers();

    void SetData(DAT_BuildingHouse data);
}
