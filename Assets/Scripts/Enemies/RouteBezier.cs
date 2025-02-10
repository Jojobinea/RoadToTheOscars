using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteBezier : MonoBehaviour
{
    [SerializeField] private Transform[] controlPoints;
    [SerializeField] private int resolution = 50; // Number of points in the curve
    private Vector3 gizmoPosition;

    private void OnDrawGizmos()
    {
        if (controlPoints.Length < 2)
            return;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            gizmoPosition = CalculateBezierPoint(t, controlPoints);
            Gizmos.DrawSphere(gizmoPosition, 0.1f);
        }

        // Draw control lines
        for (int i = 0; i < controlPoints.Length - 1; i++)
        {
            Gizmos.DrawLine(controlPoints[i].position, controlPoints[i + 1].position);
        }
    }

    private Vector3 CalculateBezierPoint(float t, Transform[] points)
    {
        if (points.Length == 1)
            return points[0].position;

        Transform[] newPoints = new Transform[points.Length - 1];
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 p0 = points[i].position;
            Vector3 p1 = points[i + 1].position;
            newPoints[i] = new GameObject().transform;
            newPoints[i].position = Vector3.Lerp(p0, p1, t);
        }

        Vector3 result = CalculateBezierPoint(t, newPoints);

        // Cleanup temporary objects
        foreach (var obj in newPoints)
        {
            DestroyImmediate(obj.gameObject);
        }

        return result;
    }
}
