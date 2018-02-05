using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel {

    // The current tile the player is on
    public TileModel Tile { get; protected set; }

    WorldModel World { get { return WorldController.instance.World; } }

    public PlayerModel(TileModel tile)
    {
        Tile = tile;
        World.validMoveTiles = GetValidMoveTiles();
    }

    public bool IsPlacementValid(TileModel t)
    {
        // We can only place a character on the top row
        // TODO: We should expose this to the editor so we can have more control
        if (t.Y < 1)
        {
            return true;
        }
        else
            return false;
    }

    public TileModel[] GetValidMoveTiles()
    {
        if (World.Turn > 0)
        {
            return Tile.GetNeighbours();
        }
        else
        {
            Debug.Log("There are no valid moves");
            return null;
        }
    }

    public void MoveToTile(TileModel t)
    {
        Debug.Log("Player moved");
        for (int i = 0; i < World.validMoveTiles.Length; i++)
        {
            if (t == World.validMoveTiles[i])
            {
                Tile = t;
            }
        }
    }
}
