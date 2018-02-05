using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour {

    public Sprite playerSprite;

    Dictionary<PlayerModel, GameObject> characterGameObjectMap;
    WorldModel World { get { return WorldController.instance.World; } }

    // Use this for initialization
    void Start () {
        characterGameObjectMap = new Dictionary<PlayerModel, GameObject>();
        World.onPlayerCreatedCallback += OnCharacterCreated;
        World.onCharacterMovedCallback += OnCharacterMoved;
	}
	
    public void OnCharacterCreated(PlayerModel player)
    {
        GameObject charGo = new GameObject();
        characterGameObjectMap.Add(player, charGo);

        // Set up the GameObject
        charGo.name = "Player";
        if (player.Tile != null)
        {
            charGo.transform.position = new Vector3(player.Tile.X, player.Tile.Y, 0);
        }
        else
            charGo.SetActive(false);

        charGo.transform.SetParent(transform, true);

        // Set up the sprite renderer
        SpriteRenderer sr = charGo.AddComponent<SpriteRenderer>();
        sr.sprite = playerSprite;
        sr.sortingLayerName = "Characters";
    }

    public void OnCharacterMoved(PlayerModel player)
    {
        GameObject charGo = characterGameObjectMap[player];
        charGo.SetActive(true);
        charGo.transform.position = new Vector3(player.Tile.X, player.Tile.Y, 0);
    }
}
