using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineScript : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Start()
    {
        SetRenderer();
    }

    public void SetRenderer() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawLine(List<Vector2> points, Color color)
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Set the color of the line
        lineRenderer.material.color = color;

        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0));
        }
    }
}
