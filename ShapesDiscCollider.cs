using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[ExecuteInEditMode]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Disc))]
public class ShapesDiscCollider : MonoBehaviour
{
    public bool drawGizmos = false;
    [Range(0.01f, 1)] public float qualityLevel = 0.2f;

    Disc ring;
    PolygonCollider2D poly;

    DiscType type;
    float radius;
    float thiccness;
    float angRadStart;
    float angRadEnd;
    List<Vector2> points;

    private void Start()
    {
        ring = GetComponent<Disc>();
        poly = GetComponent<PolygonCollider2D>();
        points = new List<Vector2>();
    }

    private void Update()
    {
        if (type != ring.Type || radius != ring.Radius || thiccness != ring.Thickness || angRadStart != ring.AngRadiansStart || angRadEnd != ring.AngRadiansEnd)
            UpdateData();
    }

    public void OnValidate()
    {
        UpdateData();
    }

    void UpdateData()
    {
        if(poly == null) { return; }

        type = ring.Type;
        radius = ring.Radius;
        thiccness = ring.Thickness;
        angRadStart = ring.AngRadiansStart;
        angRadEnd = ring.AngRadiansEnd;
        Refresh(type, radius, thiccness, angRadStart, angRadEnd);
    }

    void Refresh(DiscType _type, float _radius, float _thiccness, float _angRadStart, float _angRadEnd)
    {
        points.Clear();

        switch (_type)
        {
            case DiscType.Disc:
                _angRadStart = 0;
                _angRadEnd = 360;
                _thiccness = 0;
                points.AddRange(GetCircle(_radius + _thiccness, _angRadStart, _angRadEnd));
                break;
            case DiscType.Pie:
                _thiccness = 0;
                points.Add(Vector2.zero);
                points.AddRange(GetCircle(_radius + _thiccness, _angRadStart, _angRadEnd));
                break;
            case DiscType.Ring:
                _angRadStart = 0;
                _angRadEnd = 360;
                _thiccness /= 2;
                points.AddRange(GetCircle(_radius + _thiccness, _angRadStart, _angRadEnd));
                points.AddRange(GetCircle(_radius + -_thiccness, _angRadStart, _angRadEnd, true));
                break;
            case DiscType.Arc:
                _thiccness /= 2;
                points.AddRange(GetCircle(_radius + _thiccness, _angRadStart, _angRadEnd));
                points.AddRange(GetCircle(_radius + -_thiccness, _angRadStart, _angRadEnd, true));
                break;
            default:
                break;
        }

        poly.points = points.ToArray();
    }

    List<Vector2> GetCircle(float _radius, float _angRadStart, float _angRadEnd, bool reverse = false)
    {
        List<Vector2> points = new List<Vector2>();
        float angle = -_angRadStart;
        float arcLength = Mathf.Clamp(Mathf.Abs(_angRadEnd - _angRadStart), 0, Mathf.PI * 2);
        int direction = (int)Mathf.Sign(_angRadEnd - _angRadStart);
        int stepCount = Mathf.Max(Mathf.FloorToInt((arcLength / qualityLevel)), 2);

        for (int i = 0; i <= stepCount; i++)
        {
            float x = Mathf.Sin(angle) * _radius;
            float y = Mathf.Cos(angle) * _radius;
            Vector2 point = new Vector2(x, y);
            point = ShapesMath.Rotate90CW(point);
            points.Add(point);

            angle += (arcLength / stepCount) * -direction;
        }

        if (reverse)
            points.Reverse();

        return points;
    }

    public void OnDrawGizmos()
    {
        if(poly == null || !drawGizmos) { return; }
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

        Draw.Polyline(path, true, scale/2, Color.red);
    }
}
