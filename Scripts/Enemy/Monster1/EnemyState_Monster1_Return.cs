using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState_Monster1_Return : EnemyState_Monster1
{
    private Vector3 m_spawnPosition;

    protected override void Awake()
    {
        base.Awake();
        m_spawnPosition = transform.position;
    }

    public override void InitState()
    {

    }

    private void Update()
    {
        if (GameLibrary.Bool_IsGameStop)
            return;

        MoveToSpawnPosition();
    }

    // 스폰위치로 회전 및 좌표이동을 하며 이동
    private void MoveToSpawnPosition()
    {
        // 회전
        Vector3 direcitonToSpawnPosition = m_spawnPosition - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direcitonToSpawnPosition), m_enemyManager.Stat.ReturnRotationSpeed * Time.deltaTime);

        // 이동
        transform.position = Vector3.MoveTowards(transform.position, m_spawnPosition, m_enemyManager.Stat.ReturnMoveSpeed * Time.deltaTime);

        // 스폰위치로 돌아갔을 경우 Idle State로 변경
        if (transform.position.Equals(m_spawnPosition))
            m_enemyManager.ChangeState(E_EnemyState_Monster1.Idle);
    }

    public override void EndState()
    {

    }
}
