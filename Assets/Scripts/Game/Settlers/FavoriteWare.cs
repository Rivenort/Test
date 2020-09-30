using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FavoriteWare
{
    public DAT_Ware.Type ware;
    public int mood;
    public bool active;

    public FavoriteWare(DAT_Ware.Type ware, int mood, bool active)
    {
        this.ware = ware;
        this.mood = mood;
        this.active = active;
    }
    public FavoriteWare() { }
}