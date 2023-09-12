using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineB : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 previousPosition;

    [SerializeField]
    private float minDistance = 0.1f;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 1;
        previousPosition = transform.position;

    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {

            Vector3 currentPosition = Camera.main.WorldToViewportPoint(Input.mousePosition);
            currentPosition.z = 0f;

            /*
            Lets the player draw flat but the offset between where you look and where the mark shows is extreme
            Vector3 currentPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
            currentPosition.z = 0f;
            */

            /*
            Creates a tool that lets the player draw in 3d
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            */

            /*Original solution, doesn't work for my use case
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0f;
            */
            if (Vector3.Distance(currentPosition, previousPosition) > minDistance)
            {
                if(previousPosition == transform.position)
                {
                    line.SetPosition(0, currentPosition);
                }
                else
                {
                    line.positionCount++;
                    line.SetPosition(line.positionCount - 1, currentPosition);
                }
                
                previousPosition = currentPosition;
            }
        }
    }
}
