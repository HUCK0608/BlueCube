using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState_Long_Attack : EnemyState
{
    private Transform m_player;

    protected override void Awake()
    {
        base.Awake();

        m_player = PlayerManager.Instance.Player3D_Object.transform;
    }

    public override void InitState()
    {
        // 공격
        m_enemyManager.Weapon.Attack();
    }

    private void Update()
    {
        // 게임 시간이 멈춰있을경우 리턴
        if (GameLibrary.Bool_IsGameStop_Old)
            return;

        // 공격이 끝났을 때 탐지범위를 벗어나있으면 Return 상태로 변경
        if (!m_enemyManager.Weapon.IsAttack && !m_enemyManager.Stat.DetectionArea.CheckDetected(m_player.position))
            m_enemyManager.ChangeState(E_EnemyState.Return);

        // 공격이끝나면 Move 상태로 변경
        if (!m_enemyManager.Weapon.IsAttack)
            m_enemyManager.ChangeState(E_EnemyState.Move);

        Rotation();
    }

    private void Rotation()
    {
        Vector3 directionToPlayer = m_player.position - transform.position;
        directionToPlayer.y = 0f;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer.normalized), m_enemyManager.Stat.RotationSpeed * Time.deltaTime);
    }

    public override void EndState()
    {
    }
}
