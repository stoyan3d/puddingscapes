using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel {

    // The current tile the player is on
    public TileModel Tile { get; protected set; }
    public int Strength { get; protected set; }

    WorldModel World { get { return WorldController.instance.World; } }

    public PlayerModel(TileModel tile, int strength)
    {
        Tile = tile;
        Strength = strength;
        //World.validMoveTiles = GetValidMoveTiles();
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
        TileModel[] validMoves = Tile.GetNeighbours();

        // Any tile that has a stronger enemy than the player will be set as null
        //for (int i = 0; i < validMoves.Length; i++)
        //{
        //    if (validMoves[i] != null)
        //    {
        //        if (validMoves[i].enemy != null)
        //        {
        //            if (validMoves[i].enemy.Strength >= Strength)
        //            {
        //                validMoves[i] = null;
        //            }
        //        }
        //    }
        //}

        return validMoves;
    }

    public bool MoveToTile(TileModel t)
    {
        // Check if the tile we want to move to is a valid move tile
        for (int i = 0; i < World.validMoveTiles.Length; i++)
        {
            if (t == World.validMoveTiles[i])
            {
                if (t.enemy != null)
                {
                    if (t.enemy.Enraged)
                    {
                        // Debug.Log("Attacking Enraged");
                        t.enemy.ReceiveDamage(2);
                        return true;
                    }
                    else
                    {
                        Tile = t;
                        World.KillEnemyOnTile(t);
                        return true;
                    }
                }
                else
                {
                    // Move if the tile is empty
                    Tile = t;
                    return true;
                }
            }
        }

        return false;
    }
}
