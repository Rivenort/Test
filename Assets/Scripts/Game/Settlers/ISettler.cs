using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines operations that can be done with a Settler.
/// @author Rivenort
/// </summary>
public interface ISettler : ITimeTriggerable
{
    /// <returns> Id of a Settler </returns>
    Guid GetId();

    /// <returns> Name of a Settler </returns>
    string GetName();

    /// <returns> Upkeep of a Settler </returns>
    int GetUpkeep();

    /// <returns> Id of a Sprite (S_SettlersManager)</returns>
    int GetPortraitId();

    /// <returns> Current mood </returns>
    int GetMood();

    /// <returns> Favorite building mood </returns>
    int GetMoodBuilding();


    /// <returns> Favorite wares mood </returns>
    int GetMoodWares();

    /// <returns> Id of a house </returns>
    Guid GetHouse();

    /// <returns> Id of a workplace </returns>
    Guid GetWorkplace();

    /// <param name="newWorkplace"> New workplace's id </param>
    /// <returns> previous workplace id (Empty Guid if wasn't any)</returns>
    Guid SetWorkplace(Guid newWorkplace);

    /// <param name="newHouse"> New house's id </param>
    /// <returns> Previous house's id (Empty Guid if wasn't any)</returns>
    Guid SetHouse(Guid newHouse);

    /// <returns> Removed workplace </returns>
    Guid RemoveWorkplace();


    List<FavoriteBuilding> GetFavoriteBuildings();
    List<FavoriteWare> GetFavoriteWares();
}
