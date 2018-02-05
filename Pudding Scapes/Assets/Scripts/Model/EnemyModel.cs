using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel {

    // The current tile the enemy is on
    public TileModel Tile { get; protected set; }

    public enum EnemyType { Green, Orange, Red, Yellow}
    public EnemyType Type { get; protected set; }

    WorldModel World { get { return WorldController.instance.World; } }

    public EnemyModel(TileModel tile, EnemyType type)
    {
        Tile = tile;
        Type = type;
    }
}
