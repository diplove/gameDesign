using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

    public Transform[] spawnLocations;
    public GameObject[] whatToSpawnPrefab;
    public GameObject[] whatToSpawnClone;

    void Start()
    {
        //InvokeRepeating("spawnAsteroids", 0, 2.0f);
        spawnAsteroids();
    }

    void spawnAsteroids()
    {
        for (int i=0; i<spawnLocations.Length; i++)
        {
            whatToSpawnClone[0] = Instantiate(whatToSpawnPrefab[0], spawnLocations[i].transform.position, Quaternion.identity) as GameObject;
        }
    }

    void Update()
    {

    }
}
