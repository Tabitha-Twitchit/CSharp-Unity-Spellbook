using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Critter"))
        {
            Destroy(gameObject);
        }
    }
}
