using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState3D : PlayerState
{
    protected Rigidbody m_rigidBody;

    protected override void Awake()
    {
        base.Awake();
        m_rigidBody = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        base.Update();
    }
}
