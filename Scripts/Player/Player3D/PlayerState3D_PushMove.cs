using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PushMove : PlayerState3D
{
    Transform m_currentPushItem;

    private Vector3 m_playerMovePositon;
    private Vector3 m_itemMovePosition;

    public override void InitState()
    {
        base.InitState();

        m_currentPushItem = m_playerManager.Hand.CurrentPushItem.transform;

        // 플레이어 및 아이템 이동 위치 구하기
        Vector3 movePosition = m_subController.Body.forward * m_playerManager.Hand.CurrentPushItem.MoveDistance;
        m_itemMovePosition = m_currentPushItem.position + movePosition;
        m_playerMovePositon = transform.position + movePosition;
    }

    private void Update()
    {
        MovePlayerAndItem();

        ChangeStates();
    }

    // 플레이어 및 상자 이동
    private void MovePlayerAndItem()
    {
        // 밀기 속도 가져오기
        float moveSpeed_Push = m_playerManager.Stat.MoveSpeed_Push;

        // 상자 및 플레이어 이동
        m_currentPushItem.position = Vector3.MoveTowards(m_currentPushItem.position, m_itemMovePosition, moveSpeed_Push * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, m_playerMovePositon, moveSpeed_Push * Time.deltaTime);
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        // 이동을 완료했을 경우 PushIdle 상태로 변경
        if (m_currentPushItem.position.Equals(m_itemMovePosition))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.PushIdle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
