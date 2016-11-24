using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Path))]
public class PathInspector : Editor {

    private Path path;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private int handleSelectedIndex = -1;
    private const float handleSize = 0.06f;
    private const float handlePickSize = 0.1f;

    private void OnSceneGUI()
    {

        path = target as Path;
        handleTransform = path.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Global ? Quaternion.identity : handleTransform.rotation;

        Handles.color = Color.white;
        Vector3 p0 = ShowPoint(0);
        for(int i = 1; i < path.PointsCount; i++)
        {
            Vector3 p1 = ShowPoint(i);
            Handles.DrawLine(p0, p1);
            p0 = p1;
        }
    }

    public override void OnInspectorGUI()
    {
        path = target as Path;
        if (handleSelectedIndex >= 0 && handleSelectedIndex < path.PointsCount)
        {
            DrawSelectedPointInspector();
        }
        if (GUILayout.Button("Add PathPoint"))
        {
            Undo.RecordObject(path, "Add PathPoint");
            path.AddCurve();
            EditorUtility.SetDirty(path);
        }
        if (GUILayout.Button("Remove PathPoint"))
        {
            Undo.RecordObject(path, "Remove PathPoint");
            path.RemovePathPoint();
            EditorUtility.SetDirty(path);
        }
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", path.GetPoint(handleSelectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(path, "Move Point");
            EditorUtility.SetDirty(path);
            path.SetPoint(handleSelectedIndex, point);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(path.GetPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        if (Handles.Button(point, handleRotation, handleSize * size, handlePickSize * size, Handles.DotCap))
        {
            handleSelectedIndex = index;
            EditorUtility.SetDirty(path);
        }
        if (handleSelectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.PositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(path, "Move Point");
                path.SetPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }
        return point;
    }
}
