using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BezierCurve))]
public class BezierSplineInspector : Editor
{

    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const float handleSize = 0.06f;
    private const float handlePickSize = 0.1f;
    private int handleSelectedIndex = -1;
    private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        Color.cyan
    };

    private void OnSceneGUI()
    {
        curve = target as BezierCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Global ? Quaternion.identity : handleTransform.rotation;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < curve.PointsCount; i += 3) //i+=3 только если добавляем три точки
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 4f);
            p0 = p3;
        }

        
    }

    public override void OnInspectorGUI()
    {
        curve = target as BezierCurve;
        if (handleSelectedIndex >= 0 && handleSelectedIndex < curve.PointsCount)
        {
            DrawSelectedPointInspector();
        }
        if (GUILayout.Button("Add To Spline"))
        {
            Undo.RecordObject(curve, "Add Curve");
            curve.AddCurve();
            EditorUtility.SetDirty(curve);
        }
        if (GUILayout.Button("Remove From Spline"))
        {
            Undo.RecordObject(curve, "Remove Curve");
            curve.RemoveCurve();
            EditorUtility.SetDirty(curve);
        }
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", curve.GetPoint(handleSelectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.SetPoint(handleSelectedIndex, point);
        }
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", curve.GetPointMode(handleSelectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Change Point Mode");
            curve.SetPointMode(handleSelectedIndex, mode);
            EditorUtility.SetDirty(curve);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.GetPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        Handles.color = modeColors[(int)curve.GetPointMode(index)];
        if (Handles.Button(point, handleRotation, handleSize * size, handlePickSize * size, Handles.CircleCap))
        {
            handleSelectedIndex = index;
            EditorUtility.SetDirty(curve);
        }
        if (handleSelectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.PositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curve, "Move Point");
                curve.SetPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }
        return point;
    }
}
