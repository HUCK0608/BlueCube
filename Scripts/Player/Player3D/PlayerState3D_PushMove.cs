using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PushMove : PlayerState3D
{
    private Item_Push m_currentPushItem;

    private Vector3 m_playerMovePositon;

    public override void InitState()
    {
        base.InitState();

        m_currentPushItem = m_playerManager.Hand.CurrentPushItem;

        // 플레이어 이동 위치 구하기
        Vector3 movePosition = m_subController.Body.forward * m_playerManager.Hand.CurrentPushItem.MoveDistance;
        m_playerMovePositon = transform.position + movePosition;

        // 상자 이동
        m_currentPushItem.PushItem(m_subController.Body.forward);
    }

    private void Update()
    {
        MovePlayer();

        ChangeStates();
    }

    // 플레이어 및 상자 이동
    private void MovePlayer()
    {
        // 밀기 속도 가져오기
        float moveSpeed_Push = m_playerManager.Stat.MoveSpeed_Push;

        // 플레이어 이동
        transform.position = Vector3.MoveTowards(transform.position, m_playerMovePositon, moveSpeed_Push * Time.deltaTime);
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 이동을 완료했을 경우 PushIdle 상태로 변경
        if (!m_currentPushItem.IsMove)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PushIdle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
