using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * speed);
    }
}
