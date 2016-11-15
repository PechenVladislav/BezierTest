using UnityEngine;
using System.Collections;

public class BezierCurve : MonoBehaviour {

    public Vector3[] points;

	private void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(2f, -1f, 0f),
            new Vector3(2f, 0f, 0f)
        };
    }

    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    }
}
