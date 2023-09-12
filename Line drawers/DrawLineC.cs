using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;


/// <summary>
/// This script allows the user to draw in world space 1 unit in front of the
/// camera.I recommend placing it on a transparent image within a canvas, if childed to the player
/// it will follow the player. When the camera is not locked it will allow the player to draw in 3d
/// space as they move. Using your ToggleControl script and, say FlyingControllerLocked, scripts you
/// can draw in both 2d space relative to the camera at time of lock, and then again in 3d when unlocked.
/// </summary>

public class DrawLineC : MonoBehaviour
{
    [SerializeField] Material lineMaterial;
    public float lineWidth = 0.01f;
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    public float fadeSpeed;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 1; // Set distance from the camera
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            points.Add(transform.InverseTransformPoint(worldPos));
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
        
        /*
         * It seems like something could be added here that stops the list being appended to
         * so the line doesn't leap to and connect the coordinates of the next click, a la
         * line tool in MS Paint.
        if (Input.GetMouseButtonUp(0))
        {  
        }
        */

        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(FadeOutObject());
        }
    }

    public IEnumerator FadeOutObject()
    {
        // while not totally invisible the line renderer is assigned a color and the alpha value is faded over time.
        //when it reaches 0 or less the next segment is performed.
        while (lineRenderer.material.color.a >= 0)
        {
            //this is a really key line that lets us put a pin in the current values we want to keep, while letting us change others
            Color objectColor = lineRenderer.material.color;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            lineRenderer.material.color = objectColor;
            yield return null;
        }
        
        //the points in the line are cleared.
        points.Clear();
        lineRenderer.positionCount = points.Count;
        
        //after the points are cleared, the transparency is resored to full so the line can be drawn, and the coroutine is ended.
        while (lineRenderer.material.color.a < 1)
        {
            Color objectColor = lineRenderer.material.color;
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 1);
            lineRenderer.material.color = objectColor;
            yield return null;
        }

    }


}

