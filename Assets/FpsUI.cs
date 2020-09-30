using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsUI : MonoBehaviour
{

    public TMP_Text text;
    

    // Update is called once per frame
    void Update()
    {
        text.text = "FPS: " + ((int)(1.0f / Time.smoothDeltaTime));
    }
}
