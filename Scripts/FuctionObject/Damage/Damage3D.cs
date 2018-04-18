﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Damage3D : MonoBehaviour
{
    private Damage m_damage;

    private void Awake()
    {
        m_damage = GetComponentInParent<Damage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        m_damage.DoHit(other.transform.parent.gameObject);
    }
}
