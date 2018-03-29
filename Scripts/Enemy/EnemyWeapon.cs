using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    protected EnemyManager m_enemyManager;

    private bool m_isAttack;
    public bool IsAttack { get { return m_isAttack; } }

    private bool m_isReload;
    public bool IsReload { get { return m_isReload; } }

    // 아래 3개 다 구조화 해서 다시 짜야함(★)
    [SerializeField]
    protected int m_bulletDamage;

    [SerializeField]
    protected float m_bulletSpeed;

    [SerializeField]
    protected float m_bulletDurationTime;

    // 선딜레이
    [SerializeField]
    private float m_firstDelay;

    // 후딜레이
    [SerializeField]
    private float m_lastDelay;

    // 재장전 시간
    [SerializeField]
    private float m_reloadDelay;

    protected virtual void Awake()
    {
        m_enemyManager = GetComponent<EnemyManager>();
    }

    // 공격
    public void Attack()
    {
        StartCoroutine(StartAttackOrder());
    }

    // 공격 순서 시작
    private IEnumerator StartAttackOrder()
    {
        m_isAttack = true;
        m_isReload = true;
        yield return StartCoroutine(GameLibrary.Timer(m_firstDelay));
        UseWeapon();
        yield return StartCoroutine(GameLibrary.Timer(m_lastDelay));
        m_isAttack = false;
        yield return StartCoroutine(GameLibrary.Timer(m_reloadDelay));
        m_isReload = false;
    }

    // 무기사용
    protected virtual void UseWeapon()
    {
    }
}
