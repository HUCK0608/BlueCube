using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BulletStat : MonoBehaviour
{
    // 총알 스피드
    [SerializeField]
    private float m_speed;
    public float Speed { get { return m_speed; }}

    // 총알 데미지
    [SerializeField]
    private int m_damage;
    public int Damage { get { return m_damage; }}

    // 총알 지속시간
    [SerializeField]
    private float m_durationTime;
    public float DurationTime { get { return m_durationTime; } }
}
