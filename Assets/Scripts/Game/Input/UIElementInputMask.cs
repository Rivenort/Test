using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/// <summary>
/// For UI elements that doesn't want to propagate input events
/// futher into the game world (M_InputManager).
/// @author Rivenort
/// </summary>
public class UIElementInputMask : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private void OnDisable()
    {
        M_InputManager.SSetProcessingEvents(true);
    }
        
    public void OnPointerEnter(PointerEventData eventData)
    {
        M_InputManager.SSetProcessingEvents(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        M_InputManager.SSetProcessingEvents(true);
    }


}
