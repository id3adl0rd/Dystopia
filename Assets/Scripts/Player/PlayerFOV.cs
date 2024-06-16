using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Other;

public class PlayerFOV : MonoBehaviour
{
    [SerializeField]private float _fov;

    [SerializeField] private int rayCount;

    [SerializeField] private float angle;

    [SerializeField] private float viewDistance;

    [SerializeField] private LayerMask layerMask;

    private Vector3 _origin;
    private Mesh _mesh;
    private float _startingAngle;

    private void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _origin = Vector3.zero;
    }

    private void LateUpdate()
    {
        float angle = _startingAngle;
        float angleIncrease = _fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = _origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++) {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(_origin, Other.Utils.GetVectorFromAngle(angle), viewDistance, layerMask);
            if (raycastHit2D.collider is null) {
                vertex = _origin + Other.Utils.GetVectorFromAngle(angle) * viewDistance;
            } else {
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }


        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
        _mesh.bounds = new Bounds(_origin, Vector3.one * 1000f);
    }

    public void SetOrigin(Vector3 _origin)
    {
        this._origin = _origin;
    }

    public void SetAimDirection(Vector3 aimDirection) {
        _startingAngle = Other.Utils.GetAngleFromVectorFloat(aimDirection) + _fov / 2f;
        Debug.Log(_startingAngle);
    }

    public void SetFoV(float fov) {
        this._fov = fov;
    }

    public void SetViewDistance(float viewDistance) {
        this.viewDistance = viewDistance;
    }
}
