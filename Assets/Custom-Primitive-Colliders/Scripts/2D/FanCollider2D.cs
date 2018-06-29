/*
Custom-Primitive-Colliders
Copyright (c) 2018 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/
using UnityEngine;

namespace CustomPrimitiveColliders
{
    [AddComponentMenu("CustomPrimitiveColliders/2D/Fan Collider 2D"), RequireComponent(typeof(PolygonCollider2D))]
    public class FanCollider2D : BaseCustomCollider
    {
        [SerializeField]
        private float m_radius = 1f;
        [SerializeField, Range(1, 360)]
        private int m_fanAngle = 135;
        [SerializeField]
        private int m_numVertices = 32;

        private void Awake()
        {
            ReCreate(m_radius, m_fanAngle, m_numVertices);
        }

#if UNITY_EDITOR

        private void Reset()
        {
            ReCreate(m_radius, m_fanAngle, m_numVertices);
        }

        private void OnValidate()
        {
            ReCreate(m_radius, m_fanAngle, m_numVertices);
        }

#endif

        public void ReCreate(float radius, int fanAngle, int numVertices = 32)
        {
            Vector2[] points = CreatePoints(radius, fanAngle, numVertices);

            polygonCollider2d.points = null;
            polygonCollider2d.points = points;
        }

        private Vector2[] CreatePoints(float radius, int fanAngle, int numVertices)
        {
            if (radius <= 0f)
            {
                radius = 0.01f;
            }

            fanAngle = Mathf.Clamp(fanAngle, 1, 360);

            if (numVertices < 4)
            {
                numVertices = 4;
            }

            m_radius = radius;
            m_fanAngle = fanAngle;
            m_numVertices = numVertices;

            Vector2[] points = new Vector2[numVertices + (fanAngle == 360 ? 2 : 1)];

            Quaternion quatStep = Quaternion.Euler(0f, 0f, fanAngle / (float)(fanAngle == 360 ? m_numVertices : (numVertices - 1)));

            points[0] = Vector2.zero;

            for (int i = 1; i <= numVertices; i++)
            {
                if (i == 1)
                {
                    points[i] = new Vector2(radius, 0f);
                }
                else
                {
                    points[i] = quatStep * points[i - 1];
                }
            }

            if (fanAngle == 360)
            {
                points[points.Length - 1] = points[1];
            }

            Vector2 meshForward;
            int centerIndex = Mathf.FloorToInt(numVertices / 2);
            if (numVertices % 2 == 0)
            {
                meshForward = (points[centerIndex] - points[0]) + (points[centerIndex + 1] - points[0]);
            }
            else
            {
                meshForward = points[centerIndex + 1] - points[0];
            }

            Quaternion quat = Quaternion.FromToRotation(meshForward, Vector3.up);
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = quat * points[i];
            }

            return points;
        }
    }
}