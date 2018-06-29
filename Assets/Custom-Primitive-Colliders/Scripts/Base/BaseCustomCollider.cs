/*
Custom-Primitive-Colliders
Copyright (c) 2018 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPrimitiveColliders
{
    public abstract class BaseCustomCollider : MonoBehaviour
    {
        private MeshCollider m_meshCollider;

        protected MeshCollider meshCollider
        {
            get
            {
                if (m_meshCollider == null)
                {
                    m_meshCollider = GetComponent<MeshCollider>();

                    if (m_meshCollider == null)
                    {
                        m_meshCollider = gameObject.AddComponent<MeshCollider>();
                    }
                }
                return m_meshCollider;
            }
        }

        private PolygonCollider2D m_polygonCollider2d;

        protected PolygonCollider2D polygonCollider2d
        {
            get
            {
                if (m_polygonCollider2d == null)
                {
                    m_polygonCollider2d = GetComponent<PolygonCollider2D>();

                    if (m_polygonCollider2d == null)
                    {
                        m_polygonCollider2d = gameObject.AddComponent<PolygonCollider2D>();
                    }
                }
                return m_polygonCollider2d;
            }
        }
    }
}