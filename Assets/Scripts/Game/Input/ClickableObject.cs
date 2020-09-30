using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @see M_InputManager
/// @author Rivenort
/// </summary>
public class ClickableObject : MonoBehaviour
{
    public Action Func;
    public bool disabled;

    public void OnClick()
    {
        if (Func != null)
            Func();
    }
}
