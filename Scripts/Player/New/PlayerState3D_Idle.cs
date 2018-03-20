using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Idle : PlayerState3D
{
    public override void InitState()
    {
        base.InitState();
    }

    protected override void Update()
    {
        base.Update();

        // 플레이어에서 마우스의 방향을 가져옴
        Vector3 mouseDirectionToPlayer = GameManager.Instance.CameraManager.GetMouseDirectionToPivot(transform.position);

        CheckMoveState();
        // 회전
        m_subController.RotateHeadAndBody(mouseDirectionToPlayer);

        if(Input.GetKeyDown(KeyCode.Mouse0))
            m_mainController.PlayerWeapon.ShootFireBall3D(mouseDirectionToPlayer);
    }

    // Move 상태로 바뀔지 체크
    private void CheckMoveState()
    {
        // 키 입력에 따른 이동 방향 벡터를 가져옴
        Vector3 movement = m_subController.GetMoveDirection();

        // 이동이 있을 경우 Move 상태로 변경
        if (movement != Vector3.zero)
            m_mainController.ChangeState3D(E_PlayerState.Move);
    }

    public override void EndState()
    {
        base.EndState();
    }
}
