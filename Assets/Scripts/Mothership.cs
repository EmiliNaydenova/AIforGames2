using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mothership : MonoBehaviour {

    public GameObject enemy;
    public int numberOfEnemies = 20;

    public GameObject spawnLocation;

    public List<GameObject> drones = new List<GameObject>();
    public List<GameObject> scouts = new List<GameObject>();

    public int maxScouts = 4;
    // initialise the boids
    void Start() {

        for (int i = 0; i < numberOfEnemies; i++)
        {

            Vector3 spawnPosition = spawnLocation.transform.position;

            spawnPosition.x = spawnPosition.x + Random.Range(-50, 50);
            spawnPosition.y = spawnPosition.y + Random.Range(-50, 50);
            spawnPosition.z = spawnPosition.z + Random.Range(-50, 50);

            //Instantiate(enemy, spawnPosition, spawnLocation.transform.rotation);

            GameObject thisEnemy = Instantiate(enemy, spawnPosition, spawnLocation.transform.rotation) as GameObject;
            drones.Add(thisEnemy);
        }
    }

    //Introducing a timer
    public float scoutSpawnInterval = 2f;
    private float scoutSpawnTimer = 0f;
    // Update is called once per frame
    void Update()
    {
        scoutSpawnTimer += Time.deltaTime;
        if (scouts.Count < maxScouts && scoutSpawnTimer >= scoutSpawnInterval)
        {
            scoutSpawnTimer = 0f;
            scouts.Add(drones[0]);
            drones.Remove(drones[0]);

            scouts[scouts.Count - 1].GetComponent<Drone>().droneBehaviour = Drone.DroneBehaviours.Scouting;
        }

    }
}

