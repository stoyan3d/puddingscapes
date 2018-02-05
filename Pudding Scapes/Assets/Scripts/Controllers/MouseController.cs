using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    WorldModel World { get { return WorldController.instance.World; } }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        UpdatePlayerMovement();
	}

    void UpdatePlayerMovement()
    {
        if (Input.GetMouseButtonUp(0))
        {
            TileModel currTile = GetMouseOverTile();
            // Spawn the player in our frist turn
            if (World.player == null)
            {
                World.CreateCharacter(currTile);
            }
            else if (World.player.Tile != currTile)
            {
                World.MovePlayer(currTile);
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
