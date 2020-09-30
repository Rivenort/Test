using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// @author Rivenort
/// </summary>
public class MenuManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        MenuManager[] objs = (MenuManager[])FindObjectsOfType(typeof(MenuManager));
        if (objs.Length > 1)
        {
            Destroy(objs[0].gameObject);
        } 
    }
}
