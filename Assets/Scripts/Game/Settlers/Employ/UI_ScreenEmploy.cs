using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScreenEmploy : MonoBehaviour
{
    private static UI_ScreenEmploy s_instance = null;

    private ISettler m_employee = null;

    public GameObject settlerScreen;
    public GameObject settlerUIprefab;
    public GameObject container;
    public Button     employButton;
    public TMP_Text   settlersCount;

    private int m_settlersAvailable;

    private void Start()
    {
        if (s_instance != null)
            Debug.LogWarning("Only one instance is allowed!");
        s_instance = this;
    }

    private void OnEnable()
    {
      

    }

    private void RefreshUI()
    {
        if (m_employee != null)
            employButton.gameObject.SetActive(true);
        else
            employButton.gameObject.SetActive(false);
    }

    public void AddEmployee(Settler settler)
    {
        if (settler == m_employee)
        {
            m_employee = null;
            return;
        }
        m_employee = settler;
        RefreshUI(); 
    }


    public static void SAddEmployee(Settler settler)
    {
        if (s_instance != null)
        {
            s_instance.AddEmployee(settler);
        }
    }


    public void OnEmploy()
    {
        UI_ScreenBuilding.AssignWorker(m_employee.GetId());
        M_ScreenManager.SChangeScreen(this.gameObject);
    }

    public void ShowEmployScreen()
    {
        m_employee = null;
        employButton.gameObject.SetActive(false);
        m_settlersAvailable = 0;
        int i;
        for (i = 0; i < container.transform.childCount; i++)
        {
            GameObject.Destroy(container.transform.GetChild(i).gameObject);
        }
        List<ISettler> settlers = M_SettlersManager.SGetSettlers();

        i = 0;
        foreach (Settler s in settlers)
        {
            if (s.GetWorkplace() == Guid.Empty)
            {
                m_settlersAvailable++;
                GameObject temp = Instantiate(settlerUIprefab);
                temp.transform.SetParent(container.transform);
                temp.GetComponent<UI_ElemSettlerEmploy>().Setup(s, settlerScreen);
                RectTransform rectTransform = temp.GetComponent<RectTransform>();
                float yOffset = container.GetComponent<RectTransform>().rect.height / 2 - rectTransform.rect.height / 2;
                rectTransform.localPosition = new Vector3(0, yOffset - rectTransform.rect.height * i, 0);
                i++;
            }
        }
        settlersCount.text = m_settlersAvailable.ToString();

        M_ScreenManager.SChangeScreenPersistentBack(this.gameObject);
    }

    public static void SShowEmployScreen()
    {
        if (s_instance == null) return;
        s_instance.ShowEmployScreen();
    }

    public static bool IsMaxEmployees()
    {
        if (s_instance == null) return false;
        if (s_instance.m_employee == null) return false;
        return true;
    }
}
