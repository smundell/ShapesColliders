using Shapes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Polyline))]
public class ShapesPolylineCollider : ShapesCollider
{
    Polyline polyLine;
    bool closed = true;
    float thickness;

    private void OnEnable()
    {
        polyLine = GetComponent<Polyline>();
    }

    private void Start()
    {
        polyLine = GetComponent<Polyline>();
    }

    public override void UpdateData()
    {
        if (poly == null) { return; }
        thickness = polyLine.Thickness;
        closed = polyLine.Closed;
        Refresh();
    }

    protected override void Refresh()
    {
        points.Clear();
        points.AddRange(DrawLine(polyLine.PolyPoints, thickness / 2, closed));
        points.AddRange(DrawLine(polyLine.PolyPoints, -(thickness / 2), closed, true));
        poly.points = points.ToArray();
    }

    Vector2 GetPoint(Vector2 _point, Vector2 p1, Vector2 p2, float d)
    {
        Vector2 p12 = (p2 - p1).normalized;
        return _point + d * new Vector2(-p12.y, p12.x);
    }

    List<Vector2> DrawLine(List<PolylinePoint> polyPoints, float _thickness, bool _closed, bool reverseDrawOrder = false)
    {
        List<Vector2> _points = new List<Vector2>();
        for (int i = 0; i < polyPoints.Count; i++)
        {
            Vector2 result = polyPoints[i].point;
            if (i == 0)
            {
                result = GetPoint(polyPoints[i].point, polyPoints[i].point, polyPoints[i + 1].point, _thickness * polyPoints[i].thickness);
            }
            else if (i == polyPoints.Count-1)
            {
                result = GetPoint(polyPoints[i].point, polyPoints[i-1].point, polyPoints[i].point, _thickness * polyPoints[i].thickness);
            }
            else
            {
                Vector2 p1 = polyPoints[i - 1].point;
                Vector2 p2 = polyPoints[i].point;
                Vector2 p3 = polyPoints[i + 1].point;

                Vector2 p12 = (p2 - p1).normalized;
                Vector2 n12 = new Vector2(-p12.y, p12.x);

                Vector2 p23 = (p3 - p2).normalized;
                Vector2 n23 = new Vector2(-p23.y, p23.x);

                Vector2 n123 = (n12 + n23).normalized;

                if (polyLine.Joins == PolylineJoins.Simple)
                    result = p2 + (_thickness * polyPoints[i].thickness) * n123;
                else
                    result = p2 + ((_thickness * polyPoints[i].thickness) / Vector2.Dot(n12, n123)) * n123;
            }

            _points.Add(result);
        }

        if (reverseDrawOrder)
            _points.Reverse();
        return _points;
    }

    private void OnDrawGizmosSelected()
    {
        UpdateData();
    }
}
