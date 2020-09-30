using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScreenRemoveWorker : MonoBehaviour
{
    private static UI_ScreenRemoveWorker s_instance = null;

    private static List<ISettler> s_workersToRemove = new List<ISettler>();

    public GameObject settlerScreen;
    public GameObject settlerUIprefab;
    public GameObject container;
    public Button removeButton;
    public TMP_Text setlersCount;

    public const string FORMAT_EMPLOYEES = "{0} / {1} workers choosen";
    private int m_settlersAvailable;

    private bool m_isInfoScreen;

    private void Start()
    {
        if (s_instance != null)
            Debug.LogWarning("Only one instance is allowed!");
        s_instance = this;
    }

    private void OnEnable()
    {
        /*
        if (m_isInfoScreen)
        {
            m_isInfoScreen = false;
        } 
        else
        {
            removeButton.gameObject.SetActive(false);
            s_workersToRemove.Clear();
            for (int i = 0; i < container.transform.childCount; i++)
            {
                GameObject.Destroy(container.transform.GetChild(i).gameObject);
            }
            //List<Settler> settlers = UI_ScreenBuilding.GetCurrentBuilding().settlers;
            m_settlersAvailable = settlers.Count;


            for (int i = 0; i < settlers.Count; i++)
            {
                GameObject temp = Instantiate(settlerUIprefab);
                temp.transform.SetParent(container.transform);
                temp.GetComponent<UI_ElemSettlerRemove>().Setup(settlers[i], settlerScreen);
                RectTransform rectTransform = temp.GetComponent<RectTransform>();
                float yOffset = container.GetComponent<RectTransform>().rect.height / 2 - rectTransform.rect.height / 2;
                rectTransform.localPosition = new Vector3(0, yOffset - rectTransform.rect.height * i, 0);
            }
            //setlersCount.text = string.Format(FORMAT_EMPLOYEES, s_workersToRemove.Count, UI_ScreenBuilding.GetCurrentBuilding().data.requiredSettlers);
        } */
    }

    private void RefreshUI()
    {
        if (s_workersToRemove.Count > 0)
            removeButton.gameObject.SetActive(true);
        else
            removeButton.gameObject.SetActive(false);

        //setlersCount.text = string.Format(FORMAT_EMPLOYEES, s_workersToRemove.Count, UI_ScreenBuilding.GetCurrentBuilding().data.requiredSettlers);
    }

    public void RemoveEmployee(ISettler settler)
    {
        if (s_workersToRemove.Contains(settler))
        {
            s_workersToRemove.Remove(settler);
        }
        else
        {
            s_workersToRemove.Add(settler);
        }
        RefreshUI();
    }


    public static void SRemoveEmployee(ISettler settler)
    {
        if (s_instance != null)
        {
            s_instance.RemoveEmployee(settler);
        }
    }

    public void OnRemove()
    {
        //UI_ScreenBuilding.RemoveWorker(s_workersToRemove);
        M_ScreenManager.SHideScreen();
    }

    public void SetIfInfoScreen(bool val)
    {
        m_isInfoScreen = val;
    }

    public static void SSetIfInfoScreen(bool val)
    {
        if (s_instance != null)
        {
            s_instance.SetIfInfoScreen(val);
        }
    }
}
