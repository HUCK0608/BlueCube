using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Damage2D : MonoBehaviour
{
    private Damage m_damage;

    private void Awake()
    {
        m_damage = GetComponentInParent<Damage>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        m_damage.DoHit(other.transform.parent.gameObject);
    }
}
