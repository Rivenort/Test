using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScreenSettler : MonoBehaviour
{
    private static UI_ScreenSettler s_instance = null;

    public Sprite spriteWhenNoWorkplace;
    public TMP_Text settlerName;
    public Image settlerPortrait;
    public TMP_Text settlerUpkeep;
    public Slider settlerMood;
    public TMP_Text moodValue;

    [Space(10)]
    [Header("Preferences Sub Page")]
    public GameObject preferencesSubPage;
    public GameObject favoriteWaresContainer;
    public GameObject favoriteBuildingsContainer;
    public GameObject favoriteThingRowElementPrefab;
    public Color elemBgHighlightColor;

    [Space(10)]
    public Button devSettings;

    [Space(10)]
    public Button workplaceButton;
    public Button homeButton;

    private ISettler m_settler;

    void Start()
    {
        if ( settlerName == null ||
            workplaceButton == null)
            Debug.LogWarning("Component hasn't been fully initialized!");
        if (s_instance != null)
            Debug.LogWarning("Once instance is allowed!");
        s_instance = this;

        devSettings.onClick.AddListener(() => { UI_DevSettler.SShowSettler(m_settler); });
    }

    private void OnEnable()
    {
        /*
        if (m_settler != null)
        {
            ShowSettler(m_settler);
        } */
    }

    public void ShowSettler(ISettler settler)
    {
        if (settler == null)
        {
            Debug.LogWarning(typeof(ISettler).Name + " cannot be null!");
            M_ScreenManager.SPreviousScreen();
            return;
        }


        m_settler = settler;
        settlerName.text = settler.GetName();
        settlerUpkeep.text = settler.GetUpkeep().ToString();
        settlerMood.value = settler.GetMood();
        moodValue.text = string.Format("{0}%", settler.GetMood());

        // Preferences

        List<GameObject> favBuildings = new List<GameObject>();
        foreach (FavoriteBuilding favoriteBuilding in settler.GetFavoriteBuildings())
        {
            GameObject obj = Instantiate(favoriteThingRowElementPrefab);
            if (favoriteBuilding.active)
                obj.GetComponent<Image>().color = elemBgHighlightColor;
            obj.GetComponentInChildren<TMP_Text>().text = M_BuildingManager.SGetDatBuilding(favoriteBuilding.buildingTemplId).name;
            obj.GetComponentInChildren<Slider>().value = favoriteBuilding.maxMood;
            favBuildings.Add(obj);
        }

        List<GameObject> favWares = new List<GameObject>();
        foreach (FavoriteWare favoriteWare in settler.GetFavoriteWares())
        {
            GameObject obj = Instantiate(favoriteThingRowElementPrefab);
            if (favoriteWare.active)
                obj.GetComponent<Image>().color = elemBgHighlightColor;
            obj.GetComponentInChildren<TMP_Text>().text = favoriteWare.ware.ToString();
            obj.GetComponentInChildren<Slider>().value = favoriteWare.mood;
            favWares.Add(obj);
        }
        GUIUtils.AddElementsToCleanYScrollContainer(favoriteBuildingsContainer, favBuildings);
        GUIUtils.AddElementsToCleanYScrollContainer(favoriteWaresContainer, favWares);
        preferencesSubPage.SetActive(false);


        settlerPortrait.sprite = M_SettlersManager.SGetPortrait(settler.GetPortraitId());

        if (settler.GetWorkplace() != System.Guid.Empty)
            workplaceButton.image.sprite = M_BuildingManager.SGetBuilding(settler.GetWorkplace()).GetIcon();
        else
            workplaceButton.image.sprite = spriteWhenNoWorkplace;

        if (settler.GetHouse() != System.Guid.Empty)
            homeButton.image.sprite = M_BuildingManager.SGetBuilding(settler.GetHouse()).GetIcon();
        else
            homeButton.image.sprite = spriteWhenNoWorkplace;


        M_ScreenManager.SChangeScreenPersistentBack(this.gameObject);
    }

    public void OnClickWorkplace()
    {
        if (m_settler.GetWorkplace() != null)
            UI_ScreenBuilding.SShowStats(M_BuildingManager.SGetBuilding(m_settler.GetWorkplace()));
    }

    public void OnClickHome()
    {
        if (m_settler.GetHouse() != null)
            UI_ScreenBuilding.SShowStats(M_BuildingManager.SGetBuilding(m_settler.GetHouse()));
    }

    public static void SShowSettler(ISettler settler)
    {
        if (s_instance != null)
        {
            s_instance.ShowSettler(settler);
        } 
    }

    public static void SShowSettler(Guid id)
    {
        if (s_instance != null)
        {
            s_instance.ShowSettler(M_SettlersManager.SGetSettler(id));
        }
    }
}
