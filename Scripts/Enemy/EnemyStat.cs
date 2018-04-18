using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    [SerializeField]
    private float m_hp;

    /// <summary>데미지를 입힌다.</summary>
    public void Hit(int damage)
    {
        m_hp -= damage;

        DieCheck();
    }

    /// <summary>죽음 체크</summary>
    private void DieCheck()
    {
        if (m_hp <= 0f)
            gameObject.SetActive(false);
    }
}
