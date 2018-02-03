﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public float transitionTime = 1f;
    public GameObject baseTile;
    public GameObject player;
    public GameObject[] tile;
    List<GameObject> tileBank = new List<GameObject>();

    private static int rows = 4;
    private static int cols = 6;

    private GameObject tile1 = null;
    private GameObject tile2 = null;

    private TileSlot[,] tiles = new TileSlot[cols, rows];
    private IEnumerator coroutine;
    private bool renewBoard;
    
    // Use this for initialization
    void Start()
    {
        // Simple pooling system
        int numCopies = (rows * cols) / 3;
        for (int i = 0; i < numCopies; i++)
        {
            for (int j = 0; j < tile.Length; j++)
            {
                GameObject go = Instantiate(tile[j], new Vector3(-10, -10, 0), tile[j].transform.rotation);
                go.SetActive(false);
                tileBank.Add(go);
            }
        }

        ShuffleList();

        // Initialize our grid
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Vector3 position = new Vector3(c, r, 0);
                // spawn the background tiles
                Instantiate(baseTile, position, baseTile.transform.rotation);

                // spawn the creatures
                for (int n = 0; n < tileBank.Count; n++)
                {
                    GameObject go = tileBank[n];
                    if (!go.activeSelf)
                    {
                        go.transform.position = position;
                        go.SetActive(true);
                        tiles[c, r] = new TileSlot(go, go.name);
                        n = tileBank.Count + 1;
                    }
                }
            }
        }
    }

    void ShuffleList()
    {
        System.Random rand = new System.Random();
        int r = tileBank.Count;
        while (r > 1)
        {
            r--;
            int n = rand.Next(r + 1);
            GameObject val = tileBank[n];
            tileBank[n] = tileBank[r];
            tileBank[r] = val;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrid();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 1000);

            if (hit)
            {
                tile1 = hit.collider.gameObject;
                //Debug.Log("We hit tile " + tile1.name);
            }
        }

        // if finger is detected after an initial tile has been chosen
        else if (Input.GetMouseButtonUp(0) && tile1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 1000);

            if (hit)
            {
                //Debug.Log("We let go on tile " + tile1.name);
                tile2 = hit.collider.gameObject;
            }

            if (tile1 && tile2)
            {
                Vector3 tile1Pos = tile1.transform.position;
                Vector3 tile2Pos = tile2.transform.position;

                int distX = (int)Mathf.Abs(tile1Pos.x - tile2Pos.x);
                int distY = (int)Mathf.Abs(tile1Pos.y - tile2Pos.y);

                // if tiles are not adjacent return
                if (distX == 1 ^ distY == 1)
                {
                    // swap the tiles in our matrix as well
                    TileSlot tempTile = tiles[(int)tile1Pos.x, (int)tile1Pos.y];
                    tiles[(int)tile1Pos.x, (int)tile1Pos.y] = tiles[(int)tile2Pos.x, (int)tile2Pos.y];
                    tiles[(int)tile2Pos.x, (int)tile2Pos.y] = tempTile;

                    coroutine = SwapTiles(tile1, tile2, transitionTime);
                    StartCoroutine(coroutine);

                    // reset the touched tiles
                    tile1 = null;
                    tile2 = null;
                }
                else
                {
                    Debug.Log("Tiles are not adjacent");
                }
                
            }
        }
    }

    IEnumerator SwapTiles(GameObject tile1, GameObject tile2, float transitionTime)
    {
        Vector3 tile2Target = tile1.transform.position;
        Vector3 tile1Target = tile2.transform.position;
        while (true)
        {
            tile1.transform.position = Vector3.Lerp(tile1.transform.position, tile1Target, transitionTime * Time.deltaTime);
            tile2.transform.position = Vector3.Lerp(tile2.transform.position, tile2Target, transitionTime * Time.deltaTime);
            yield return null;
        }
    }

    void CheckGrid()
    {
        int counter = 1;
        // check in columns
        for (int r = 0; r < rows; r++)
        {
            counter = 1;
            for (int c = 1; c < cols; c++)
            {
                if(tiles[c, r] != null && tiles[c - 1, r] != null)
                {
                    // if the tiles exist
                    if (tiles[c, r].type == tiles[c - 1, r].type)
                    {
                        counter++;
                    }
                    else
                        counter = 1;
                    if(counter == 3)
                    {
                        if (tiles[c, r] != null)
                            tiles[c, r].tileObj.SetActive(false);
                        if (tiles[c - 1, r] != null)
                            tiles[c - 1, r].tileObj.SetActive(false);
                        if (tiles[c - 2, r] != null)
                            tiles[c - 2, r].tileObj.SetActive(false);
                        tiles[c, r] = null;
                        tiles[c - 1, r] = null;
                        tiles[c - 2, r] = null;
                        renewBoard = true;
                    }
                }
            }
        }


        // check in columns
        for (int c = 0; c < cols; c++)
        {
            counter = 1;
            for (int r = 1; r < rows; r++)
            {
                if (tiles[c, r] != null && tiles[c, r - 1] != null)
                {
                    // if the tiles exist
                    if (tiles[c, r].type == tiles[c, r - 1].type)
                    {
                        counter++;
                    }
                    else
                        counter = 1;
                    if (counter == 3)
                    {
                        if (tiles[c, r] != null)
                            tiles[c, r].tileObj.SetActive(false);
                        if (tiles[c, r - 1] != null)
                            tiles[c, r - 1].tileObj.SetActive(false);
                        if (tiles[c, r - 2] != null)
                            tiles[c, r - 2].tileObj.SetActive(false);
                        tiles[c, r] = null;
                        tiles[c, r - 1] = null;
                        tiles[c, r - 2] = null;
                        renewBoard = true;
                    }
                }
            }
        }

        if (renewBoard)
        {
            //RenewGrid();
            renewBoard = false;
        }
    }
}
