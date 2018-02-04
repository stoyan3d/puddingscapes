using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldModel {

    // The tile width of the world.
    public int Width { get; protected set; }

    // The tile height of the world
    public int Height { get; protected set; }

    public int Turn { get; protected set; }

    public delegate void OnTurnUpdate();
    public OnTurnUpdate onTurnUpdateCallback;

    public delegate void OnCharacterCreated(PlayerModel player);
    public OnCharacterCreated onCharacterCreatedCallback;

    private TileModel[,] tiles;
    //private PlayerModel player;

    public WorldModel(int width, int height)
    {
        Turn = 0;

        Width = width;
        Height = height;

        tiles = new TileModel[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                tiles[x, y] = new TileModel(x, y);
            }
        }
    }

    public TileModel GetTileAt(int x, int y)
    {
        if (x >= Width || x < 0 || y >= Height || y < 0)
        {
            Debug.LogError("Tile ("+x+","+y+") is out of range.");
            return null;
        }

        return tiles[x, y];
    }

    public void AdvanceTurn()
    {
        Turn += 1;

        if (onTurnUpdateCallback != null)
            onTurnUpdateCallback.Invoke();
    }

    public PlayerModel CreateCharacter(TileModel t)
    {
        Debug.Log("CreateCharacter");

        PlayerModel p = new PlayerModel(t);
        //player = p;

        if (onCharacterCreatedCallback != null)
            onCharacterCreatedCallback.Invoke(p);

        return p;
    }
}
