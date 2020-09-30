using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Screen Manager manages the UI gameobjects called "Screen".
/// @author Rivenort
/// </summary>
public class M_ScreenManager : MonoBehaviour
{
    private static M_ScreenManager s_instance = null;

    private GameObject m_prevScreen = null;
    private GameObject m_currentScreen = null;

    public GameObject backButton;

    [Space(10)]
    public GameObject[] screensToBeReactivated;
    private Boolean m_activated = true;
    private const float ACTIVATION_TIME = 0.01f;
    private float m_activationTimeElapsed = 0f;

    /// <summary>
    /// Persistent Screen is a gameobject that is supposed to save a reference to his
    /// previous screen. Whenever a "persistent screen" is active again, reference to the
    /// previous screen is recovered.
    /// </summary>
    internal class PersistentScreen
    {
        internal GameObject screen;
        internal GameObject prevScreen;

        internal PersistentScreen(GameObject screen, GameObject prevScreen)
        {
            this.screen = screen;
            this.prevScreen = prevScreen;
        }
    }
    // stack possible
    private List<PersistentScreen> m_persistentScreens = new List<PersistentScreen>();



    private void Start()
    {
        if (backButton == null)
            Debug.LogWarning("Component is not fully initialized!");
        if (s_instance != null)
            Debug.LogWarning("Once instance of class is allowed!");
        s_instance = this;

        foreach (GameObject o in screensToBeReactivated)
        {
            o.SetActive(true);
        }
        m_activated = true;
        m_activationTimeElapsed = 0f;

    }


    private void Update()
    {

       if (m_activated && m_activationTimeElapsed > ACTIVATION_TIME)
        {
            foreach (GameObject o in screensToBeReactivated)
            {
                o.SetActive(false);
            }
            m_activated = false;
        }
        if (m_activated)
            m_activationTimeElapsed += Time.deltaTime;
    }

    public void ChangeScreen(GameObject screen)
    {
        if (m_currentScreen == screen)
        {
            HideScreen();
        } else
        {
            if (m_currentScreen != null)
                m_currentScreen.SetActive(false);
            m_prevScreen = m_currentScreen;
            m_currentScreen = screen;

            ApplyPersistentScreen();

            m_currentScreen.SetActive(true);
            backButton.SetActive(true);
            M_GameHelper.SPauseLevel();
        }
    }

    public void ChangeScreenPersistentBack(GameObject screen) {
        m_persistentScreens.Add(new PersistentScreen(m_currentScreen, m_prevScreen));
        ChangeScreen(screen);
    }


    public void PreviousScreen()
    {
        if (m_prevScreen != null)
        {
            if (m_currentScreen != null)
            {
                m_currentScreen.SetActive(false);
                GameObject temp = m_currentScreen;
                m_currentScreen = m_prevScreen;
                m_prevScreen = temp;
            } else
            {
                m_currentScreen = m_prevScreen;
                m_prevScreen = null;
            }
            
            ApplyPersistentScreen();

            m_currentScreen.SetActive(true);
            backButton.SetActive(true);
            M_GameHelper.SPauseLevel();
        } else 
        {
            HideScreen();
        }

    }

    public void HideScreen()
    {
        if (m_currentScreen != null)
        {
            
            m_currentScreen.SetActive(false);
            m_prevScreen = m_currentScreen;
            m_currentScreen = null;
            backButton.SetActive(false);
            M_GameHelper.SResumeLevel();
        }
    }


    private void ApplyPersistentScreen()
    {
        foreach (PersistentScreen p in m_persistentScreens)
        {
            if (p.screen == m_currentScreen)
            {
                m_prevScreen = p.prevScreen;
                m_persistentScreens.Remove(p);
                break;
            }
        }
    }

    public static void SHideScreen()
    {
        if (s_instance != null)
        {
            s_instance.HideScreen();
        }
    }

    public static void SChangeScreen(GameObject screen)
    {
        if (s_instance != null)
        {
            s_instance.ChangeScreen(screen);
        }
    }

    public static void SChangeScreenPersistentBack(GameObject screen)
    {
        if (s_instance != null)
        {
            s_instance.ChangeScreenPersistentBack(screen);
        }
    }

    public static void SPreviousScreen()
    {
        if (s_instance != null)
        {
            s_instance.PreviousScreen();
        }
    }
}
