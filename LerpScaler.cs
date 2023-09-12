using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LerpScaler : MonoBehaviour
{
    //access material for color changing
    public MeshRenderer Renderer;

    //values to randomize for color changin
    private float redVal;
    private float greVal;
    private float bluVal;
    private float alphVal;

    //rotation speed for spinning the object
    public float rotSpeed;
     
    //variables for scaling
    private Vector3 minScale;
    public Vector3 maxScale;
    public bool repeatable;
    public float lerpSpeed;
    public float duration;

    IEnumerator Start()
    {
        minScale = transform.localScale;
        while (repeatable)
        {
            yield return RepeatLerp(minScale, maxScale, duration);
            yield return RepeatLerp(maxScale, minScale, duration);
        }

    }

    void Update()
    {
        redVal = Random.Range(0f, 1f);
        greVal = Random.Range(0f, 1f);
        bluVal = Random.Range(0f, 1f);
        alphVal = Random.Range(0f, 1f);
        rotSpeed = Random.Range(0f, 50f);

        Material material = Renderer.material;
        material.color = new Color(redVal, greVal, bluVal, alphVal);

        transform.Rotate(rotSpeed * Time.deltaTime, 0.0f, 0.0f);
        transform.Rotate(Vector3.forward, rotSpeed*Time.deltaTime);
        
    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time) 
    {
        float i = 0.0f;
        float rate = (1.0f / time) * lerpSpeed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
        
    }
}
