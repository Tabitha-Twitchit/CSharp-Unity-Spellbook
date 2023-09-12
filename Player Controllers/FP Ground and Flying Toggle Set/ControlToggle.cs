using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlToggle : MonoBehaviour
{
    private CharacterController characterController;
    private FPController firstPersonController;
    private FlyingController flyingController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        firstPersonController = GetComponent<FPController>();
        flyingController = GetComponent<FlyingController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeControl();
        }
    }

    void ChangeControl()
    {
        if(characterController.enabled && firstPersonController.enabled)
        {
            characterController.enabled = false;
            firstPersonController.enabled = false;

            flyingController.enabled = true;
        }
        else if (flyingController.enabled)
        {
            flyingController.enabled = false;

            characterController.enabled = true;
            firstPersonController.enabled = true;
        }
    }
}
