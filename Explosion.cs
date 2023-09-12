using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionForce = 10f;
    public float explosionRadius = 10f;
    public float explosionZone = 6f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Vector3 randomPosition = new Vector3(Random.Range(-explosionZone, explosionZone), Random.Range(-explosionZone, explosionZone), Random.Range(-explosionZone, explosionZone));
            Collider[] colliders = Physics.OverlapSphere(randomPosition, explosionRadius);

            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(explosionForce, randomPosition, explosionRadius);
            }
        }

    }
}
