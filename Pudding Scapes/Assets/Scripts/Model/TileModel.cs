using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileModel
{
    public enum TileType { Empty, Floor };

    public TileType Type { get; protected set; }
    public int X { get; protected set; }
    public int Y { get; protected set; }

    public TileModel(int x, int y)
    {
        X = x;
        Y = y;
    }
}
