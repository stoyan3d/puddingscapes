using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

    public int worldWidth = 6;
    public int worldHeigh = 4;

    public WorldModel World { get; protected set; }
    public static WorldController instance;

	// Use this for initialization
	void OnEnable () {
        if (instance != null)
            Debug.LogError("There should be only one instance of world controller");

        instance = this;

        // Create the world
        World = new WorldModel(worldWidth, worldHeigh);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
