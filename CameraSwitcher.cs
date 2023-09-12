using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
   //the cameras or game objects to be toggled among
    public GameObject cam1;
    public GameObject cam2;

//every frame check for the input to be run, and if down, run the custom function ToggleCam
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleCam();
        }
    }

    /*set a boolean (true/false) that corresponds to whether the game object is active. When the funtion is run
    flip it to the opposite state. This means the game would need to start with the main cam active, and the 
    second cam deactivated. */
    void ToggleCam()
    {
        bool cam1State = cam1.activeSelf;
        bool cam2State = cam2.activeSelf;
        cam1.SetActive(!cam1State);
        cam2.SetActive(!cam2State);
    }
}
