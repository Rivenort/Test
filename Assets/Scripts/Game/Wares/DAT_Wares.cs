using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Rivenort
/// </summary>
[CreateAssetMenu(fileName = "wares", menuName = M_GameHelper.EDITOR_MENU_ITEM + "wares")]
public class DAT_Wares : ScriptableObject
{
    public DAT_Ware[] wares;
}
