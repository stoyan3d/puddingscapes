using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel {

    // The current tile the enemy is on
    public TileModel Tile { get; protected set; }
    public int Strength { get; protected set; }
    public int MaxStrength { get; protected set; }
    public bool Enraged { get; protected set; }
    public bool AboutToGrow { get; protected set; }

    public enum EnemyType { Green, Orange, Red, Yellow}
    public EnemyType Type { get; protected set; }

    WorldModel World { get { return WorldController.instance.World; } }

    public EnemyModel(TileModel tile, EnemyType type, int strength, int maxStrength)
    {
        Tile = tile;
        Type = type;
        Strength = strength;
        MaxStrength = maxStrength;

        Enraged = false;

        World.onTurnUpdateCallback += UpdateGrowth;
    }

    // This is called every turn
    public void UpdateGrowth()
    {
        AboutToGrow = CanGrow();
        if (AboutToGrow && Strength < MaxStrength)
        {
            Strength += 1;
        }

        if (Strength == MaxStrength && !Enraged)
        {
            Enraged = true;
        }
            
    }

    public bool CanGrow()
    {
        // If an enemy is touching two other enemies of the same type 
        // we allow it to grow
        int touchCount = 0;
        bool neighbourCanGrow = false;

        TileModel[] neighbours = Tile.GetNeighbours();

        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i] != null)
            {
                if (neighbours[i].enemy != null)
                {
                    if (neighbours[i].enemy.Type == Type)
                    {
                        touchCount++;
                        neighbourCanGrow = neighbours[i].enemy.AboutToGrow;
                    }   
                }
            }
        }

        if (touchCount >= 2 || neighbourCanGrow)
            return true;

        return false;
    }
}
