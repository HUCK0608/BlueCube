using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    [SerializeField]
    protected int m_hp;
    public int Hp { get { return m_hp; } }

    /// <summary>데미지를 입힌다.</summary>
    public void Hit(int damage)
    {
        m_hp -= damage;
    }
}
