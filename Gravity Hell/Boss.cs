using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject bossProjectile;

    /*In addition to the enemy script, the boss has the additional power of firing projectiles at regular intervals*/
    void Start()
    {
        InvokeRepeating("Fire", 2, 3);
    }

    void Fire()
    {
        Instantiate(bossProjectile, transform.position, transform.rotation);
    }

}
