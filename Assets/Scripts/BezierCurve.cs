using UnityEngine;
using System;

public class BezierCurve : MonoBehaviour {

    [SerializeField]
    private Vector3[] points;

    public int PointsCount
    {
        get
        {
            return points.Length;
        }
    }

    public Vector3 GetPoint(int index)
    {
        return points[index];
    }

    public void SetPoint(int index, Vector3 point)
    {
        if (index % 3 == 0)                        //если выбрана средняя, то перемщать соседние точки
        {
            Vector3 delta = point - points[index];
            if (index > 0)
            {
                points[index - 1] += delta;
            }
            if (index + 1 < points.Length)
            {
                points[index + 1] += delta;
            }
        }
        points[index] = point;
        EnforceMode(index);
    }

    public int CurveCount
    {
        get
        {
            return (points.Length - 1) / 3;         //только если добавляем три точки
        }
    }

    [SerializeField]
    private BezierControlPointMode[] modes;

    public BezierControlPointMode GetPointMode(int index)
    {
        return modes[(index + 1) / 3];              //только если добавляем три точки
    }

    public void SetPointMode(int index, BezierControlPointMode mode)
    {
        modes[(index + 1) / 3] = mode;              //только если добавляем три точки
        EnforceMode(index);
    }

    private void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(2f, -1f, 0f),
            new Vector3(2f, 0f, 0f)
        };

        modes = new BezierControlPointMode[] {
            BezierControlPointMode.Free,
            BezierControlPointMode.Free
        };
    }

    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    }

    public void AddCurve()
    {
        Vector3 point = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        point.y += 1f;
        points[points.Length - 3] = point;
        point.x += 2f;
        point.y -= 2f;
        points[points.Length - 2] = point;
        point.y += 1f;
        points[points.Length - 1] = point;

        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];
        EnforceMode(points.Length - 4);
    }

    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free || modeIndex == 0 || modeIndex == modes.Length - 1)
        {
            return;
        }

        int middlePointIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (index <= middlePointIndex)
        {
            fixedIndex = middlePointIndex - 1;
            enforcedIndex = middlePointIndex + 1;
        }
        else {
            fixedIndex = middlePointIndex + 1;
            enforcedIndex = middlePointIndex - 1;
        }

        Vector3 middle = points[middlePointIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        points[enforcedIndex] = middle + enforcedTangent;
    }
}
