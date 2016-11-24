using UnityEngine;
using System.Collections;

public class DrawBackground : MonoBehaviour {

    public float bgWidth = 40f;

    public Color colorTopUp;
    public Color colorTop;
    public Color colorBottom;
    public Color colorBottomDown;

    private Mesh mesh;
    private DrawGround drawCurve;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;
	
	public void DrawBG ()
    {
        drawCurve = GetComponent<DrawGround>();
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = new Vector3[drawCurve.GetSegmentsCount * 2 * 2];
        triangles = new int[(drawCurve.GetSegmentsCount * 6 - 6) * 2];
        colors = new Color[vertices.Length];

        for (int i = 0, j = 0; j < vertices.Length / 2; i++, j += 2)                //нижняя часть
        {
            vertices[j] = drawCurve.GetSegmentPoint(i) + new Vector3(0f, 0f, 20f);
            colors[j] = Color.Lerp(colorBottom, colorBottomDown, vertices[j].y > 0 ? 0f : -vertices[j].y / bgWidth);
            vertices[j + 1] = new Vector3(vertices[j].x, -bgWidth, 20f);
            colors[j + 1] = Color.Lerp(colorBottom, colorBottomDown, vertices[j + 1].y > 0 ? 0f : -vertices[j + 1].y / bgWidth);
        }
        for (int i = 0, j = 0; i < triangles.Length / 2; i += 6, j += 2)            //нижняя часть
        {
            triangles[i] = j;
            triangles[i + 1] = triangles[i + 4] = j + 2;
            triangles[i + 2] = triangles[i + 3] = j + 1;
            triangles[i + 5] = j + 3;
        }


        for (int i = 0, j = vertices.Length / 2; j < vertices.Length - 1; i++, j += 2)      //верхняя часть
        {
            vertices[j] = drawCurve.GetSegmentPoint(i) + new Vector3(0f, 0f, 20f);
            colors[j] = Color.Lerp(colorTop, colorTopUp, vertices[j].y < 0 ? 0f : vertices[j].y / bgWidth);
            vertices[j + 1] = new Vector3(vertices[j].x, bgWidth, 20f);
            colors[j + 1] = Color.Lerp(colorTop, colorTopUp, vertices[j + 1].y < 0 ? 0f : vertices[j + 1].y / bgWidth);
        }
        for (int i = triangles.Length / 2, j = vertices.Length / 2; i < triangles.Length; i += 6, j += 2)       //верхняя часть
        {
            triangles[i] = j;
            triangles[i + 1] = triangles[i + 4] = j + 1;
            triangles[i + 2] = triangles[i + 3] = j + 2;
            triangles[i + 5] = j + 3;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
	}
}
