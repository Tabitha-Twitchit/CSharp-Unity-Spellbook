using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private Vector3 spawnPos = new Vector3(25, 0.1f , 0);
    private PlayerController playerControllerScript;
    public float time = 0;
    public float spawnTime;


    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        SetRandomTimer();
        
        //InvokeRepeating("Spawner", 2.0f, 2.0f);
        //Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
    }
    void Update()
    {
        time += Time.deltaTime;
        if(time >= spawnTime)
        {
            Spawner();
            time = 0;
            SetRandomTimer();
        }
    }

    void Spawner()
    {
        if (playerControllerScript.gameOff == false)
        {
            int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
            Instantiate(obstaclePrefabs[obstacleIndex], spawnPos, obstaclePrefabs[obstacleIndex].transform.rotation);
        }
    }

    void SetRandomTimer()
    {
        spawnTime = Random.Range(1.0f, 3.0f);
    }
}
