
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DevResources : MonoBehaviour
{
    public TMP_InputField inputFood;
    public TMP_InputField inputResearch;

    public GameObject elemBuilding;
    public GameObject container;

    public Color elemBgColorA;
    public Color elemBgColorB;


    private void Start()
    {
        if (inputFood == null ||
            inputResearch == null)
            Debug.LogWarning("Component hasn't been fully initialized!");




        List<GameObject> rows = new List<GameObject>();

        int i = 0;
        List<DAT_Building> buildings = M_BuildingManager.SGetBuildingsTemplates().GetBuildings();
        foreach (DAT_Building building in buildings)
        {
            GameObject temp = Instantiate(elemBuilding);
            //temp.GetComponent<UI_DevElemBuilding>().Setup(building);


            if (i % 2 == 0)
            {
                temp.GetComponent<Image>().color = elemBgColorA;
            }
            else
            {
                temp.GetComponent<Image>().color = elemBgColorB;
            }
            rows.Add(temp);
            i++;
        }

        GUIUtils.AddElementsToCleanYScrollContainer(container, rows);
    }

    private void OnEnable()
    {
        inputFood.text = M_InGameResourcesManager.GetFood().ToString();
        inputResearch.text = M_InGameResourcesManager.GetResearchPoints().ToString();
    }

    public void ApplyFood()
    {
        try
        {
            string temp = inputFood.text.Replace("\u200B", "");
            M_InGameResourcesManager.SAddFood(Int32.Parse(temp) - M_InGameResourcesManager.GetFood());
        }
        catch (FormatException e) { }
    }
        
    public void ApplyResearch()
    {
        try
        {
            string temp = inputResearch.text.Replace("\u200B", "");
            M_InGameResourcesManager.SAddResearch(Int32.Parse(temp) - M_InGameResourcesManager.GetResearchPoints());
        } catch (FormatException e) { }
        
    }

    public void ApplyBuildings()
    {
        foreach (UI_DevElemBuilding elem in container.GetComponentsInChildren<UI_DevElemBuilding>())
        {
            elem.ApplyChanges();
        }
    }
}
