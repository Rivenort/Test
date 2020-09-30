using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class M_MiscManager : MonoBehaviour
{
    private static M_MiscManager s_instance = null;

    public GameObject cancelButton;

    private void Start()
    {
        if (s_instance != null)
            Debug.LogWarning("Only one instance allowed!");
        s_instance = this;
    }

    public static void ShowCancelButton(bool val)
    {
        if (s_instance != null && s_instance.cancelButton != null)
        {
            s_instance.cancelButton.gameObject.SetActive(val);
        }
    }

    public static void AddListenerCancelButton(UnityAction val)
    {
        if (s_instance != null && s_instance.cancelButton != null)
        {
            s_instance.cancelButton.GetComponent<Button>().onClick.AddListener(val);
        }
    }

    public static void RemoveListenersCancelButton()
    {
        if (s_instance != null && s_instance.cancelButton != null)
        {
            s_instance.cancelButton.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}
