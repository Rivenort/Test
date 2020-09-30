using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @see M_InputManager
/// @author Rivenort
/// </summary>
public class DraggableObject : MonoBehaviour
{
    public bool draggingEnabled;
    /// <summary> high Y value = closer to the camera (object will be above the others)</summary>
    public bool keepHighYDepthValue;
}
