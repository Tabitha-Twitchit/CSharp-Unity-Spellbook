using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    private GameObject[] enemy;
    private Rigidbody rb;
    public ParticleSystem particle;
    public float speed;
    private float projectileLife = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        StartCoroutine(ProjectileDestroy());
    }

    /*As long as the first enemy in the array is not null, the projectile will move towards it. 
     * Strangely, I'm not sure why the resulting behavior is that after the first enemy is destroyed 
     * it automatically targets the next without need to be told to. Maybe, the second one just becomes 
     * the first one?*/
    void FixedUpdate()
    {
        if (enemy[0] != null)
        {
            Vector3 lookDirection = (enemy[0].transform.position - transform.position).normalized;
            rb.AddForce(lookDirection * speed);
        }
        
    }
    
    IEnumerator ProjectileDestroy()
    {
        yield return new WaitForSeconds(projectileLife);
        particle.Play();
        Destroy(gameObject, 0.3f);
        
    }
    

}
