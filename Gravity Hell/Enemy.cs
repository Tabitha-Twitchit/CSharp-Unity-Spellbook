using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRB;
    private GameObject player;
    private float destroyDistance = -7.5f;
    
    // Start is called before the first frame update
    void Start()
    {
       //accesses this rigidbody and reference for the player
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        //if Enemy falls, destroy it.
        if(transform.position.y < destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        //sets a vector between the player and the enemy and normalizes it. This prevents the multiplier
        //it will become in the next expression from being too high if they're far apart--KABLAM. It then 
        //adds force on the enemy's rigidbody in that direction with an added multiplie for its speed.
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRB.AddForce(lookDirection * speed);
    }
}
