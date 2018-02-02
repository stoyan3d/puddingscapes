using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

	public GameObject[] tile;
    List<GameObject> tilebank = new List<GameObject>();

	static int rows = 6;
	static int cols = 4;

    GameObject tile1 = null;
    GameObject tile2 = null;

    TileSlot[,] tiles = new TileSlot[cols, rows];

	// Use this for initialization
	void Start () {
		if (tile == null)
		{
			Debug.LogWarning("You haven't assigned a base tile!");
			return;
		}

		for (int r = 0; r < rows; r++)
		{
            for (int c = 0; c < cols; c++)
            {
                Vector3 position = new Vector3(r - rows / 2, c - cols / 2, 0);
            }
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
