using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState_Long_Return : EnemyState
{
    public override void InitState()
    {

    }

    private void Update()
    {
        // 게임 시간이 멈춰있을경우 리턴
        if (GameLibrary.Bool_IsGameStop_Old)
            return;

        MoveToSpawnPosition();
    }

    // 스폰지점으로 이동
    private void MoveToSpawnPosition()
    {
        // 스폰지점으로의 방향
        Vector3 directionToSpawnPosition = m_enemyManager.Stat.SpawnPosition - transform.position;
        directionToSpawnPosition = directionToSpawnPosition.normalized;

        // 이동
        transform.position = Vector3.MoveTowards(transform.position, m_enemyManager.Stat.SpawnPosition, m_enemyManager.Stat.MoveSpeed * Time.deltaTime);
        // 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToSpawnPosition), m_enemyManager.Stat.RotationSpeed * Time.deltaTime);

        // 스폰지점으로 돌아가면 Idle 상태로 변경
        if (transform.position.Equals(m_enemyManager.Stat.SpawnPosition))
            m_enemyManager.ChangeState(E_EnemyState.Idle);
    }

    public override void EndState()
    {
    }
}
