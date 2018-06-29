/*
Custom-Primitive-Colliders
Copyright (c) 2018 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/
using UnityEngine;

namespace CustomPrimitiveColliders
{
    [AddComponentMenu("CustomPrimitiveColliders/2D/Triangle Collider 2D"), RequireComponent(typeof(PolygonCollider2D))]
    public class TriangleCollider2D : BaseCustomCollider
    {
        [SerializeField]
        private float m_radius = 0.5f;
        [SerializeField]
        private float m_length = 1f;
        [SerializeField]
        private bool m_useOpenAngle = false;
        [SerializeField, Range(1, 179)]
        private int m_openAngle = 45;

        private void Awake()
        {
            ReCreate(m_radius, m_length, m_useOpenAngle, m_openAngle);
        }

#if UNITY_EDITOR

        private void Reset()
        {
            ReCreate(m_radius, m_length, m_useOpenAngle, m_openAngle);
        }

        private void OnValidate()
        {
            ReCreate(m_radius, m_length, m_useOpenAngle, m_openAngle);
        }

#endif

        public void ReCreate(float radius, float length, bool useOpenAngle = false, int openAngle = 45)
        {
            Vector2[] points = CreatePoints(radius, length, useOpenAngle, openAngle);

            polygonCollider2d.points = null;
            polygonCollider2d.points = points;
        }

        private Vector2[] CreatePoints(float radius, float length, bool useOpenAngle, int openAngle)
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

            m_radius = radius;
            m_length = length;
            m_useOpenAngle = useOpenAngle;
            m_openAngle = openAngle;
            int numVertices = 4;

            Vector2[] points = new Vector2[numVertices];

            points[0] = Vector2.zero;
            points[1] = new Vector2(radius, length);
            points[2] = new Vector2(-radius, length);
            points[3] = Vector2.zero;

            return points;
        }
    }
}