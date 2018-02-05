using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour {

    public Sprite[] enemySprites;
    public GameObject destroyVFX;

    Dictionary<EnemyModel, SpriteRenderer> characterGameObjectMap;
    WorldModel World { get { return WorldController.instance.World; } }
    private ParticleSystem vfx;

    // Use this for initialization
    void Start () {
        characterGameObjectMap = new Dictionary<EnemyModel, SpriteRenderer>();
        World.onEnemyCreatedCallback += OnEnemyCreated;
        World.onEnemyKilledCallback += OnEnemyKilled;
        World.onTurnUpdateCallback += UpdateGrowthSprites;
        vfx = Instantiate(destroyVFX).GetComponent<ParticleSystem>();
        vfx.gameObject.SetActive(false);
	}
	
    private void OnEnemyCreated(EnemyModel enemy)
    {
        GameObject charGo = new GameObject();

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
        characterGameObjectMap.Add(enemy, sr);
    }

    private void OnEnemyKilled(EnemyModel enemy)
    {
        PlayVFX(enemy);
        Destroy(characterGameObjectMap[enemy]);
        characterGameObjectMap.Remove(enemy);
    }

    private void UpdateGrowthSprites()
    {
        foreach (var enemy in characterGameObjectMap)
        {
            if (enemy.Key.Enraged)
            {
                enemy.Value.sprite = GetSprite(enemy.Key.Type.ToString(), "2");
            }
            else
            {
                enemy.Value.sprite = GetSprite(enemy.Key.Type.ToString(), "1");
            }
        }
    }

    private Sprite GetSprite(string type, string mode)
    {
        string spriteName = "Enemy_" + type + "_" + mode;
        for (int i = 0; i < enemySprites.Length; i++)
        {
            if (enemySprites[i].name == spriteName)
                return enemySprites[i];
        }

        Debug.LogWarning("Could not find sprite: " + spriteName);
        return null;
    }

    private void PlayVFX(EnemyModel enemy)
    {
        vfx.transform.position = new Vector3((float)enemy.Tile.X + 0.5f, (float)enemy.Tile.Y + 0.5f, -1);
        vfx.gameObject.SetActive(true);
        vfx.Play();
    }
}
