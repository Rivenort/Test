using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The following component takes care of the Building's UI management.
/// </summary>
public class UI_ScreenBuilding : MonoBehaviour
{
    private static UI_ScreenBuilding s_instance = null;

    [Header("Generic Info")]
    [Space(10)]
    public TMP_Text buildingName;
    public TMP_Text buildingDesc;

    [Space(10)]
    [Header("Productive Building")]
    public GameObject productiveBuildingContent;
    public TMP_Text wareProd;
    public TMP_Text foodPerDay;
    public TMP_Text researchPerDay;
    public Image workerImage;
    public Sprite defaultWorkerImg;
    public TMP_Text workerName;
    public Button employButton;
    public Button removeButton;
    public Button infoButtonProd;

    [Space(10)]
    [Header("House")]
    public GameObject houseBuildingContent;
    public TMP_Text settlersNumber;
    public Button infoButtonHouse;

    public const string FORMAT_PER_DAY = "{0} per day";
    public const string FORMAT_SETTLERS = "{0} settlers";

    private IBuilding m_currentBuilding = null;

    private void OnEnable()
    {
        if (m_currentBuilding != null)
        {
            RefreshUI();
        }
    }

    void Start()
    {

        if (buildingName == null ||
            buildingDesc == null ||
            productiveBuildingContent == null ||
            wareProd == null ||
            foodPerDay == null ||
            researchPerDay == null ||
            workerImage == null ||
            workerName == null ||
            defaultWorkerImg == null ||
            infoButtonProd == null ||
            employButton == null ||
            removeButton == null ||
            houseBuildingContent == null ||
            settlersNumber == null ||
            infoButtonHouse == null)
            Debug.LogWarning("Component hasn't been fully initialized!");
        if (s_instance != null)
            Debug.LogWarning("There should be only only instance of that component!");
            
        s_instance = this;
    }



    public void ShowStatsProduction(IBuildingProduction building)
    {
        m_currentBuilding = building;
        // show stats
        buildingName.text = building.GetName();
        buildingDesc.text = building.GetDesc();
        foodPerDay.text = string.Format(FORMAT_PER_DAY, building.GetProducedFood());
        researchPerDay.text = string.Format(FORMAT_PER_DAY, building.GetProducedResearch());
        wareProd.text = building.GetProducedWareType().ToString();
        // check if someone's working
        Guid settler = building.GetWorker();
        /* Done in RefreshUI()
        if (settler == Guid.Empty) // no settler
        {
            removeButton.gameObject.SetActive(false);
            employButton.gameObject.SetActive(true);
        } else
        {
            removeButton.gameObject.SetActive(true);
            employButton.gameObject.SetActive(false);
        }*/


        houseBuildingContent.SetActive(false);
        productiveBuildingContent.SetActive(true);

        /* done in RefreshUI()
        if (building.GetWorker() != Guid.Empty)
        {
            workerImage.sprite = M_SettlersManager.SGetPortraitOfSettler(building.GetWorker());
        }*/

        M_ScreenManager.SChangeScreenPersistentBack(s_instance.gameObject);
    }

    public void ShowStatsHouse(IBuildingHouse building)
    {
        m_currentBuilding = building;
        buildingName.text = building.GetName();
        buildingDesc.text = building.GetDesc();

        productiveBuildingContent.SetActive(false);
        houseBuildingContent.SetActive(true);


        M_ScreenManager.SChangeScreenPersistentBack(s_instance.gameObject);
    }

    public static void SShowStats(IBuilding building)
    {
        if (building == null)
        {
            Debug.LogWarning("Building cannot be null!");
            return;
        }

        if (s_instance != null)
        {
            if (building is IBuildingHouse)
                s_instance.ShowStatsHouse(building as IBuildingHouse);
            else if (building is IBuildingProduction)
                s_instance.ShowStatsProduction(building as IBuildingProduction);
        }
    }

    

    private void RefreshUI()
    {
        buildingName.text = m_currentBuilding.GetName();
        buildingDesc.text = m_currentBuilding.GetDesc();

        if (m_currentBuilding is IBuildingProduction)
        {
            IBuildingProduction prod = (IBuildingProduction)m_currentBuilding;
            foodPerDay.text = string.Format(FORMAT_PER_DAY, prod.GetProducedFood());
            researchPerDay.text = string.Format(FORMAT_PER_DAY, prod.GetProducedResearch());
            if (prod.GetWorker() != Guid.Empty)
            {
                workerImage.sprite = M_SettlersManager.SGetPortraitOfSettler(prod.GetWorker());
                workerName.text = M_SettlersManager.SGetNameOfSettler(prod.GetWorker());

                employButton.gameObject.SetActive(false);
                removeButton.gameObject.SetActive(true);
                infoButtonProd.gameObject.SetActive(true);
            } else
            {
                workerImage.sprite = defaultWorkerImg;
                workerName.text = "None";

                employButton.gameObject.SetActive(true);
                removeButton.gameObject.SetActive(false);
                infoButtonProd.gameObject.SetActive(false);
            }

           
        }
    }



    public static void AssignWorker(Guid settler)
    {
        if (s_instance == null) return;

        
        M_BuildingManager.SRemoveSettler(settler);
        M_SettlersManager.SAssignWorkplace(settler, s_instance.m_currentBuilding.GetId());
        M_BuildingManager.SAssignSettler(s_instance.m_currentBuilding.GetId(), settler);
        
        s_instance.RefreshUI();
    }

    public static IBuilding GetCurrentBuilding()
    {
        if (s_instance != null)
        {
            return s_instance.m_currentBuilding;
        }
        return null;
    }

    /// <summary>[SetInInspector] On info button click event </summary>
    public void OnProductionInfoButtonClick()
    {
        if (!(m_currentBuilding is IBuildingProduction))
        {
            Debug.LogWarning("Building is not a production one.");
            return;
        }
        Guid settler = ((IBuildingProduction)m_currentBuilding).GetWorker();
        if (settler == Guid.Empty)
        {
            Debug.LogWarning("Production Building doesn't have a worker.");
            return;
        }
        UI_ScreenSettler.SShowSettler(settler);
    }

    /// <summary>[SetInInspector] On info button click event </summary>
    public void OnHouseInfoButtonClick()
    {
        if (!(m_currentBuilding is IBuildingHouse))
        {
            Debug.LogWarning("Building is not a house.");
            return;
        }

        UI_ScreenSettlers.SShowSettlers(((IBuildingHouse)m_currentBuilding).GetSettlers());
    }

    /// <summary>[SetInInspector] </summary>
    public void OnEmployButtonClick()
    {
        if (!(m_currentBuilding is IBuildingProduction))
        {
            Debug.LogWarning("Building is not a production one.");
            return;
        }
        UI_ScreenEmploy.SShowEmployScreen();
    }

    /// <summary>[SetInInspector] </summary>
    public void OnRemoveButtonClick()
    {
        if (!(m_currentBuilding is IBuildingProduction))
        {
            Debug.LogWarning("Building is not a production one.");
            return;
        }
        Guid settler = M_BuildingManager.SRemoveSettler(m_currentBuilding.GetId());
        M_SettlersManager.SRemoveWorkplace(settler);
        RefreshUI();
    }
}
