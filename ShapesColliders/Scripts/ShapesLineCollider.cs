using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[ExecuteInEditMode]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Line))]
public class ShapesLineCollider : ShapesCollider
{
    Line line;
    Vector3 start;
    Vector3 end;
    float thickness;

    private void OnEnable()
    {
        line = GetComponent<Line>();
    }

    private void Start()
    {
        line = GetComponent<Line>();
    }

    public override void Update()
    {
        if (start != line.Start || end != line.End || thickness != line.Thickness)
            UpdateData();
    }

    public override void UpdateData()
    {
        if (poly == null) { return; }

        start = line.Start;
        end = line.End;
        thickness = line.Thickness;
        Refresh();
    }

    protected override void Refresh()
    {
        points.Clear();
        points.AddRange(DrawLine(start, end, thickness/2));
        points.AddRange(DrawLine(start, end, -(thickness / 2), true));
        poly.points = points.ToArray();
    }

    List<Vector2> DrawLine(Vector2 _start, Vector2 _end, float _thickness, bool reverseDrawOrder = false)
    {
        List<Vector2> _points = new List<Vector2>();
        Vector2 direction = (_end - _start).normalized;
        direction = ShapesMath.Rotate90CW(direction);
        _points.Add(_start + (direction * _thickness));
        _points.Add(_end + (direction * _thickness));

        if (reverseDrawOrder)
            _points.Reverse();
        return _points;
    }
}
