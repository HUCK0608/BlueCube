using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Idle : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();
    }

    private void Update()
    {
        // 시점변환중이거나 탐지시점이면 return
        if (GameLibrary.Bool_IsCO)
            return;

        // 플레이어에서 마우스의 방향을 가져옴
        Vector3 mouseDirectionToPlayer = GameManager.Instance.CameraManager.GetMouseDirectionToPivot(transform.position);

        // 머리, 몸 회전
        m_subController.RotateHeadAndBody(mouseDirectionToPlayer);

        ChangeMoveState();
        ChangeAttackState();
        ChangeJumpUpState();
    }

    // Move 상태로 바뀔지 체크
    private void ChangeMoveState()
    {
        // 키 입력에 따른 이동 방향 벡터를 가져옴
        Vector3 movement = m_subController.GetMoveDirection();

        // 이동이 있을 경우 Move 상태로 변경
        if (movement != Vector3.zero)
            m_mainController.ChangeState3D(E_PlayerState.Move);
    }

    // Attack 상태로 바뀔지 체크
    private void ChangeAttackState()
    {
        // 공격키를 눌렀을 때
        if (Input.GetKeyDown(m_playerManager.Stat.AttackKey))
            // 무기를 사용할 수 있다면 Attack 상태로 변경
            if(m_playerManager.PlayerWeapon.CanUse)
                m_mainController.ChangeState3D(E_PlayerState.Attack);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
