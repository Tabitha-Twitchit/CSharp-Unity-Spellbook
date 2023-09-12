using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    public List<Vector2> mousePositions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }

        if(Input.GetMouseButton(0))
        {
            Vector2 tempMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(tempMousePos, mousePositions[mousePositions.Count - 1]) > .1f)
            {
                UpdateLine(tempMousePos);
            }
        }
    }

    void CreateLine()
    {
        //currentLine = Instantiate(linePrefab, drawingPlane.position, Quaternion.identity);

        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer =  currentLine.GetComponent<LineRenderer>();
        mousePositions.Clear();
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        lineRenderer.SetPosition(0, mousePositions[0]);
        lineRenderer.SetPosition(1, mousePositions[1]);
    }

    void UpdateLine(Vector2 newMousePos)
    {
        mousePositions.Add(newMousePos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount -1, newMousePos);

    }
}
