using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour {

    public Sprite[] enemySprites;
    public GameObject destroyVFX;

    Dictionary<EnemyModel, GameObject> characterGameObjectMap;
    WorldModel World { get { return WorldController.instance.World; } }
    private ParticleSystem vfx;

    // Use this for initialization
    void Start () {
        characterGameObjectMap = new Dictionary<EnemyModel, GameObject>();
        World.onEnemyCreatedCallback += OnEnemyCreated;
        World.onEnemyKilledCallback += OnEnemyKilled;
        vfx = Instantiate(destroyVFX).GetComponent<ParticleSystem>();
        vfx.gameObject.SetActive(false);
	}
	
    public void OnEnemyCreated(EnemyModel enemy)
    {
        Debug.Log("OnEnemyCreated");
        GameObject charGo = new GameObject();
        characterGameObjectMap.Add(enemy, charGo);

        // Set up the GameObject
        charGo.name = "Enemy_" + enemy.Type;
        if (enemy.Tile != null)
        {
            charGo.transform.position = new Vector3(enemy.Tile.X, enemy.Tile.Y, 0);
        }

        charGo.transform.SetParent(transform, true);

        // Set up the sprite renderer
        SpriteRenderer sr = charGo.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Characters";

        sr.sprite = GetSprite(enemy.Type.ToString(), "1");
    }

    public void OnEnemyKilled(EnemyModel enemy)
    {
        vfx.transform.position = new Vector3((float)enemy.Tile.X + 0.5f, (float)enemy.Tile.Y + 0.5f, -1);
        vfx.gameObject.SetActive(true);
        vfx.Play();
        Destroy(characterGameObjectMap[enemy]);
        characterGameObjectMap.Remove(enemy);
    }

    private Sprite GetSprite(string type, string mode)
    {
        string spriteName = "Pudding_" + type + "_" + mode;
        for (int i = 0; i < enemySprites.Length; i++)
        {
            if (enemySprites[i].name == spriteName)
                return enemySprites[i];
        }

        Debug.LogWarning("Could not find sprite: " + spriteName);
        return null;
    }
}
