using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneLoop : MonoBehaviour

/*A scene controller on key press that loops back to the initial scene after completion of first
 */
{
    //the threshold at which the game loops back to initial scene
    private int sceneMax;

    void Start()
    {
        //because the build index numbers start at 0, and the scene count starts at 1
        //we substract one to correct for this
        sceneMax = SceneManager.sceneCountInBuildSettings - 1;
    }

    void Update()
    {
        /*
        To check whether numbers are aligning use this:
        
        //Build Index is the current scene index number, will change
        Debug.Log("Build Index" + SceneManager.GetActiveScene().buildIndex);
        //the number of scenes in the game
        Debug.Log("Total Scenes in Build" + SceneManager.sceneCountInBuildSettings);
        //the corrected sceneMax that should be 1 less than Total Scenes in Build
        Debug.Log("Scene Max" + sceneMax);
        */

        if (Input.GetKeyDown(KeyCode.Period))
        {
            if (SceneManager.GetActiveScene().buildIndex == sceneMax)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                SceneManager.LoadScene(sceneMax);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }
}