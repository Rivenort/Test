using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DevElemBuilding : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text name;
    public Image icon;

    [Space(10)]
    [Header("Requirements")]
    public TMP_InputField reqFood;
    public TMP_InputField reqResearch;
    public TMP_InputField reqWorkers;
    public TMP_InputField costFactor;

    [Space(10)]
    [Header("Requirements")]
    public TMP_InputField suppFood;
    public TMP_InputField suppResearch;
    public TMP_InputField suppSettlers;

    [Space(10)]
    [Header("Other")]
    public TMP_InputField buildTime;

    [Space(10)]
    [Header("Data")]
    public DAT_Building data;
    public DAT_Building backupData;

    void Start()
    {
        if (reqFood == null ||
            reqResearch == null ||
            reqWorkers == null ||
            costFactor == null ||
            suppFood == null ||
            suppResearch == null ||
            suppSettlers == null ||
            buildTime == null ||
            data == null ||
            backupData == null)
            Debug.LogWarning("Component hasn't been fully initialized!");
        this.name.text = data.name;
        //this.icon.sprite = data.prefab.GetComponent<Building>().spriteWhenReady;
    }

    private void OnEnable()
    {

    }

    public void Setup(DAT_Building data)
    {
        /*
        this.data = data;
        this.backupData = data;

        reqFood.text = data.requiredFood.ToString();
        reqResearch.text = data.requiredResearchPoints.ToString();
        costFactor.text = data.costFactor.ToString();

        suppFood.text = data.foodPerDay.ToString();
        suppResearch.text = data.researchPerDay.ToString();

        buildTime.text = data.buildingTime.ToString(); */
    }

    public void ApplyChanges()
    {
        /*
        try
        {
            data.requiredFood = Int32.Parse(reqFood.text.Replace("\u200B", ""));
            data.requiredResearchPoints = Int32.Parse(reqResearch.text.Replace("\u200B", ""));
            data.costFactor = float.Parse(costFactor.text.Replace("\u200B", ""));

            data.foodPerDay = Int32.Parse(suppFood.text.Replace("\u200B", ""));
            data.researchPerDay = Int32.Parse(suppResearch.text.Replace("\u200B", ""));

            data.buildingTime = float.Parse(buildTime.text.Replace("\u200B", ""));
        }
        catch (FormatException e) { } */
    }

    public void OnReset()
    {/*
        data.requiredFood = backupData.requiredFood;
        data.requiredResearchPoints = backupData.requiredResearchPoints;
        data.costFactor = backupData.costFactor;

        data.foodPerDay = backupData.foodPerDay;
        data.researchPerDay = backupData.researchPerDay;

        data.buildingTime = backupData.buildingTime;

        OnEnable(); */
    }

}
