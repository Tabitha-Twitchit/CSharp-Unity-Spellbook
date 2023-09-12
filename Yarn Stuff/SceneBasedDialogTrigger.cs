using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

/// <summary>
/// Allows different start nodes from a single Yarn script depending on the name of the current scene.
/// Obvi scene names would be changed per current project and further checks added for different chars. 
/// </summary>

public class SceneBasedDialogTrigger : MonoBehaviour
{
    private string conversationStartNode;
    private DialogueRunner dialogueRunner;
    private LineView lineView;

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        lineView = FindObjectOfType<Yarn.Unity.LineView>();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Car")
        {
            conversationStartNode = "Car";
        }
        if (sceneName == "Stairs")
        {
            conversationStartNode = "Stairs";
        }
        if (sceneName == "Layers")
        {
            conversationStartNode = "Layers";
        }
        if (sceneName == "Fountain")
        {
            conversationStartNode = "Fountain";
        }
        if (sceneName == "Playground")
        {
            conversationStartNode = "Playground";
        }
        if (sceneName == "Crocodile")
        {
            conversationStartNode = "Crocodile";
        }
        if (sceneName == "Kaleidoscope")
        {
            conversationStartNode = "Kaleidoscope";
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            lineView.OnContinueClicked();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            dialogueRunner.StartDialogue(conversationStartNode);
        }
    }
}
