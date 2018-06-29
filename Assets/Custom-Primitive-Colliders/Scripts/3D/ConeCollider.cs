/*
Custom-Primitive-Colliders
Copyright (c) 2018 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/
using System.Text;
using UnityEngine;

namespace CustomPrimitiveColliders
{
    [AddComponentMenu("CustomPrimitiveColliders/3D/Cone Collider"), RequireComponent(typeof(MeshCollider))]
    public class ConeCollider : BaseCustomCollider
    {
        [SerializeField]
        private float m_radius = 0.5f;
        [SerializeField]
        private float m_length = 1f;
        [SerializeField]
        private bool m_useOpenAngle = false;
        [SerializeField, Range(1, 179)]
        private int m_openAngle = 45;
        [SerializeField]
        private int m_numVertices = 32;

        private void Awake()
        {
            ReCreate(m_radius, m_length, m_useOpenAngle, m_openAngle, m_numVertices);
        }

#if UNITY_EDITOR

        private void Reset()
        {
            ReCreate(m_radius, m_length, m_useOpenAngle, m_openAngle, m_numVertices);
        }

        private void OnValidate()
        {
            ReCreate(m_radius, m_length, m_useOpenAngle, m_openAngle, m_numVertices);
        }

#endif

        public void ReCreate(float radius, float length, bool useOpenAngle = false, int openAngle = 45, int numVertices = 32)
        {
            Mesh mesh = CreateMesh(radius, length, useOpenAngle, openAngle, numVertices);

            if (meshCollider.sharedMesh != null)
            {
                meshCollider.sharedMesh.Clear();
                if (Application.isPlaying)
                {
                    Destroy(meshCollider.sharedMesh);
                }
                else
                {
                    DestroyImmediate(meshCollider.sharedMesh);
                }

            }

            meshCollider.sharedMesh = mesh;
        }

        private Mesh CreateMesh(float radius, float length, bool useOpenAngle, int openAngle, int numVertices)
        {
            if (radius <= 0f)
            {
                radius = 0.01f;
            }

            if (length <= 0f)
            {
                length = 0.01f;
            }

            openAngle = Mathf.Clamp(openAngle, 1, 179);

            if (useOpenAngle)
            {
                radius = length * Mathf.Tan(openAngle * Mathf.Deg2Rad / 2f);
            }

            if (numVertices < 4)
            {
                numVertices = 4;
            }

            m_radius = radius;
            m_length = length;
            m_useOpenAngle = useOpenAngle;
            m_openAngle = openAngle;
            m_numVertices = numVertices;

            Mesh mesh = new Mesh();

#if UNITY_EDITOR
            StringBuilder sbName = new StringBuilder("Cone");
            sbName.Append(numVertices);
            sbName.Append("_radius_");
            sbName.Append(radius);
            sbName.Append("_length_");
            sbName.Append(length);
            mesh.name = sbName.ToString();
#endif

            Vector3[] vertices = new Vector3[numVertices * 3];
            Vector3[] normals = new Vector3[numVertices * 3];
            Vector2[] uvs = new Vector2[numVertices * 3];
            int[] triangles = new int[numVertices * 6];

            float slope = Mathf.Atan(radius / length);
            float slopeSin = Mathf.Sin(slope);
            float slopeCos = Mathf.Cos(slope);

            int triangleCount = 0;

            for (int i = 0; i < numVertices; i++)
            {
                float angle = 2f * Mathf.PI * i / numVertices;
                float angleSin = Mathf.Sin(angle);
                float angleCos = Mathf.Cos(angle);
                float angleHalf = 2f * Mathf.PI * (i + 0.5f) / numVertices;
                float angleHalfSin = Mathf.Sin(angleHalf);
                float angleHalfCos = Mathf.Cos(angleHalf);

                vertices[i] = Vector3.zero;
                vertices[i + numVertices] = new Vector3(radius * angleCos, radius * angleSin, length);
                vertices[i + numVertices * 2] = new Vector3(0, 0, length);

                normals[i] = new Vector3(angleHalfCos * slopeCos, angleHalfSin * slopeCos, -slopeSin);
                normals[i + numVertices] = new Vector3(angleHalfCos * slopeCos, angleHalfSin * slopeCos, -slopeSin);
                normals[i + numVertices * 2] = Vector3.forward;

                uvs[i] = new Vector2(i / (float)numVertices, 1f);
                uvs[i + numVertices] = new Vector2(i / (float)numVertices, 0f);
                uvs[i + numVertices * 2] = new Vector2(0.5f, 0.5f);

                triangles[triangleCount++] = i + numVertices;
                triangles[triangleCount++] = i;
                triangles[triangleCount++] = i == numVertices - 1 ? numVertices : i + numVertices + 1;

                triangles[triangleCount++] = i + numVertices;
                triangles[triangleCount++] = i == numVertices - 1 ? numVertices : i + numVertices + 1;
                triangles[triangleCount++] = i + numVertices * 2;
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            return mesh;
        }
    }
}