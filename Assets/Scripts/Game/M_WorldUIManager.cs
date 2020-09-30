using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// @author Rivenort
/// </summary>
public class M_WorldUIManager : MonoBehaviour
{
    private static M_WorldUIManager s_instance = null;

    [Header("Misc")]
    public Button smallBuildButton;

    void Start()
    {
        if (s_instance != null)
        {
            Debug.LogWarning("Only one instance allowed!");
        }
        s_instance = this;
    }

   
    void Update()
    {
        
    }

    public static void ShowSmallBuildButton(bool val)
    {
        if (s_instance != null && s_instance.smallBuildButton != null)
        {
            s_instance.smallBuildButton.gameObject.SetActive(val);
        }
    }

    public static void AddListenerSmallBuildButton(UnityAction Func)
    {
        if (s_instance != null && s_instance.smallBuildButton != null)
        {
            s_instance.smallBuildButton.onClick.AddListener(Func);
        }
    }

    public static void SetPositionSmallBuildButton(Vector3 pos)
    {
        if (s_instance != null && s_instance.smallBuildButton != null)
        {
            s_instance.smallBuildButton.transform.position = pos;
        }
    }

    public static Rect GetSizeSmallBuildButton()
    {
        if (s_instance != null && s_instance.smallBuildButton != null)
        {
            return s_instance.smallBuildButton.GetComponent<RectTransform>().rect;
        }
        return Rect.zero;
    }
}
