using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private GameObject player;
    private Rigidbody rb;
    private Renderer rend;
    public ParticleSystem particle;
    public float speed;
    private float projectileLife = 0.8f;
    
    
    /* get access to the rigidbody for movement, renderer for disappearing the projectile, while allowing the particle
     * to still play (destroying game object stifles the particle). Begin the countdown to destroying the game object.*/
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponentInChildren<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ProjectileDestroy());
    }

    /*lookat ensures object faces the player, not just moves toward them*/
    void FixedUpdate()
    {
        transform.LookAt(player.transform.position);
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        rb.AddForce(lookDirection * speed);
    }

    
    /*waits the requisite amount of time, plays the explosion particles, turns off the projectile's renderer and destroys 
     * the object after the particle is done.*/
    IEnumerator ProjectileDestroy()
    {
        yield return new WaitForSeconds(projectileLife);
        particle.Play();
        rend.enabled = false;
        Destroy(gameObject, 0.3f);

    }
}
