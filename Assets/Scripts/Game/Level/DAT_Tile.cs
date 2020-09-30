using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DAT_Tile
{
    public bool occupied;

    public DAT_Tile(bool occupied)
    {
        this.occupied = occupied;
    }
}
