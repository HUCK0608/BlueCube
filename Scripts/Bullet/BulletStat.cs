using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BulletStat : MonoBehaviour
{
    // 총알 스피드
    private float m_speed;
    public float Speed { get { return m_speed; } set { m_speed = value; } }

    // 총알 데미지
    private float m_damage;
    public float Damage { get { return m_damage; } set { m_damage = value; } }
}
