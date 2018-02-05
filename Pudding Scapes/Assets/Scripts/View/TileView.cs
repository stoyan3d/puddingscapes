using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour {

    public Sprite baseTile;

    Dictionary<TileModel, GameObject> tileGameObjectMap;

    WorldModel World { get { return WorldController.instance.World; } }

	// Use this for initialization
	void Start () {
        // Instantiate dictionary that tracks which GameObject is rendering which tile
        tileGameObjectMap = new Dictionary<TileModel, GameObject>();

        // Create a GameObject for each Tile
        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                TileModel tileData = World.GetTileAt(x, y);
                GameObject tileGo = new GameObject();
                tileGameObjectMap.Add(tileData, tileGo);

                // Set up the GameObject
                tileGo.name = "Tile_" + x + "_" + y;
                tileGo.transform.position = new Vector3(tileData.X, tileData.Y, 0);
                tileGo.transform.SetParent(transform, true);

                // Set up the sprite renderer
                SpriteRenderer sr = tileGo.AddComponent<SpriteRenderer>();
                sr.sprite = baseTile;
                sr.sortingLayerName = "Tiles";
            }
        }

        World.onTurnUpdateCallback += VisualizeAvailableTiles;
    }


    // Not sure if this is efficient
    private void VisualizeAvailableTiles()
    {
        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                TileModel tileData = World.GetTileAt(x, y);
                SpriteRenderer sr = tileGameObjectMap[tileData].GetComponent<SpriteRenderer>();
                sr.color = Color.white;
                for (int i = 0; i < World.validMoveTiles.Length; i++)
                {
                    if (World.validMoveTiles[i] != null)
                    {
                        if (tileData == World.validMoveTiles[i])
                        {
                            sr.color = Color.green;
                        }
                    }
                }
            }
        }
    }
}
