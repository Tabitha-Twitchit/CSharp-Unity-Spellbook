using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This specific texture scroller is setup to be part of the Infinite Runner set, allowing for variation
 * in speed while dashing. It's intended for use on the ground, which can't move physically in the same 
 * way the background does*/

public class ScrollTex : MonoBehaviour {

	public float ScrollX;
	public float ScrollY;
    public float dashMod;
    private PlayerController playerControllerScript;

	private void Start()
	{
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    void Update ()
    {
        if (playerControllerScript.dashBool == true && playerControllerScript.gameOff == false)
        {
            float OffsetX = Time.time * ScrollX * dashMod;
            float OffsetY = Time.time * ScrollY * dashMod;
            GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
        }
        else if (playerControllerScript.gameOff == false)
        {
            float OffsetX = Time.time * ScrollX;
            float OffsetY = Time.time * ScrollY;
            GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
        }
	}
}
