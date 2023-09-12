using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    
    public float speed;
 
    // Update is called once per frame
    void Update()
    {
        //                 new vector3(1, 0, 0) * 5 * real time
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * speed * Time.deltaTime);
    }
}
