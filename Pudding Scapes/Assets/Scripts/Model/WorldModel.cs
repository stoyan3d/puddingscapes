using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldModel {

    // The tile width of the world.
    public int Width { get; protected set; }

    // The tile height of the world
    public int Height { get; protected set; }

    public int Turn { get; protected set; }

    public TileModel[] validMoveTiles;

    public static WorldModel Instance { get; protected set; }

    public delegate void OnTurnUpdate();
    public OnTurnUpdate onTurnUpdateCallback;

    public delegate void OnCharacterCreated(PlayerModel player);
    public OnCharacterCreated onCharacterCreatedCallback;

    public delegate void OnCharacterMoved(PlayerModel player);
    public OnCharacterMoved onCharacterMovedCallback;

    private TileModel[,] tiles;
    public PlayerModel player { get; protected set; }

    public WorldModel(int width, int height)
    {
        Instance = this;

        Turn = -1;

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

        validMoveTiles = GetValidPlayerSpawnTiles();
    }

    public TileModel GetTileAt(int x, int y)
    {
        if (x >= Width || x < 0 || y >= Height || y < 0)
        {
            //Debug.LogError("Tile ("+x+","+y+") is out of range.");
            return null;
        }

        return tiles[x, y];
    }

    public TileModel[] GetValidPlayerSpawnTiles()
    {
        // We can place the cahracter on the top row in our first turn
        TileModel[] startingTiles = new TileModel[Width];

        for (int i = 0; i < Width; i++)
        {
            startingTiles[i] = GetTileAt(i, Height - 1);
        }

        return startingTiles;
    }

    public void AdvanceTurn()
    {
        Turn += 1;

        // Update our valid move tiles
        if (player != null)
            validMoveTiles = player.GetValidMoveTiles();

        if (onTurnUpdateCallback != null)
            onTurnUpdateCallback.Invoke();
    }

    public PlayerModel CreateCharacter(TileModel t)
    {
        Debug.Log("CreateCharacter");
        for (int i = 0; i < validMoveTiles.Length; i++)
        {
            if (t == validMoveTiles[i])
            {
                PlayerModel p = new PlayerModel(t);
                player = p;

                if (onCharacterCreatedCallback != null)
                    onCharacterCreatedCallback.Invoke(p);

                AdvanceTurn();

                return p;
            }
        }
        return null;
    }

    public void MoveCharacter()
    {

    }
}
