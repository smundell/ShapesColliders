using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Triangle))]
public class ShapesTriangleCollider : MonoBehaviour
{
    public bool drawGizmos = false;

    PolygonCollider2D poly;
    Triangle triangle;
    Vector3 A;
    Vector3 B;
    Vector3 C;

    private void Awake()
    {
        triangle = GetComponent<Triangle>();
        poly = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if (A != triangle.A || B != triangle.B || C != triangle.C)
            UpdateData();
    }

    private void OnValidate()
    {
        UpdateData();
    }

    void UpdateData()
    {
        if (poly == null) { return; }

        A = new Vector3(triangle.A.x, triangle.A.y);
        B = new Vector3(triangle.B.x, triangle.B.y);
        C = new Vector3(triangle.C.x, triangle.C.y);
        Refresh();
    }

    void Refresh()
    {
        List<Vector2> points = new List<Vector2>
        {
            A,B,C
        };
        poly.points = points.ToArray();
    }

    public void OnDrawGizmos()
    {
        if (poly == null || !drawGizmos) { return; }
        PolylinePath path = new PolylinePath();
        float scale = 0.03f;
        foreach (Vector2 point in poly.points)
        {
            Vector2 adjustedPoint = point;
            adjustedPoint = ShapesMath.Rotate(adjustedPoint, transform.eulerAngles.z * Mathf.Deg2Rad);
            adjustedPoint *= transform.lossyScale.x;
            adjustedPoint += (Vector2)transform.position;

            Draw.Ring(adjustedPoint, scale, scale, Color.red);
            path.AddPoints(adjustedPoint);
        }

        Draw.Polyline(path, true, scale / 2, Color.red);
    }
}
