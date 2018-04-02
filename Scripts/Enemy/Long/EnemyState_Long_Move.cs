using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState_Long_Move : EnemyState
{
    private Transform m_player;

    private Ray m_ray;

    protected override void Awake()
    {
        base.Awake();

        m_player = PlayerManager.Instance.Player3D_Object.transform;

        m_ray = new Ray();
    }

    public override void InitState()
    {
    }

    private void Update()
    {
        // 게임 시간이 멈춰있을경우 리턴
        if (GameLibrary.Bool_IsGameStop_Old)
            return;

        // 탐지범위를 벗어나면 Return 상태로 변경
        if (!m_enemyManager.Stat.DetectionArea.CheckDetected(m_player.position))
            m_enemyManager.ChangeState(E_EnemyState.Return);

        // 재장전을 완료하면 Attack 상태로 변경
        if (!m_enemyManager.Weapon.IsReload)
        {
            m_enemyManager.ChangeState(E_EnemyState.Attack);
        }

        MoveAndRotation();
    }

    // 이동 및 회전
    private void MoveAndRotation()
    {
        // 플레이어로의 방향
        Vector3 directionToPlayer = m_player.position - transform.position;
        directionToPlayer.y = 0f;
        directionToPlayer = directionToPlayer.normalized;

        // 플레이어와의 거리
        float distanceToPlayer = Vector3.Distance(transform.position, m_player.position);

        // 최대한 가까이 갈 수 있는 거리만큼 이동
        if (distanceToPlayer > m_enemyManager.Stat.MaxNearDistance)
        {
            // 정면에 무엇이 없을경우 이동
            if(!CheckForward())
                transform.Translate(Vector3.forward * m_enemyManager.Stat.MoveSpeed * Time.deltaTime);
        }

        // 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), m_enemyManager.Stat.RotationSpeed * Time.deltaTime);
    }

    // 정면에 무엇이 있는지 체크
    private bool CheckForward()
    {
        bool checkForward = false;

        m_ray.origin = transform.position;
        m_ray.direction = transform.forward;

        float rayDistance = 1.5f;

        if(Physics.Raycast(m_ray, rayDistance, GameLibrary.LayerMask_Ignore_RBP))
        {
            checkForward = true;
        }

        return checkForward;
    }

    public override void EndState()
    {
    }
}
