using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ElemBuilding : MonoBehaviour
{
    public DAT_Building buildingData;


    public Button buildButton;
    public Button infoButton;
    public TMP_Text foodText;
    public TMP_Text researchText;
    public TMP_Text buildingName;
    //public Image icon;

    public int requiredFood;
    public int requiredResearch;


    public void Setup(DAT_Building data)
    {
        buildingData = data;
        buildingName.text = data.name;

        UT_Pair<int, int> cost = M_BuildingManager.SGetBuildingCost(data.id);
        requiredFood = cost.first;
        requiredResearch = cost.second;

        if (data != null && requiredFood <= M_InGameResourcesManager.GetFood() && requiredResearch <= M_InGameResourcesManager.GetResearchPoints())
        {
            buildButton.interactable = true;
            buildButton.image.color = Color.green;
        }
        else
        {
            buildButton.interactable = false;
            buildButton.image.color = Color.red;
        }


    }


    public void Setup()
    {
        if (buildingData != null)
        {
            UT_Pair<int, int> cost = M_BuildingManager.SGetBuildingCost(buildingData.id);
            requiredFood = cost.first;
            requiredResearch = cost.second;

            foodText.text = requiredFood.ToString();
            researchText.text = requiredResearch.ToString();

            if (buildingData != null  && requiredFood <= M_InGameResourcesManager.GetFood() && requiredResearch <= M_InGameResourcesManager.GetResearchPoints())
            {
                buildButton.interactable = true;
                buildButton.image.color = Color.green;
            }
            else
            {
                buildButton.interactable = false;
                buildButton.image.color = Color.red;
            }
        }
    }

    public void OnClickBuild()
    {
        M_ScreenManager.SHideScreen();
        M_BuildingManager.SStartBuilding(buildingData);
    }

    public void OnClickInfo()
    {
        //UI_ScreenBuild.SShowBuildingInfo(buildingData);
    }

}
