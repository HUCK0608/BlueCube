using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState_Monster1_Chase : EnemyState_Monster1
{
    private Transform m_player;

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
        FollowPlayer();
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

    public override void EndState()
    {

    }
}
