﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

    public int worldWidth = 6;
    public int worldHeigh = 4;
    public int turns = 10;

    public WorldModel World { get; protected set; }
    public static WorldController instance;

	// Use this for initialization
	void OnEnable () {
        if (instance != null)
        {
            Debug.LogError("There should be only one instance of world controller");
            return;
        }  

        instance = this;

        // Create the world
        World = new WorldModel(worldWidth, worldHeigh, turns);
	}

    private void Start()
    {
        // The first move to get us started
        World.AdvanceTurn();

        World.CreateEnemies();
    }
}
