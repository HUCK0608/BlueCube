using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DamageDuration : MonoBehaviour
{
    private Collider m_collider;
    private Collider2D m_collider2D;

    [SerializeField]
    private float m_durationTime;

    private void Awake()
    {
        m_collider = GetComponentInChildren<Collider>();
        m_collider2D = GetComponentInChildren<Collider2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyCollider());
    }

    private IEnumerator DestroyCollider()
    {
        yield return new WaitForSeconds(m_durationTime);

        m_collider.enabled = false;
        m_collider2D.enabled = false;
    }
}
