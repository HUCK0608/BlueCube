using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerState3D_Move : PlayerState3D
{
    private Vector3 m_moveDirection;
    private Vector3 m_mouseDirectionToPlayer;
    private int m_aniMoveDirection;

    public override void InitState()
    {
        base.InitState();
    }

    protected override void Update()
    {
        base.Update();

        // 방향키 입력 방향을 가져옴
        m_moveDirection = m_subController.GetMoveDirection();
        // 플레이어에서 마우스의 방향을 가져옴
        m_mouseDirectionToPlayer = GameManager.Instance.CameraManager.GetMouseDirectionToPivot(transform.position);

        CheckIdleState();
        CalcAniMoveDirection();
        Move();
        Rotate();
    }

    // 애니메이션용 각도에 따른 방향 계산
    private void CalcAniMoveDirection()
    {
        // 현재 바라보는 방향에서 걷는방향의 각도를 구함
        float angle = Vector3.Angle(m_mouseDirectionToPlayer, m_moveDirection);
        // 부호를 구하기 위한 연산
        Vector3 cross = Vector3.Cross(m_mouseDirectionToPlayer, m_moveDirection);

        // 부호
        if (cross.y < 0) angle = -angle;

        // 바라보는 방향에서 걷는방향의 각도를 이용하여 걷기 애니메이션을 정함
        // Forward
        if (angle >= -50f && angle <= 50f)
            m_aniMoveDirection = 0;
        // Left
        else if (angle > -130f && angle < -50f)
            m_aniMoveDirection = 2;
        // Right
        else if (angle > 50f && angle < 130f)
            m_aniMoveDirection = 3;
        // Back
        else
            m_aniMoveDirection = 1;

        // 이 부분에 애니메이션 설정 넣어야함
    }

    // Idle 상태로 바뀔지 체크
    private void CheckIdleState()
    {
        if (m_moveDirection.Equals(Vector3.zero))
            m_mainController.ChangeState3D(E_PlayerState.Idle);
    }

    // 이동
    private void Move()
    {
        // 정면 이동일 경우 정면 속도로 이동
        if (m_aniMoveDirection.Equals(0))
        {
            m_subController.Move(m_moveDirection, m_playerManager.Stat.MoveSpeed_Forward);
        }
        // 정면이 아닌 이동일 경우 옆, 뒤 속도로 이동
        else
        {
            m_subController.Move(m_moveDirection, m_playerManager.Stat.MoveSpeed_SideBack);
        }
    }

    // 머리와 몸 회전
    private void Rotate()
    {
        // 정면 이동일 경우 머리는 마우스방향을 바라보고 몸은 이동방향으로 바라봄
        if(m_aniMoveDirection.Equals(0))
        {
            m_subController.RotateHead(m_mouseDirectionToPlayer);
            m_subController.RotateBody(m_moveDirection);
        }
        // 정면 이동이 아닐경우 마우스 방향을 바라봄
        else
        {
            m_subController.RotateHeadAndBody(m_mouseDirectionToPlayer);
        }

    }

    public override void EndState()
    {
        base.EndState();
    }
}
