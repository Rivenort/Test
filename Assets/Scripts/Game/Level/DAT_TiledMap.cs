using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

/// <summary>
/// @author Rivenort
/// </summary>
[System.Serializable]
public class DAT_TiledMap
{
    public GameObject[] tiles;
    public int width;
    public int height;


    public DAT_TiledMap(int w, int h, GameObject[] tiles)
    {
        this.width = w;
        this.height = h;
        this.tiles = tiles;
    }


    public void SetOccupied(int i, int j, bool occupied)
    {
        if (i < width && j < height)
            tiles[i * width + j].GetComponent<Tile>().SetOccupied(occupied);
    }

    public bool GetOccupied(int i, int j)
    {
        if (i < width && j < height)
            return tiles[i * width + j].GetComponent<Tile>().IsOccupied();
        return false;
    }


    public void Foreach(Action<GameObject> Func)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            Func(tiles[i]);
        }
    }

    public GameObject GetTileObject(int i, int j)
    {
        if (i < width && j < height)
            return tiles[j * width + i];
        return null;
    }

    public Tile GetTile(int i, int j)
    {
        if (i < width && j < height)
            return tiles[i * width + j].GetComponent<Tile>();
        return null;
    }

    public void HighlightOccupied(bool val)
    {
        foreach (GameObject o in tiles)
        {
            o.GetComponent<Tile>().HighlightOccupied(val);
        }
    }
}
