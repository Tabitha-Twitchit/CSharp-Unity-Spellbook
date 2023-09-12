using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    /*Note this needs a special PlayerController script that defines the lives functionality. 
     * OK, this got nutty and would probably be neater with a game manager central object instead 
     * of passing things back and forth. 2 floats to specify the play zone so if critters exit they 
     * get destroy. One reference to the player so we can get into their scoring script.
     */
    public float zRange = 30.0f;
    public float xRange = 30.0f;
    private GameObject player;


    
    /*An example of how to set the player variable to the specific player we are wanting to affect using
     * its specific name vs a tag. This could also be done with a reference to the script instead of 
     * the over arching object, I think. 
     */
    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        //if this transform goes beyond the range on the x and z axes it triggers the CritterTrack
        //method and then the object gets destroyed. 
        if (transform.position.z > zRange)
        {            
            CritterTrack();
            Destroy(gameObject);
        }

        if (transform.position.x > xRange)
        {            
            CritterTrack();
            Destroy(gameObject);
        }

        if (transform.position.z < -zRange)
        {
            CritterTrack();
            Destroy(gameObject);
        }

        if (transform.position.x < -xRange)
        {
            CritterTrack();
            Destroy(gameObject);
        }
    }

    /*OK the nutty part. Within the custom method we go down the trail of breadcrumbs: player>
     * onto the playercontroller script > no idea why empty parentheses > to the specific variable
     * called Lives. We check this using the same basic pattern as on the script itself. The part 
     * that took forever to figure out was that each and every time we want to modify Lives, we 
     * have to re describe the trail of breadcrumbs. I haven't been able to find a way to rename 
     * this var locally within this script to something more manageable and also have it work on
     * the player's script
     */
    
    void CritterTrack()
    {
        if (player.GetComponent<PlayerController>().Lives > 0)
        {
            player.GetComponent<PlayerController>().Lives -= 1;
            Debug.Log("Lives =" + player.GetComponent<PlayerController>().Lives);
        }
        else if (player.GetComponent<PlayerController>().Lives <= 0)
        {
            Debug.Log("Game Over");
        }
        //Debug.Log("Lives =" );
        //player.SendMessage("Ouch");
        
    }
}
