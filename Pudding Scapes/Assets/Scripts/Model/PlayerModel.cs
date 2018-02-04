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
        // We can place the cahracter on the top row in our first turn
        if (WorldModel.Instance.Turn == 0)
        {
            TileModel[] startingTiles = new TileModel[World.Width];

            for (int i = 0; i < World.Width; i++)
            {
                startingTiles[i] = World.GetTileAt(i, World.Height - 1);
            }

            return startingTiles;
        }
        else if (World.Turn > 0)
        {
            return Tile.GetNeighbours();
        }
        else
        {
            Debug.Log("There are no valid moves");
            return null;
        }
    }
}
