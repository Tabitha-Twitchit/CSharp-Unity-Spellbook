using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius;
    public float power;
    public ParticleSystem explosionParticle;
    
 
    void Start()
    {
        Debug.Log("Explosion Started");
        StartCoroutine(Bomb());
        explosionParticle.Play();

    }

    // Update is called once per frame
    void Update()
    {
    }
    
    IEnumerator Bomb()
    {
        yield return new WaitForSeconds(0.1f);
        
        /*Locates the explosion on this object's transform. Then creates an array of colliders 
         * within a sphere centered on the object's transform, the size of the radius variable.*/
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        /*accesses the rigidbodies of all collider objects within the sphere, and if the rigidbodies 
         * are not null, applies force.*/
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0f);
        }

        StartCoroutine(BombDestroyer());    
    }

    //removes the explosion game object
    IEnumerator BombDestroyer()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);

    }
}
