﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents segment which consists of list of TileType
/// </summary>

[System.Serializable]
public class Segment
{
    /// <summary>
    /// This is not segment this is TileTypeList
    /// </summary>
    public List<PoolableType> ListOfTiles;

    public Segment()
    {
        ListOfTiles = new List<PoolableType>();
    }

    public Segment(PoolableType tile)
    {
        ListOfTiles = new List<PoolableType>();
        for (int i = 0; i < 5; i++)
        {
            ListOfTiles.Add(tile);
        }
    }

    public Segment(Segment segment)
    {
        ListOfTiles = new List<PoolableType>();
        for (int i = 0; i < 5; i++)
        {
            ListOfTiles.Add(segment.ListOfTiles[i]);
        }
    }

    public PoolableType this[int i]
    {

        get
        {
            return ListOfTiles[i];
        }

        set
        {
            ListOfTiles[i] = value;
        }
    }

    public int Count
    {
        get
        {
            return ListOfTiles.Count;
        }
    }
}

