using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Triangle))]
public class ShapesTriangleCollider : ShapesCollider
{
    Triangle triangle;
    Vector3 A;
    Vector3 B;
    Vector3 C;

    private void Start()
    {
        triangle = GetComponent<Triangle>();
    }

    public override void Update()
    {
        if (A != triangle.A || B != triangle.B || C != triangle.C)
            UpdateData();
    }

    protected override void UpdateData()
    {
        if (poly == null) { return; }

        A = new Vector3(triangle.A.x, triangle.A.y);
        B = new Vector3(triangle.B.x, triangle.B.y);
        C = new Vector3(triangle.C.x, triangle.C.y);
        Refresh();
    }

    protected override void Refresh()
    {
        List<Vector2> points = new List<Vector2>
        {
            A,B,C
        };
        poly.points = points.ToArray();
    }
}
