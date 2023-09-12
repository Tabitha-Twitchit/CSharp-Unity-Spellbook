using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed;
    public float dashMod;
    private PlayerController playerControllerScript;
    private float leftBound = -12;

    /*MoveLeft is intended to pair with a specific PlayerController script for infinite running, as well as a backgroun
     * repeater, ground texture scroller and spawn manager*/
    
    /*sets our private playercontroller var as the specific one by name then accessing its script component*/
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }


    /*If dashing (as set by the playercontrollerscript) and the gameOff state is FALSE (game is running), things 
     * will move faster using the dashMod value. Otherwise things will run at normal speed.*/
    void Update()
    {
       
        if(playerControllerScript.dashBool == true && playerControllerScript.gameOff == false)
        {
            transform.Translate(Vector3.left * speed * dashMod * Time.deltaTime);
        }
        else if (playerControllerScript.gameOff == false)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
         
     /*Destroys obstacles that move left off screen*/
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

}
