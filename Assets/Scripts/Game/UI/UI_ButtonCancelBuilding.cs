using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonCancelBuilding : MonoBehaviour
{
    public void OnClickCancelBuilding()
    {
        M_BuildingManager.SQuitBuilding();
    }
}
