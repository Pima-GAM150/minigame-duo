using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject prefab;
    public List<GameObject> enemies;
    public int numberOfEnemies;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        checkForNulls();
        if(enemies.Count < numberOfEnemies)
        {
            spawnObjects();
        }

	}

    public void spawnObjects()
    {
        Transform location = this.transform;
        for (int i = enemies.Count; i < numberOfEnemies; i++)
        {
            float ang = Random.value * 360;
            float radius = 20f;
            Vector2 pos;
            pos.x = location.position.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = location.position.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            instaniateEnemy(prefab, pos);
        }
    }

    public void instaniateEnemy(GameObject prefab, Vector2 location)
    {
        GameObject newEnemy = (GameObject)Instantiate(prefab, location, Quaternion.identity);
        enemies.Add(newEnemy);
    }

    public void checkForNulls()
    {
        foreach (GameObject obj in enemies)
        {
            if (obj == null)
            {
                enemies.Remove(obj);
            }
        }
    }


}
