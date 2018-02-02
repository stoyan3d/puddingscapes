using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject baseTile;
	public int tilesX = 6;
	public int tilesY = 4;

	// Use this for initialization
	void Start () {
		if (baseTile == null)
		{
			Debug.LogWarning("You haven't assigned a base tile!");
			return;
		}

		for (int x = 0; x <= tilesX; x++)
		{
			Vector3 position = new Vector3(x - tilesX/2, 0, 0);
			Instantiate(baseTile, position, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
