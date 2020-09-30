using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// @author Rivenort
/// </summary>
public class UI_ScreenStats : MonoBehaviour
{
    public TMP_Text settlersText;
    public TMP_Text upkeepText;
    public TMP_Text foodText;
    public TMP_Text researchText;



    private void Start()
    {
        if (settlersText == null ||
            upkeepText == null ||
            foodText == null ||
            researchText == null)
        {
            Debug.LogWarning("Component is not fully initialized!");
 
        } 
    }

    public void OnOpen()
    {
        settlersText.text = M_SettlersManager.SGetSettlersCount().ToString();
        upkeepText.text = M_SettlersManager.SGetUpkeep().ToString();
        foodText.text = M_BuildingManager.SGetFoodPerDay().ToString();
        researchText.text = M_BuildingManager.SGetResearchPerDay().ToString();

    }

    public void OnSettlersStatsClick()
    {
        UI_ScreenSettlers.SShowSettlers();
    }


}
