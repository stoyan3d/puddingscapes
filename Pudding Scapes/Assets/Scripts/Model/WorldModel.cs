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
    public PlayerModel Player { get; protected set; }

    public bool GameWon { get; protected set; }
    public bool GameLost { get; protected set; }

    public WorldModel(int width, int height)
    {
        Instance = this;

        Turn = -1;

        Width = width;
        Height = height;

        tiles = new TileModel[Width, Height];

        // Populate the base tiles
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                tiles[x, y] = new TileModel(x, y, TileModel.TileType.Floor);
            }
        }

        // Add an exit
        int exitX = Random.Range(0, Width);
        tiles[exitX, 0].Type = TileModel.TileType.Exit;

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
        if (Player != null)
        {
            validMoveTiles = Player.GetValidMoveTiles();

            if (Player.Tile.Type == TileModel.TileType.Exit)
                GameWon = true;
        }

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
                Player = p;

                if (onCharacterCreatedCallback != null)
                    onCharacterCreatedCallback.Invoke(Player);

                AdvanceTurn();

                return Player;
            }
        }
        return null;
    }

    public void MovePlayer(TileModel t)
    {
        // Keep advancing if the game isn't over yet
        if (GameWon || GameLost)
            return;

        Player.MoveToTile(t);

        AdvanceTurn();

        if (onCharacterMovedCallback != null)
            onCharacterMovedCallback.Invoke(Player);
    }
}
