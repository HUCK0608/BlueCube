using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ColliderCheck2D : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;
    private Collider2D m_collider2D;

    private Ray m_ray;
    private int m_layerMask;

    private void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<Collider2D>();

        m_layerMask = (-1) + (1 << 8);
    }

    private void Update()
    {
        m_ray = new Ray(transform.position, Vector3.back);

        if(Physics.Raycast(m_ray, 1000f))
        {
        }
    }
}
