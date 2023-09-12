using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float speed = 10.0f;
    public float xRange = 10.0f;
    public float zRange = 20.0f;
    public GameObject projectilePrefab;
    
    public int Score = 0;
    public int Lives = 3;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Lives =" + Lives);
        Debug.Log("Score =" + Score);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -xRange) 
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.z > zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
        }

        if (transform.position.z <= -15)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -15);
        }

        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);

        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        }

        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Critter") && Lives > 0)
        {
            Ouch();
        }
        else if (Lives <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    public void Ouch()
    {
        Lives -= 1;
        Debug.Log("Lives =" + Lives);
    }


}
