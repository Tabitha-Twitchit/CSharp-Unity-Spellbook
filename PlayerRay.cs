using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRay : MonoBehaviour
{
    //the distance the ray will travel in both the debug and the game
    public float range;
    public GameObject reticle;


    /*a very flexible script that casts a ray on a given input from a given point, presently the mouse location
     * but could be made to be the screen center etc. The ray can be adapted in different IF statements to 
     * check for a variety of tags and if so can send a message to the tagged object hit by the ray and call
     * methods in scripts on that game object. Here, it tells an NPC to face the player when hit by a ray so 
     * that they can have a conversation. A good ray tutorial:
     * https://gamedevbeginner.com/raycasts-in-unity-made-easy/
     */
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Debug.DrawRay(ray.origin, ray.direction * range);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, range) && hit.collider.gameObject.CompareTag("NPC"))
            {
                reticle.gameObject.SetActive(true);                                  
            }

            else
            {
                reticle.gameObject.SetActive(false);
            }
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray2 = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Debug.DrawRay(ray2.origin, ray2.direction * range);

            RaycastHit hit;
            if (Physics.Raycast(ray2, out hit, range) && hit.collider.gameObject.CompareTag("NPC"))
            {

                hit.transform.SendMessage("HitByRay");
                Debug.Log("hit NPC");     
            }
        }
    }
}
