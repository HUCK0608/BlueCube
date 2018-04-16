using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PutInit : PlayerState3D
{
    // 플레이어에서 던질 위치로의 방향
    private Vector3 directonToPutPosition;

    public override void InitState()
    {
        base.InitState();

        // 모든 속도 멈추기
        m_subController.MoveStopAll();

        // 던질 위치로의 방향 계산
        directonToPutPosition = m_playerManager.Hand.CurrentPickPutObject.GetPutPosition() - transform.position;
        directonToPutPosition.y = 0f;
        directonToPutPosition = directonToPutPosition.normalized;
    }

    private void Update()
    {
        // 회전
        m_subController.LerpRotation(directonToPutPosition);

        ChangeStates();
    }

    private void ChangeStates()
    {
        // 아이템 방향으로의 회전이 어느정도 완료되었다면 던지기 방식에 따라 상태 변경
        if(Vector3.Angle(m_subController.Forward, directonToPutPosition) < 5f)
        {
            // 던지기 방식 가져오기
            E_PutType putType = m_playerManager.Hand.CurrentPickPutObject.PutType;

            // 던지기 방식에 따른 상태 변경
            if (putType.Equals(E_PutType.Defalut))
                m_mainController.ChangeState3D(E_PlayerState3D.PutDefault);
            else if (putType.Equals(E_PutType.Throw))
                m_mainController.ChangeState3D(E_PlayerState3D.PutThrow);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
