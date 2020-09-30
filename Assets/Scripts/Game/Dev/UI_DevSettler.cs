using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DevSettler : MonoBehaviour
{
    private static UI_DevSettler s_instance = null;

    public TMP_InputField settlerName;
    public Image settlerPortrait;
    public TMP_InputField settlerUpkeep;
    public TMP_InputField settlerMoodValue;
    public Slider settlerMoodSlider;

    private Settler m_settler;

    void Start()
    {
        if (settlerName == null ||
            settlerPortrait == null ||
            settlerUpkeep == null ||
            settlerMoodValue == null ||
            settlerMoodSlider == null)
            Debug.LogWarning("Component hasn't been fully initialized!");
        if (s_instance != null)
            Debug.LogWarning("Once instance is allowed!");
        s_instance = this;
    }

    public void ShowSettler(ISettler settler)
    {
        /*
        if (settler == null)
        {
            Debug.LogWarning(typeof(Settler).Name + " cannot be null!");
            M_ScreenManager.SPreviousScreen();
            return;
        }

        m_settler = settler;
        settlerName.text = settler.m_name;
        settlerUpkeep.text = settler.m_upkeep.ToString();
        settlerMoodSlider.value = settler.m_mood;
        settlerMoodValue.text = settler.mood.ToString();

        settlerPortrait.sprite = M_SettlersManager.SGetPortrait(settler.GetPortraitId()); */
    }


    public void OnClickApply()
    {
        /*
        m_settler.SetName(settlerName.text.Replace("\u200B", ""));
        try
        {
            m_settler.SetMood(int.Parse(settlerMoodValue.text.Replace("\u200B", "")));
            m_settler.SetUpkeep(float.Parse(settlerUpkeep.text.Replace("\u200B", "")));
        } catch (FormatException e) { }
        */
    }
    
    public void OnMoodSliderUpdate()
    {
        settlerMoodValue.text = settlerMoodSlider.value.ToString();
    }

    public void OnMoodInputUpdate()
    {
        try
        {
            settlerMoodSlider.value = float.Parse(settlerMoodValue.text.Replace("\u200B", ""));
        }
        catch (FormatException e) { }
    }

    public static void SShowSettler(ISettler settler)
    {
        if (s_instance != null)
        {
            s_instance.ShowSettler(settler);
        }
    }
}
