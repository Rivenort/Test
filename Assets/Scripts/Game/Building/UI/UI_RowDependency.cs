using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_RowDependency : MonoBehaviour
{
    public TMP_Text buildingName;
    public TMP_Text moodText;
    public void Setup(string name, float requiredMood)
    { 
        buildingName.text = name;

        moodText.text = string.Format("required mood     {0}", (int)(requiredMood * 100) + 1);
    }
}
