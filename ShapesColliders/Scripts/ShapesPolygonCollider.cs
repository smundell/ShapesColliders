
using UnityEngine;
using Shapes;

[ExecuteInEditMode]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Polygon))]
public class ShapesPolygonCollider : ShapesCollider
{
    Polygon pol;

    private void Start()
    {
        pol = GetComponent<Polygon>();
        UpdateData();
    }

    protected override void Refresh()
    {
        poly.points = pol.PolyPoints.ToArray();
    }

    private void OnDrawGizmosSelected()
    {
        UpdateData();
    }
}
