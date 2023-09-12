using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Print_to_Log : MonoBehaviour
{
    void Update()
    {
        //if space key press down
        //print message

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space Key");
        }
    }
}

// note: Another use of Debug.Log("Your words here"); is that you can place it within a script that's causing problems
//and see where the script stops being parsed, by checking whether or not "Your words here" print to the console. If 
//not the error occurs before you call Debug.Log