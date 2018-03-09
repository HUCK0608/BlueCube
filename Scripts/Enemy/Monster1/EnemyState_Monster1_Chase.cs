using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState_Monster1_Chase : EnemyState_Monster1
{
    private Transform m_player;

    private Vector3 m_spawnPosition;

    protected override void Awake()
    {
        base.Awake();
        m_spawnPosition = transform.position;
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

        FollowPlayer();
        CheckChaseRange();
        CheckAttackRange();
    }

    // 회전 및 좌표이동을하며 플레이어를 쫓아감
    private void FollowPlayer()
    {
        // 회전
        Vector3 directionToPlayer = m_player.position - transform.position;
        directionToPlayer.y = 0f;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer.normalized), m_enemyManager.Stat.RotationSpeed);

        // 이동
        Vector3 movePosition = m_player.transform.position;
        movePosition.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, movePosition, m_enemyManager.Stat.MoveSpeed * Time.deltaTime);
    }

    // 스폰위치에서 추적범위 밖까지 가면 Return State로 변경
    private void CheckChaseRange()
    {
        float distanceToSpawnPosition = Vector3.Distance(transform.position, m_spawnPosition);

        if (distanceToSpawnPosition >= m_enemyManager.Stat.ChaseRange)
            m_enemyManager.ChangeState(E_EnemyState_Monster1.Return);
    }

    // 공격범위안에 들어오면 Attack State로 변경
    private void CheckAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, m_player.position);

        if (distanceToPlayer <= m_enemyManager.Stat.AttackRange)
            m_enemyManager.ChangeState(E_EnemyState_Monster1.Attack);
    }

    public override void EndState()
    {

    }
}
