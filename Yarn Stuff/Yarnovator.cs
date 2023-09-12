using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Yarnovator : MonoBehaviour 
{
    /* The script pulls a $floor variable in that is set in YarnSpinner dialog and matches it with 
     * an appropriate transform corresponding to the level of the building.
     * Note:There may well be a better way to do these floor transforms using an array.*/
    
    public Transform floor1, floor24, floor47;
    public float speed;
    public InMemoryVariableStorage varStore;
    private bool elevatorReady;
    private float usableFloorNum;

    /*when the player steps and stays on the platform (having already spoken to the elevator attendant)
     * the elevator reads the $floor value, and outputs it as a float and checks that the player
     * is the one who triggered it. Then it reparents the player to the elevator. It then sets a bool
     that allows the elevator to be called during fixedupdate*/
    private void OnTriggerEnter (Collider other)
    {
        
        if (varStore.TryGetValue("$floor", out float floorNum) && other.tag == "Player")
        {
            other.transform.parent = this.transform;
            //Make it wait a couple sec and close the doors.
            //int floorNumAsInt = (int)floorNum;
            Debug.Log("floor number:" + floorNum);
            usableFloorNum = floorNum;
            elevatorReady = true;
        }
    }
    //unparents the player when out of the trigger
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

    /*If the elevator is ready to be called based on the criteria in OnTriggerEnter then run
     * FloorSeeker*/
    void FixedUpdate()
    {
        if (elevatorReady == true)
        {
            FloorSeeker(usableFloorNum);
        }
    }
    
    /*checks the flor number as translated and passed in byonTriggerEnter and moves to the
    assigned public transform. As mentioned there's probably a better way to do this with Arrays*/
    void FloorSeeker(float usableFloorNum)
    {
        if (usableFloorNum == 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, floor1.position, speed * Time.deltaTime);
        }
        
        if (usableFloorNum == 24f)
        {
            transform.position = Vector3.MoveTowards(transform.position, floor24.position, speed * Time.deltaTime);
        }

        if (usableFloorNum == 47f)
        {
            transform.position = Vector3.MoveTowards(transform.position, floor47.position, speed * Time.deltaTime);
        }
        
    }
    

}
