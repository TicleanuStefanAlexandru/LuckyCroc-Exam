using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DetectionCircleRenderer : MonoBehaviour
{
    public float radius = 5f;
    public int segments = 64;

    void Start()
    {
        DrawCircle();
    }

    public void SetRadius(float r)
    {
        radius = r;
        DrawCircle();
    }
    
    void Update()
    {
        DrawCircle();
    }

    void DrawCircle()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.loop = true;
        lr.useWorldSpace = true;
        lr.positionCount = segments + 1;

        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = transform.position.x + Mathf.Cos(angle) * radius;
            float z = transform.position.z + Mathf.Sin(angle) * radius;
            float y = transform.position.y + 0.05f; // slightly above ground
            lr.SetPosition(i, new Vector3(x, y, z));
        }
    }

}
