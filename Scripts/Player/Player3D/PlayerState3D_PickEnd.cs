using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_PickEnd : PlayerState3D
{
    private bool m_isPut;

    public override void InitState()
    {
        base.InitState();

        // x, z속도 멈춤
        m_subController.MoveStopXZ();

        StartCoroutine(PutItem());
    }

    private IEnumerator PutItem()
    {
        m_isPut = false;

        // 현재 들고있는 아이템
        Item_PickPut currentPickItem = m_playerManager.Hand.CurrentPickItem;

        // 회전방향 구하기
        Vector3 directionToPutPosition = currentPickItem.CurrentPutPosition - transform.position;
        directionToPutPosition.y = 0f;

        float checkAngle = 5f;

        while (true)
        {
            // 회전
            m_subController.LerpRotation(directionToPutPosition);

            // 회전이 어느정도 완료되었다면 반복문 종료
            if (Vector3.Angle(directionToPutPosition, m_subController.Forward) < checkAngle)
                break;

            yield return null;
        }

        // 아이템 놓기
        currentPickItem.PutItem();

        m_isPut = true;
    }

    private void Update()
    {
        // 상태 변경
        ChangeStates();
    }

    private void ChangeStates()
    {
        // 애니메이션과 아이템 모두 놓는 동작이 끝나면 Idle 상태로 변경
        if(m_isPut && m_playerManager.Hand.CurrentPickItem.IsPut)
        {
            m_mainController.ChangeState3D(E_PlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();
    }
}
