using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel {

    // The current tile the player is on
    public TileModel Tile { get; protected set; }

    public PlayerModel(TileModel tile)
    {
        Tile = tile;
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
}
