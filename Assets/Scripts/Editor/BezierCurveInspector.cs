using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor {

    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;

	private void OnSceneGUI()
    {
        curve = target as BezierCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Global ? Quaternion.identity : handleTransform.rotation;

        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        Handles.color = Color.gray;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p2, p3);

        Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 4f);
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.PositionHandle(point, handleRotation);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            curve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
}
