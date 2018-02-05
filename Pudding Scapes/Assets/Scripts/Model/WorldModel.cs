using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldModel {

    // The tile width of the world.
    public int Width { get; protected set; }

    // The tile height of the world
    public int Height { get; protected set; }

    public int Turns { get; protected set; }

    public TileModel[] validMoveTiles;

    public static WorldModel Instance { get; protected set; }

    public delegate void OnTurnUpdate();
    public OnTurnUpdate onTurnUpdateCallback;

    public delegate void OnCharacterCreated(PlayerModel player);
    public OnCharacterCreated onPlayerCreatedCallback;

    public delegate void OnEnemyCreated(EnemyModel enemy);
    public OnEnemyCreated onEnemyCreatedCallback;

    public delegate void OnCharacterMoved(PlayerModel player);
    public OnCharacterMoved onCharacterMovedCallback;

    public delegate void OnEnemyKilled(EnemyModel enemy);
    public OnEnemyKilled onEnemyKilledCallback;

    private TileModel[,] tiles;
    public PlayerModel Player { get; protected set; }
    public List<EnemyModel> Enemies { get; protected set; }

    public bool GameWon { get; protected set; }
    public bool GameLost { get; protected set; }

    public WorldModel(int width, int height, int turns)
    {
        Instance = this;

        Turns = -1;

        Width = width;
        Height = height;
        Turns = turns;

        Enemies = new List<EnemyModel>();

        tiles = new TileModel[Width, Height];
        int exitX = Random.Range(0, Width);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                // Populate the base tiles
                tiles[x, y] = new TileModel(x, y, TileModel.TileType.Floor);

                // Add an exit
                if (tiles[exitX, 0] != null)
                    tiles[exitX, 0].Type = TileModel.TileType.Exit;
            }
        }

        validMoveTiles = GetValidPlayerSpawnTiles();
    }

    public void CreateEnemies()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                // Add the enemies
                if (tiles[x, y].Type == TileModel.TileType.Floor)
                {
                    // Get a random enemy type from our enum
                    EnemyModel.EnemyType typeRand;
                    int enemyTypesAmount = System.Enum.GetValues(typeof(EnemyModel.EnemyType)).Length;
                    typeRand = (EnemyModel.EnemyType)Random.Range(0, enemyTypesAmount);

                    CreateEnemy(tiles[x, y], typeRand);
                }
            }
        }
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
        // Update our valid move tiles
        if (Player != null)
        {
            Turns -= 1;
            validMoveTiles = Player.GetValidMoveTiles();

            if (Player.Tile.Type == TileModel.TileType.Exit)
                GameWon = true;

            if (Turns == 0 && GameWon == false)
                GameLost = true;
        }

        if (onTurnUpdateCallback != null)
            onTurnUpdateCallback.Invoke();
    }

    public PlayerModel CreateCharacter(TileModel tile)
    {
        Debug.Log("CreateCharacter");
        for (int i = 0; i < validMoveTiles.Length; i++)
        {
            if (tile == validMoveTiles[i])
            {
                // TODO: Expose the player strength in the editor
                PlayerModel p = new PlayerModel(tile, 3);
                Player = p;

                KillEnemyOnTile(tile);

                if (onPlayerCreatedCallback != null)
                    onPlayerCreatedCallback.Invoke(Player);

                AdvanceTurn();

                return Player;
            }
        }
        return null;
    }

    public EnemyModel CreateEnemy(TileModel tile, EnemyModel.EnemyType type)
    {
        EnemyModel enemy = new EnemyModel(tile, type, 1 ,3);

        tile.enemy = enemy;

        Enemies.Add(enemy);

        if (onEnemyCreatedCallback != null)
            onEnemyCreatedCallback.Invoke(enemy);

        return enemy;
    }

    public void KillEnemyOnTile(TileModel tile)
    {
        Enemies.Remove(tile.enemy);
        if (onEnemyKilledCallback != null)
            onEnemyKilledCallback.Invoke(tile.enemy);

        tile.enemy = null;
    }

    public void MovePlayer(TileModel t)
    {
        // Keep advancing if the game isn't over yet
        if (GameWon || GameLost)
            return;

        if (Player.MoveToTile(t))
        {
            AdvanceTurn();

            if (onCharacterMovedCallback != null)
                onCharacterMovedCallback.Invoke(Player);
        } 
    }
}
