using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRayFacer : MonoBehaviour
{
    //the thing you want this game object to face
    //private GameObject targetParent;
    public Transform target;

    /*this method is called using another script on the player hit.transform.SendMessage("HitByRay"); after
     * the raycast. WILDLY, super learning experience you can trigger custom methods this way! Note that I
     * originally tried to apply this to an animated game object and after a lot of trial and error realized
     * had to be on the top level parent with the animator rather than the child with the skinned mesh renderer
     */
    void start()
    {
        /*targetParent = 
        target = cam.GetComponent<PlayerCam>();
        playerController = player.GetComponent<PlayerMovement>();*/
    }
    
    void HitByRay()
    {
        
        transform.LookAt(target);
        Debug.Log("I was hit by a ray!");
        
    }
}
