using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class DrawGround : MonoBehaviour {

    private BezierCurve editorCurve;
    private int segmentsPerCurve = 20;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private Vector3[] curveSegments;

    public int GetSegmentsCount
    {
        get { return curveSegments.Length; }
    }

    // Use this for initialization
    void Start () {
        editorCurve = GetComponent<BezierCurve>();
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        curveSegments = new Vector3[editorCurve.CurveCount * segmentsPerCurve + 1];
        lineRenderer.SetVertexCount(editorCurve.CurveCount * segmentsPerCurve + 1);
        for (int l = 0, j = 0; l < editorCurve.CurveCount; l++, j += 3)
        {
            for (int i = 0; i <= segmentsPerCurve; i++)
            {
                float t = (float)i / segmentsPerCurve;
                curveSegments[i + segmentsPerCurve * l] = Bezier.GetPoint(editorCurve.GetPoint(j), editorCurve.GetPoint(j + 1), editorCurve.GetPoint(j + 2), editorCurve.GetPoint(j + 3), t);
            }
        }
        lineRenderer.SetPositions(curveSegments);
        
        edgeCollider.points = V3ToV2(curveSegments);
    }

    public Vector3 GetSegmentPoint(int index)
    {
        return curveSegments[index];
    }

    public Vector2[] V3ToV2(Vector3[] curveSegmentsV3)
    {
        Vector2[] curveSegmentsV2 = new Vector2[curveSegmentsV3.Length];
        for (int i = 0; i < curveSegmentsV3.Length; i++)
        {
            curveSegmentsV2[i] = curveSegmentsV3[i];
        }

        return curveSegmentsV2;
    }
}
