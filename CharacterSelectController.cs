using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectController : MonoBehaviour
{
    /// <summary>
    /// Used in American Tarot/Memory as a Rock... to rotate characters in the fountain screen.
    /// mainly saved to demonstrate how a counter can be used to trigger an animation controller
    /// </summary>
    
    [SerializeField] Animator carouselAnimator;
    private int pressCount;
    // Start is called before the first frame update
    void Start()
    {
        pressCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && pressCount <= 8)
        {
            pressCount++;
        }
        else
        { 
            if (pressCount > 8)
                { 
                    pressCount = 1;
                }
        }
        carouselAnimator.SetInteger("switchCount", pressCount);
    }
}
