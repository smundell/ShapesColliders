using Shapes;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Disc))]
public class ShapesDiscCollider : ShapesCollider
{
    public bool flexible = true;
    public bool flexWithRadius = true;
    public int fixedCount = 10;

    Disc disc;
    DiscType type;
    ArcEndCap cap;
    float radius;
    float thiccness;
    float angRadStart;
    float angRadEnd;

    private void OnEnable()
    {
        disc = GetComponent<Disc>();
    }

    private void Start()
    {
        disc = GetComponent<Disc>();
    }

    public override void Update()
    {
        // Recalc on variable changes
        // Would be good to have some type of OnValidate event for shapes
        if (type != disc.Type || cap != disc.ArcEndCaps || radius != disc.Radius 
            || thiccness != disc.Thickness || angRadStart != disc.AngRadiansStart 
            || angRadEnd != disc.AngRadiansEnd)
            UpdateData();
    }

    public override void UpdateData()
    {
        if(poly == null) { return; }

        type = disc.Type;
        cap = disc.ArcEndCaps;
        radius = disc.Radius;
        thiccness = disc.Thickness;
        angRadStart = disc.AngRadiansStart;
        angRadEnd = disc.AngRadiansEnd;
        Refresh(type, radius, thiccness, angRadStart, angRadEnd);
    }

    protected void Refresh(DiscType _type, float _radius, float _thiccness, float _angRadStart, float _angRadEnd)
    {
        points.Clear();
        
        switch (_type)
        {
            case DiscType.Disc:
                _angRadStart = 0;
                _angRadEnd = 360;
                points.AddRange(GetCircle(_radius, _angRadStart, _angRadEnd));
                break;
            case DiscType.Pie:
                points.Add(Vector2.zero);
                points.AddRange(GetCircle(_radius, _angRadStart, _angRadEnd));
                break;
            case DiscType.Ring:
                _angRadStart = 0;
                _angRadEnd = 360;
                _thiccness /= 2;
                points.AddRange(GetCircle(_radius + _thiccness, _angRadStart, _angRadEnd));
                points.AddRange(GetCircle(_radius + -_thiccness, _angRadStart, _angRadEnd, false, true));
                break;
            case DiscType.Arc:
                _thiccness /= 2;
                points.AddRange(GetCircle(_radius + _thiccness, _angRadStart, _angRadEnd));
                points.AddRange(GetCircle(_radius + -_thiccness, _angRadStart, _angRadEnd, false, true));
                break;
            default:
                break;
        }

        poly.points = points.ToArray();
    }

    // Draws a circle/arc
    List<Vector2> GetCircle(float _radius, float _angRadStart, float _angRadEnd, bool flip = false, bool reverseDrawOrder = false)
    {
        List<Vector2> points = new List<Vector2>();
        // Account for negative from thiccness
        if (_radius < 0) { _radius = 0; }

        float angle = -_angRadStart;
        float arcLength = Mathf.Clamp(Mathf.Abs(_angRadEnd - _angRadStart), 0, 2*Mathf.PI);
        int direction = (int)Mathf.Sign(_angRadEnd - _angRadStart) * (flip==true?-1:1);
        
        int stepCount;
        if (flexible)
            stepCount = Mathf.Max(Mathf.FloorToInt((arcLength / qualityLevel) * (flexWithRadius==true && _radius>1?_radius:1)), 2);
        else
            stepCount = fixedCount;

        for (int i = 0; i <= stepCount; i++)
        {
            float x = Mathf.Sin(angle) * _radius;
            float y = Mathf.Cos(angle) * _radius;
            Vector2 point = new Vector2(x, y);
            //Rotate so 0 rad = right
            point = ShapesMath.Rotate90CW(point);
            points.Add(point);

            // Add angle step evenly spaced for length in arc
            angle += (arcLength / stepCount) * -direction;
        }

        // Reverse draw order for connecting inner/outer rings
        if (reverseDrawOrder)
            points.Reverse();

        return points;
    }
}
