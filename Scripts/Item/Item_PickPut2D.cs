using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_PickPut2D : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;

    private void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void OnGravity()
    {
        m_rigidbody2D.gravityScale = 1f;
    }

    public void OffGravity()
    {
        m_rigidbody2D.gravityScale = 0;
    }
}
