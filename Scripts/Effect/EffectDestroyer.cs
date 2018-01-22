using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EffectDestroyer : MonoBehaviour
{
    [SerializeField]
    private float m_destroyTime;

    private float m_addTime;

    private void Update()
    {
        m_addTime += Time.deltaTime;

        if (m_addTime >= m_destroyTime)
            Destroy(gameObject);
    }
}
