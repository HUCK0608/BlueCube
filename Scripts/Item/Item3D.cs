using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item3D : MonoBehaviour
{
    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void OnGravity()
    {
        m_rigidbody.useGravity = true;
    }

    public void OffGravity()
    {
        m_rigidbody.useGravity = false;
    }
}
