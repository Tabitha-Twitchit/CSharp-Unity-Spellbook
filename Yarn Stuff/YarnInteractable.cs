using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnInteractable : MonoBehaviour {
    // internal properties exposed to editor
    [SerializeField] private string conversationStartNode;
    
    /*TABITHA YOU ADDED THESE 4 VARS to establish the objects and scripts on obj to disable when starting 
    and ending conversation. In this case camera and player movement. Note, you initially set these as public
    however because of the scene change issue the public fields wouldn't stay persistent (static?) so instead 
    in the start method you search for the specific names of the camera/player and then refer to their components. 
    public GameObject cam;
    private Behaviour camScript;
    public GameObject player;
    private Behaviour playerController;*/

    private GameObject cam;
    private Behaviour camScript;
    private GameObject player;
    private Behaviour playerController;

    // internal properties not exposed to editor
    private DialogueRunner dialogueRunner;
    //private Light lightIndicatorObject = null;
    private bool interactable = true;
    private bool isCurrentConversation = false;
    private float defaultIndicatorIntensity;

    public void Start() {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueRunner.onDialogueComplete.AddListener(EndConversation);

        //TABITHA you added this to get the specific player script(s) to activate and deactivate and store at start.
        //note that the gameobject.find method is what allows you to not destroy the players/cams from prev scenes but
        //still reference them.
        player = GameObject.Find("Player");
        cam = GameObject.Find("PlayerCam");
        camScript = cam.GetComponent<PlayerCam>();
        playerController = player.GetComponent<PlayerMovement>();

        
        //lightIndicatorObject = GetComponentInChildren<Light>();
        // get starter intensity of light then
        // if we're using it as an indicator => hide it 
        //if (lightIndicatorObject != null) {
        //    defaultIndicatorIntensity = lightIndicatorObject.intensity;
        //    lightIndicatorObject.intensity = 0;
        //}
    }

    public void OnMouseDown() {
        if (interactable && !dialogueRunner.IsDialogueRunning) {
            StartConversation();
        }
    }

    private void StartConversation() {
        Debug.Log($"Started conversation with {name}.");
        isCurrentConversation = true;
        //TABITHA you added this next 3 lines to make the cursor come up when you start a convo, As
        //well as disabling mouse look.
        // see also lines 52-54 in EndConversation function
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        camScript.enabled = false;
        playerController.enabled = false;
        
        // if (lightIndicatorObject != null) {
        //     lightIndicatorObject.intensity = defaultIndicatorIntensity;
        // }
        dialogueRunner.StartDialogue(conversationStartNode);
    }

    private void EndConversation() {
        if (isCurrentConversation) {
            // if (lightIndicatorObject != null) {
            //     lightIndicatorObject.intensity = 0;
            // }
            //TABITHA you added this next 3 lines to make the cursor go away when you end a convo. As
            //well as disabling mouse look.
            //See also lines 38-41 in StartConversation function
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            camScript.enabled = true;
            playerController.enabled = true;

            isCurrentConversation = false;
            Debug.Log($"Started conversation with {name}.");
        }
    }

//    [YarnCommand("disable")]
    public void DisableConversation() {
        interactable = false;
    }
}