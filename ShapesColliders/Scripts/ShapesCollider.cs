using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[ExecuteInEditMode]
[RequireComponent(typeof(PolygonCollider2D))]
public class ShapesCollider : MonoBehaviour
{
    [Header("Gizmo Properties")]
    public bool drawGizmos = false;
    [Range(0.01f, 0.1f)] public float gizmoScale = 0.04f;
    public Color pointColor = Color.red;
    public Color lineColor = Color.green;

    [Header("Collider Properties")]
    [Range(0.01f, 1)] public float qualityLevel = 0.2f;

    protected PolygonCollider2D poly;
    protected List<Vector2> points;

    public virtual void Awake()
    {
        poly = GetComponent<PolygonCollider2D>();
        points = new List<Vector2>();
    }

    public virtual void Update()
    {
        
    }

    public void OnValidate()
    {
        UpdateData();
    }

    public virtual void UpdateData()
    {
        if (poly == null) { return; }
        Refresh();
    }

    protected virtual void Refresh()
    {
        points.Clear();

        poly.points = points.ToArray();
    }

    public virtual void OnDrawGizmos()
    {
        if (poly == null || !drawGizmos) { return; }
        PolylinePath path = new PolylinePath();

        foreach (Vector2 v in poly.points)
        {
            path.AddPoints(poly.transform.TransformPoint(v));
        }

        Draw.Polyline(path, true, gizmoScale / 4, lineColor);

        for (int i = 0; i < path.Count; i++)
        {
            Draw.Rectangle(path[i].point, gizmoScale, gizmoScale, pointColor);
        }
    }
}
