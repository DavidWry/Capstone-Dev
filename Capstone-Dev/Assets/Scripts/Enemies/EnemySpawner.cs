using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnee;
    public int numberOfEnemies;
    public float spawnTime;
    public float spawnDelay;
	
	void Start ()
    {
        InvokeRepeating ("SpawnObject", spawnTime, spawnDelay);
	}

    public void SpawnObject()
    {
        Instantiate (spawnee, transform.position, transform.rotation);
        numberOfEnemies += 1;

        if ( numberOfEnemies >= 5)
            CancelInvoke("SpawnObject");

    }


}
