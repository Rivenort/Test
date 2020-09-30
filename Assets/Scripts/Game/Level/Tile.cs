using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Rivenort
/// Should be in the parent object
/// </summary>
[RequireComponent(typeof(ClickableObject))]
public class Tile : MonoBehaviour
{
   
    public Color defaultHighlightColor = new Color(1f, 0.5f, 0.5f, 1f);
    public Color highlightOccupiedColor = new Color(1f, 0f, 0f, 0.4f); 
    private Color m_defaultColor = Color.white;

    public DAT_Tile data;
    public int i, j;

    private void Start()
    {
        //GetComponent<ClickableObject>().Func = () => { Debug.Log("tile clicked"); };
    }

    public void Highlight(bool val)
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        if (val)
            spriteRenderer.color = defaultHighlightColor;
        else
            spriteRenderer.color = m_defaultColor;
    }

    public void HighlightOccupied(bool val)
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        if (val && data.occupied)
            spriteRenderer.color = highlightOccupiedColor;
        else if (val)
            spriteRenderer.color = defaultHighlightColor;
        else
            spriteRenderer.color = m_defaultColor;
    }

    public bool IsOccupied()
    {
        return data.occupied;
    }

    public void SetOccupied(bool val)
    {
        data.occupied = val;
    }
}
