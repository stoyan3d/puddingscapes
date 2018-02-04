﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    PlayerModel player;

	// Use this for initialization
	void Start () {
        player = WorldController.instance.World.player;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdatePlayerMovement()
    {
        if (Input.GetMouseButtonUp(0))
        {
            TileModel currTile = GetMouseOverTile();
            if (player.Tile != currTile)
            {
                player.MoveToTile(currTile);
            }
        }
    }

    public TileModel GetMouseOverTile()
    {
        Vector3 currentMousePosition = GetGroundPosFromScreen(Input.mousePosition);
        return WorldController.instance.World.GetTileAt((int)currentMousePosition.x, (int)currentMousePosition.y);
    }

    private Vector3 GetGroundPosFromScreen(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        float d;
        Plane groundPlane = new Plane(Vector3.up, 0);
        groundPlane.Raycast(ray, out d);
        return ray.GetPoint(d);
    }
}