using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Destroy_HitCheck2D : MonoBehaviour
{
    private Destroy_Hit m_destroy;

    private void Awake()
    {
        m_destroy = GetComponentInParent<Destroy_Hit>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 데미지 오브젝트일 경우에만 실행
        if (other.transform.tag.Equals(GameLibrary.String_Damage))
        {
            Damage damage = other.transform.GetComponentInParent<Damage>();

            m_destroy.Destroy(damage.HitType);
        }
    }
}
