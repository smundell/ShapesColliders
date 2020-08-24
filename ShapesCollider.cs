using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[ExecuteInEditMode]
[RequireComponent(typeof(PolygonCollider2D))]
public class ShapesCollider : MonoBehaviour
{
    public bool drawGizmos = false;
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
        float scale = Mathf.Clamp(0.03f * qualityLevel, 0.02f, 1);
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
