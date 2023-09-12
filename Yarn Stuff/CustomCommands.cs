using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CustomCommands : MonoBehaviour
{
   /*the script provides a one-stop-shop for all yarn commands. Where each method listed can be called
    * from YarnSpinner with the string in quotes.
    */
    public DialogueRunner dialogueRunner;
   
    public void Awake()
    {
        dialogueRunner.AddCommandHandler("thedoors", DoorOpener);
    }

    /*Ultimately the DoorOpener needs some tuning to scale--how is it made to open only the correct 
     * door, or would it be okay that it open all elevator doors by tag?
     */
    public void DoorOpener()
    {
        GameObject doors = new GameObject();
        doors = GameObject.Find("Doors");
        Animator myAnimator = doors.GetComponent<Animator>();
        myAnimator.SetBool("opening", true);
    }

    
}
