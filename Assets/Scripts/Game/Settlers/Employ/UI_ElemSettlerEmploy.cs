using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @author Rivenort
/// </summary>
public class UI_ElemSettlerEmploy : MonoBehaviour
{
    private static int s_count = 0;
    [Header("Background")]
    public Color backgroundAColor;
    public Color backgroundBColor;
    public Color employColor;
    private Color m_baseColor;

    [Space(10)]
    [Header("Interface")]
    public TMP_Text name;
    public Image settlerPortrait;

    public Button infoButton;
    public Button employButton;
    public Button deemployButton;

    private Settler m_settler;
    private GameObject m_settlerScreen;


    public void Setup(Settler settler, GameObject settlerScreen)
    {

        if (settler == null)
        {
            Debug.LogWarning("Settler cannot be null!");
            return;
        } else if (settlerScreen == null)
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
        infoButton.onClick.AddListener(OnClickInfo);
        employButton.onClick.AddListener(OnClickEmploy);
        deemployButton.onClick.AddListener(OnClickDeemploy);

        s_count++;
    }

    private void OnClickInfo()
    {
        UI_ScreenSettler.SShowSettler(m_settler);
    }

    private void OnClickEmploy()
    {
        if (UI_ScreenEmploy.IsMaxEmployees()) return;

        UI_ScreenEmploy.SAddEmployee(m_settler);
        gameObject.GetComponent<Image>().color = employColor;
        employButton.gameObject.SetActive(false);
        deemployButton.gameObject.SetActive(true);
    }

    private void OnClickDeemploy()
    {
        UI_ScreenEmploy.SAddEmployee(m_settler);
        gameObject.GetComponent<Image>().color = m_baseColor;
        employButton.gameObject.SetActive(true);
        deemployButton.gameObject.SetActive(false);
    }


}
