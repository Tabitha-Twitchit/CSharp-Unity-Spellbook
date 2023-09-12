using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandoSpawner : MonoBehaviour
{
    /// <summary>
    /// Spawns a random array of gameobjects, at random times, in random places with parameters for each.
    /// Note each prefab needs a destroyer for cleanup
    /// </summary>
    
    //An array of things to be spawned
    public GameObject[] ballPrefabs;

    //Limits where they can spawn
    private float spawnLimitXLeft = -22;
    private float spawnLimitXRight = 7;
    private float spawnPosY = 30;

    //variables for a timer to track time and the point in time they spawn
    private float time = 0;
    private float spawnTime;
    
    
    //the initial time for first spawn
    void Start()
    {
        SetRandomTimer();
    }

    
    //increments time upward and checks if the time has reached our randomly specified time limit. Once reached
    //we call the random ball spawning method. Once that's run, we reset the timer to zero, and set a new random 
    //time for spawning the next item.
    private void Update()
    {
        time += Time.deltaTime;

        if(time >= spawnTime)
        {
            SpawnRandomBall();
            time = 0;
            SetRandomTimer();
        }
    }
    // Spawn random ball at random x position at top of play area. These parameters can be adjusted for different dimensions
    //or length of array.
    void SpawnRandomBall()
    {
        // Generate random ball index and random spawn position
        int ballIndex = Random.Range(0, ballPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(spawnLimitXLeft, spawnLimitXRight), spawnPosY, 0);

        // instantiate ball at random spawn location
        Instantiate(ballPrefabs[ballIndex], spawnPos, ballPrefabs[0].transform.rotation);
    }

    //method that selects a time for spawn from among a range.
    void SetRandomTimer()
    {
        spawnTime = Random.Range(0, 5);
    }
}
