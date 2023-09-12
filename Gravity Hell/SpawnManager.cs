using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject[] powerupPrefab;
    public GameObject boss;
    
    private float spawnRangeX = 9;
    private float spawnRangeZ = 9;
    
    public int enemyCount;
    public int waveNumber = 1;
    public int bossWave = 4;

    void Start()
    {
        //Calls the first wave of both enemies and powerups. enemy wave has the initial parameter passed in.
        SpawnEnemyWave(waveNumber);
        SpawnPowerupWave();
    }

    void Update()
    {
        /*assigns the enemycount variable as the number of gameobjects tagged "Enemy" at any given time, and
         * if that count drops to 0, increments the waveNumber, Calls the enemywave spawner function(with the 
         * waveNumber passed in as a parameter. And Calls the powerup wave function. You could also do this
         by obj name, tho this is said to be less performant, i.e. FindObjectsOfType<Enemy>().Length;*/
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            SpawnPowerupWave();
        }

    }

    /*The enemy spawner, a custom function that accepts integers as a parameter (hence the waveNumber from
     * start and update methods.) I don't understand the literal syntax of the "for" statement, but the net
     * result is that it runs the function following the if statement the number of times that the integer 
     * (enemiesToSpawn) passed into it, tells it to.
     */
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        //syntax breakdown: for (what happens before the loop starts; the condition that will continue it; what happens after)
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            /*note the nested custom function GenerateSpawnPos is acting as both a parameter and method here
            because it was not set as void, meaning GenerateSpawnPos can return values into instantiate that
            will be unique to each call of instantiate.*/
            int enemyIndex = Random.Range(0, enemyPrefab.Length);
            Instantiate(enemyPrefab[enemyIndex], GenerateSpawnPos(), enemyPrefab[0].transform.rotation);
        }
        //if the remainder of dividing the waveNumber by bossWave is 0, then spawn a boss. This allows a boss
        //to occur with whatever frequency the player chooses--defaults to every 4th level.
        if (waveNumber % bossWave == 0)
        {
            Instantiate(boss, GenerateSpawnPos(), boss.transform.rotation);
        }
    }

    //the powerupspawner uses the same random position method as the enemyspawner
    void SpawnPowerupWave()
    {
        int powerupIndex = Random.Range(0, powerupPrefab.Length);
        Instantiate(powerupPrefab[powerupIndex], GenerateSpawnPos(), powerupPrefab[0].transform.rotation);
    }

    /*because this custom function is typed (not sure if the most accurate term) as vector3, rather than void
     it can return a vector3 parameter back into other methods that call this function.*/

    private Vector3 GenerateSpawnPos()
    {
        float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX);
        float spawnPosZ = Random.Range(-spawnRangeZ, spawnRangeZ);
     
        //this assigns a vector3 variable/parameter that synthesizes the two random ranges above. 
        Vector3 randomPos = new Vector3(spawnPosX, 0.3f, spawnPosZ);
        
        //this statement is necessary to convey the variable fed back.
        return randomPos;
    }
}
