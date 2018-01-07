using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_PickPut3D : MonoBehaviour
{
    private Rigidbody m_rigidbody;

    private int m_colAmount;

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
