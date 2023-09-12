using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TimeMachine : MonoBehaviour
{
    private AudioSource[] allAudioSources;
    private VideoPlayer[] allVideoPlayers;

    private void Start()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        allVideoPlayers = FindObjectsOfType(typeof(VideoPlayer)) as VideoPlayer[];
    }

    private void Update()
    {
        Debug.Log("Current time=" + Time.timeScale);
    }
    void FixedUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Time.timeScale += 0.1f;
            shiftAudio();
            shiftVideo();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Time.timeScale -= 0.1f;
            shiftAudio(); 
            shiftVideo();
        }        
    }
    void shiftAudio()
    {
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.pitch = Time.timeScale;
        }
    }

    /*note the pitch shift in shift audio doesn't effect the audio in video player afaik, but 
     * you can set the audio output mode to be an audio player and I think it would then effect it.*/
    void shiftVideo()
    {
        foreach (VideoPlayer videoP in allVideoPlayers)
        {
            videoP.playbackSpeed = Time.timeScale;
        }
    }

}
// you could also use a key,:
//if (Input.GetKeyDown(KeyCode.LeftBracket))
