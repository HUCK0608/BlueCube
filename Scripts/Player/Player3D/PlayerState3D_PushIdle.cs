using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PushIdle : PlayerState3D
{
    // 현재 상호작용중인 밀기 아이템
    private Item_Push m_currentPushItem;

    // 아이템쪽으로의 방향
    private Vector3 m_directionToItem;

    public override void InitState()
    {
        base.InitState();

        m_currentPushItem = m_playerManager.Hand.CurrentPushItem;

        // 플레이어에서 아이템으로의 방향
        m_directionToItem = m_currentPushItem.transform.position - transform.position;
        m_directionToItem.y = 0f;
        m_directionToItem =  m_directionToItem.normalized;
    }
    
    private void Update()
    {
        ChangeStates();
    }

    // 상태 변경 모음
    private void ChangeStates()
    {
        Vector3 moveDirection = m_subController.GetMoveDirection();

        // 상호작용 키를 누르거나 시점변환 키를 누를경우 Idle 상태로 변경
        if(Input.GetKeyDown(m_playerManager.Stat.InteractionKey) || Input.GetKeyDown(m_playerManager.Stat.ChangeViewKey))
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
        // 이동입력을 받을 때 미는 방향이 일치하고 밀 방향에 아무것도 없을경우 PushMove 상태로 변경
        else if(!moveDirection.Equals(Vector3.zero) && Vector3.Angle(moveDirection, m_directionToItem).Equals(0f))
        {
            if (m_currentPushItem.IsCanMove(moveDirection))
                m_mainController.ChangeState3D(E_PlayerState3D.PushMove);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
