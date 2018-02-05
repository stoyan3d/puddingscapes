using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour {

    public Sprite baseTile;
    public Sprite selectorTile;
    public Sprite exitTile;

    Dictionary<TileModel, GameObject> tileGameObjectMap;
    List<GameObject> selectorPool;

    WorldModel World { get { return WorldController.instance.World; } }

	// Use this for initialization
	void Start () {
        // Instantiate dictionary that tracks which GameObject is rendering which tile
        tileGameObjectMap = new Dictionary<TileModel, GameObject>();
        selectorPool = new List<GameObject>();

        // Create a GameObject for each Tile
        for (int x = 0; x < World.Width; x++)
        {
            // Create a pool of selectors
            GameObject selectorGo = new GameObject();
            selectorGo.name = "Selector";
            selectorGo.transform.SetParent(transform);
            SpriteRenderer selectorSprite = selectorGo.AddComponent<SpriteRenderer>();
            selectorSprite.sortingLayerName = "Selectors";
            selectorSprite.sprite = selectorTile;
            selectorGo.SetActive(false);
            selectorPool.Add(selectorGo);

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
                sr.sortingLayerName = "Tiles";

                if (tileData.Type == TileModel.TileType.Floor)
                {
                    sr.sprite = baseTile;
                    if (x%2 == 0 ^ y%2 == 0)
                    {
                        sr.color = new Color(0.75f, 0.95f, 0.95f);
                    }
                }
                    

                if (tileData.Type == TileModel.TileType.Exit)
                    sr.sprite = exitTile;
            }
        }

        World.onTurnUpdateCallback += VisualizeAvailableTiles;
    }


    // Not sure if this is efficient
    private void VisualizeAvailableTiles()
    {
        ResetSelectors();
        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                TileModel tileData = World.GetTileAt(x, y);
                //SpriteRenderer sr = tileGameObjectMap[tileData].GetComponent<SpriteRenderer>();
                //sr.color = Color.white;
                for (int i = 0; i < World.validMoveTiles.Length; i++)
                {
                    if (World.validMoveTiles[i] != null)
                    {
                        if (tileData == World.validMoveTiles[i])
                        {
                            GameObject selector = GetSelector();
                            selector.transform.position = new Vector3(tileData.X, tileData.Y, 0);
                        }
                    }
                }
            }
        }
    }

    private GameObject GetSelector()
    {
        for (int i = 0; i < selectorPool.Count; i++)
        {
            if (selectorPool[i].activeSelf == false)
            {
                selectorPool[i].SetActive(true);
                return selectorPool[i];
            }
                
        }
        return null;
    }

    private void ResetSelectors()
    {
        for (int i = 0; i < selectorPool.Count; i++)
        {
            selectorPool[i].SetActive(false);
        }
    }
}
