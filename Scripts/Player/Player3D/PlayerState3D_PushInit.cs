using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PushInit : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        m_subController.MoveStopAll();

        // 싹 수정해야함(★)
        Vector3 nearHangPosition = m_playerManager.Hand.CurrentPushItem.GetHangPosition(transform.position);
        nearHangPosition.y = transform.position.y;
        transform.position = nearHangPosition;

        Vector3 directionToItem = m_playerManager.Hand.CurrentPushItem.transform.position - transform.position;
        directionToItem.y = 0f;
        m_subController.LookRotation(directionToItem.normalized);

        m_mainController.ChangeState3D(E_PlayerState3D.PushIdle);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
