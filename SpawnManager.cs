using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    
    //an array of gameobjects to spawn from, and a range of distances along 2 axes they will spawn
    public GameObject[] animalPrefabs;
    
    //this is jank, but instead of using coordinates I named these based on which form the spawn line
    //and which corresponded to a fixed point on different axes so they could be usable in the different
    //spawn areas around the field. You'll see this below when they're used in different x, y, z fields.
    private float spawnRangeLine = 10;
    private float spawnRangePoint = 20;
    
    //an int used to randomize which of the 3 areas they should spawn in.
    private int whereToSpawn;

    //does another custom method starting at 2 seconds, then repeats every 1.5 sec
    void Start()
    {
        InvokeRepeating("SpawnSiteRandomizer", 2, 1.5f);
    }

    //a janky randomizer so that invoke repeating, above, doesn't spawn 1 per side at every interval, but
    //instead only spawns on 1 random side per interval
    void SpawnSiteRandomizer()
    {
        whereToSpawn = Random.Range(0, 3);
        
        if(whereToSpawn == 0)
        {
            SpawnRandomAnimalBack();
        }
        if (whereToSpawn == 1)
        {
            SpawnRandomAnimalLeft();
        }
        if(whereToSpawn == 2)
        {
            SpawnRandomAnimalRight();
        }
    }

    //creates a random integer between 0 and 2 using the arrays.Length to get the exact num instead of us finding it.
    //then a random range method is used to spawn randomly along the X axis but constrained by our initial floats, and flat with our plane on
    //the Y axis, and at a specific point on our z axis. The prefab is instantiated from the animalIndex at the spawnPos with its 
    //existing rotation. This is repeated 2 more times to accomodate the expanded spawn zones using different x, y, z 
    //constraints and changing the rotation in euler angles to point towards the field/player.
    void SpawnRandomAnimalBack()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeLine, spawnRangeLine), 0, spawnRangePoint);

        Instantiate(animalPrefabs[animalIndex], spawnPos, animalPrefabs[animalIndex].transform.rotation);
    }

    void SpawnRandomAnimalLeft()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnPos = new Vector3(-spawnRangePoint, 0, Random.Range(- spawnRangeLine, spawnRangeLine));

        Instantiate(animalPrefabs[animalIndex], spawnPos, Quaternion.Euler(0, 90, 0));
    }

    void SpawnRandomAnimalRight()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnPos = new Vector3(spawnRangePoint, 0, Random.Range(-spawnRangeLine, spawnRangeLine));

        Instantiate(animalPrefabs[animalIndex], spawnPos, Quaternion.Euler(0, -90, 0));
    }
}
