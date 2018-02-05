using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerView : MonoBehaviour {

    public Sprite playerSprite;
    public float moveDuration = 1f;

    Dictionary<PlayerModel, GameObject> characterGameObjectMap;
    WorldModel World { get { return WorldController.instance.World; } }

    // Use this for initialization
    void Start () {
        characterGameObjectMap = new Dictionary<PlayerModel, GameObject>();
        World.onPlayerCreatedCallback += OnCharacterCreated;
        World.onCharacterMovedCallback += OnCharacterMoved;

        DOTween.Init();
	}
	
    public void OnCharacterCreated(PlayerModel player)
    {
        GameObject charGo = new GameObject();
        characterGameObjectMap.Add(player, charGo);

        // Set up the GameObject
        charGo.name = "Player";
        if (player.Tile != null)
        {
            charGo.transform.position = new Vector3(player.Tile.X + 0.5f, player.Tile.Y + 0.5f, 0);
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
        //Vector3 currentPos = charGo.transform.position;
        Vector3 targetPos = new Vector3(player.Tile.X + 0.5f, player.Tile.Y + 0.5f, 0);
        charGo.transform.DOMove(targetPos, moveDuration);
        charGo.transform.DOScale(1.2f, moveDuration * 0.5f).SetLoops(2, LoopType.Yoyo);
    }
}
