using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState_Monster1_Attack : EnemyState_Monster1
{
    private Transform m_player;

    private bool m_isHitDelay;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitState()
    {
        if (m_player == null)
            m_player = GameManager.Instance.PlayerManager.Player3D_GO.transform;
    }

    private void Update()
    {
        // 게임시간이 멈춘경우 리턴
        if (GameLibrary.Bool_IsCOV2D)
            return;

        CheckHitRange();
    }

    // 피격가능 범위인지 체크
    private void CheckHitRange()
    {
        // 공격 딜레이중이라면 리턴
        if (m_isHitDelay)
            return;

        // 몬스터에서 플레이어까지 거리
        float distanceToPlayer = Vector3.Distance(transform.position, m_player.position);

        // 피격가능 범위 안이면 플레이어를 공격
        if (distanceToPlayer <= m_enemyManager.Stat.HitRange)
        {
            // 플레이어 피격
            GameManager.Instance.PlayerManager.Stat.Hit(m_enemyManager.Stat.Damage);
            // 딜레이 타이머 코루틴 시작
            StartCoroutine(OnHitDelayTimer());
        }
        // 피격가능 범위 밖이면 Chase State로 변경
        else
        {
            m_enemyManager.ChangeState(E_EnemyState_Monster1.Chase);
        }
    }

    // 공격 딜레이 타이머
    private IEnumerator OnHitDelayTimer()
    {
        m_isHitDelay = true;

        float addTime = 0f;
        float hitDelay = m_enemyManager.Stat.HitDelayTime;

        while(true)
        {
            addTime += Time.deltaTime;

            if (addTime >= hitDelay)
                break;

            yield return null;
        }

        m_isHitDelay = false;
    }

    public override void EndState()
    {

    }
}
