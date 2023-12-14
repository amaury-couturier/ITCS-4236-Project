using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

    [SerializeField] private GuardController guardController;

    private Mesh flashMesh;
    [SerializeField] private LayerMask ObstructionMask;

    private Vector3 origin;
    private float startingAngle;
    public float StartingAngle { get { return startingAngle; } }
    private float fov;

    // Start is called before the first frame update
    void Start()
    {
        flashMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = flashMesh;
        origin = Vector3.zero;
        fov = 90f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        float viewDistance = 50f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D flash = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, ObstructionMask);
            if (flash.collider == null)
            {
                //No hit
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                //Hit
                vertex = flash.point;
            }
            vertices[vertexIndex] = vertex;
            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;

            angle -= angleIncrease;
        }

        flashMesh.vertices = vertices;
        flashMesh.uv = uv;
        flashMesh.triangles = triangles;
        flashMesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);

        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 Direction)
    {
        Direction = Direction.normalized;
        float n = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) - fov / 2;
    }
}
