using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Rivenort
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "newGameData", menuName = M_GameHelper.EDITOR_MENU_ITEM + "GameData")]
public class DAT_Game : ScriptableObject
{
    [Header("InGameResources")]
    public DAT_Resources resources;
    [Space(10)]
    [Header("Buildings")]
    public DAT_BuildingTemplates buildings;
    [Space(10)]
    [Header("Other")]
    public DAT_SettlerNames settlerNames;
    public DAT_SettlerPortraits settlerPortraits;
}
