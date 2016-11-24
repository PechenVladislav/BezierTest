using UnityEngine;
using System;
using System.Collections;

public class Path : MonoBehaviour {

    [SerializeField]
    private Vector3[] points;

    public int PointsCount
    {
        get {return points.Length; }
    }

    public Vector3 GetPoint(int index)
    {
        return points[index];
    }

    public void SetPoint(int index, Vector3 point)
    {
        points[index] = point;
    }

    private void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(0f, 2f, 0f),
            new Vector3(8f, 2f, 0f),
            new Vector3(16f, 2f, 0f)
        };
    }

    public void AddCurve()
    {
        Vector3 point = points[points.Length - 1];
        Array.Resize<Vector3>(ref points, points.Length + 1);
        point.x += 8f;
        points[points.Length - 1] = point;
    }

    public void RemovePathPoint()
    {
        if(points.Length > 3)
        {
            Array.Resize(ref points, points.Length - 1);
        }
    }
}
