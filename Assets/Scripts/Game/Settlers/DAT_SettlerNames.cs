using UnityEngine;

/// <summary>
/// Names used to create a Settler in a SettlersManager.
/// @author Rivenort
/// </summary>
[CreateAssetMenu(fileName = "newSettlerNames", menuName = M_GameHelper.EDITOR_MENU_ITEM + "SettlerNames")]
public class DAT_SettlerNames : ScriptableObject
{
    public string[] names;
}
