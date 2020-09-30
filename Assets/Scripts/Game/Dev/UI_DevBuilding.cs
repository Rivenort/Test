using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DevBuilding : MonoBehaviour
{
    private IBuilding m_building;

    public TMP_Text inputFood;
    public TMP_Text inputResearch;
    public TMP_Text inputWorkers;
    public TMP_Text inputSettlers;

    private void OnEnable()
    {

        m_building = UI_ScreenBuilding.GetCurrentBuilding();
        /*
        inputFood.text = m_building.data.foodPerDay.ToString();
        inputResearch.text = m_building.data.researchPerDay.ToString(); */
    }

    public void ApplyChanges()
    {
        /*
        try
        {
        
            m_building.data.foodPerDay = Int32.Parse(inputFood.text.Replace("\u200B", ""));
            m_building.data.researchPerDay = Int32.Parse(inputResearch.text.Replace("\u200B", ""));

        }
        catch (FormatException e) { } */
       
    }
}
