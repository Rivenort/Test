using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ElemSettlerRemove : MonoBehaviour
{
    private static int s_count = 0;

    [Header("Background")]
    public Color backgroundAColor;
    public Color backgroundBColor;
    public Color removeColor;
    private Color m_baseColor;
   

    [Space(10)]
    [Header("Interface")]
    public TMP_Text name;
    public Button infoButton;
    public Button removeButton;
    public Button unremoveButton;
    public Image settlerPortrait;

    private ISettler m_settler;
    private GameObject m_settlerScreen;

    public void Setup(ISettler settler, GameObject settlerScreen)
    {
        if (settler == null)
        {
            Debug.LogWarning("Settler cannot be null!");
            return;
        }
        else if (settlerScreen == null)
        {
            Debug.LogWarning("SettlerScreen cannot be null!");
            return;
        }

        if (s_count % 2 == 0)
        {
            gameObject.GetComponent<Image>().color = backgroundAColor;
            m_baseColor = backgroundAColor;
        }
        else
        {
            gameObject.GetComponent<Image>().color = backgroundBColor;
            m_baseColor = backgroundBColor;
        }


        m_settler = settler;
        name.text = settler.GetName();

        settlerPortrait.sprite = M_SettlersManager.SGetPortrait(settler.GetPortraitId());
        m_settlerScreen = settlerScreen;

        s_count++;
    }

    public void OnClickInfo()
    {
        UI_ScreenSettler.SShowSettler(m_settler);
        M_ScreenManager.SChangeScreenPersistentBack(m_settlerScreen);
        UI_ScreenRemoveWorker.SSetIfInfoScreen(true);
    }

    public void OnClickRemove()
    {
        UI_ScreenRemoveWorker.SRemoveEmployee(m_settler);
        gameObject.GetComponent<Image>().color = removeColor;
        removeButton.gameObject.SetActive(false);
        unremoveButton.gameObject.SetActive(true);
    }

    public void OnClickUnremove()
    {
        UI_ScreenRemoveWorker.SRemoveEmployee(m_settler);
        gameObject.GetComponent<Image>().color = m_baseColor;
        unremoveButton.gameObject.SetActive(false);
        removeButton.gameObject.SetActive(true);
    }
}
