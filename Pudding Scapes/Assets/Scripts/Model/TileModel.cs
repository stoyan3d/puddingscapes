using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileModel
{
    public enum TileType { Empty, Floor, Exit };

    public TileType Type;
    public int X { get; protected set; }
    public int Y { get; protected set; }

    public EnemyModel enemy;

    public TileModel(int x, int y, TileType type)
    {
        X = x;
        Y = y;
        Type = type;
    }

    public TileModel[] GetNeighbours()
    {
        TileModel[] neighbours;
        neighbours = new TileModel[4];

        // Neighbours can be null so we need to check for that
        neighbours[0] = WorldModel.Instance.GetTileAt(X, Y + 1); // north
        neighbours[1] = WorldModel.Instance.GetTileAt(X, Y - 1); // south
        neighbours[2] = WorldModel.Instance.GetTileAt(X + 1, Y); // west
        neighbours[3] = WorldModel.Instance.GetTileAt(X - 1, Y); // east

        return neighbours;
    }
}
