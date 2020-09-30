using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScreenBuild : MonoBehaviour
{
    private static UI_ScreenBuild s_instance = null;


    public GameObject container;
    public GameObject elemPrefab;
    public GameObject buildingDesc;

    public TMP_Text descBuildingName;
    public TMP_Text descBuildingWare;
    public TMP_Text descBuildingFood;
    public TMP_Text descBuildingResearch;
    public GameObject descDepContainer;
    public GameObject descDepRowPrefab;

    public Color elemBgColorA;
    public Color elemBgColorB;

    private void Start()
    {
        if (s_instance != null)
            Debug.LogWarning("Only one instance is allowed!");
        s_instance = this;
    }

    private void OnEnable()
    {
        OnOpen();
    }

    public void OnOpen()
    {
        if (M_BuildingManager.IsBuildingStarted())
        {
            M_BuildingManager.SQuitBuilding();
        }



        List<DAT_Building> buildings = M_BuildingManager.SGetAvailableBuildings();

        List<GameObject> rows = new List<GameObject>();
        int i = 0;
        foreach (DAT_Building building in buildings)
        {
            GameObject temp = Instantiate(elemPrefab);
            temp.GetComponent<UI_ElemBuilding>().Setup(building);

            if (i % 2 == 0)
            {
                temp.GetComponent<Image>().color = elemBgColorA;
            }
            else
            {
                temp.GetComponent<Image>().color = elemBgColorB;
            }
            i++;
            rows.Add(temp);
        }
        GUIUtils.AddElementsToCleanYScrollContainer(container, rows);

        foreach (UI_ElemBuilding elem in container.transform.GetComponentsInChildren<UI_ElemBuilding>())
        {
            elem.Setup();
        }
        buildingDesc.SetActive(false);
    }

    public static void SShowBuildingInfo(DAT_Building data)
    {
        /*
        if (s_instance != null)
        {
            s_instance.descBuildingWare.text = data.wareType.ToString();
            s_instance.descBuildingName.text = data.name;
            s_instance.descBuildingFood.text = data.foodPerDay.ToString();
            s_instance.descBuildingResearch.text = data.researchPerDay.ToString();

            List<GameObject> rows = new List<GameObject>();
            int i = 0;
            foreach (DAT_BuildingTemplate.BuildDependency dep in data.dependencies)
            {
                GameObject temp = Instantiate(s_instance.descDepRowPrefab);


                temp.GetComponent<UI_RowDependency>().Setup(M_BuildingManager.SGetDatBuilding(dep.buildingRequired).name, dep.requiredMood);

                if (i % 2 == 0)
                {
                    temp.GetComponent<Image>().color = s_instance.elemBgColorA;
                }
                else
                {
                    temp.GetComponent<Image>().color = s_instance.elemBgColorB;
                }
                i++;
                rows.Add(temp);
            }
            GUIUtils.AddElementsToCleanYScrollContainer(s_instance.descDepContainer, rows);



            s_instance.buildingDesc.SetActive(true);
        }*/
    }
}
