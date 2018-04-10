using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerController2D : MonoBehaviour
{
    // 플레이어매니저 및 메인 컨트롤러
    private PlayerManager m_playerManager;
    private PlayerController m_mainController;

    // 땅 체크 스크립트
    private CheckGround2D m_checkGround;

    // 리지드바디
    private Rigidbody2D m_rigidbody;
    public Rigidbody2D Rigidbody { get { return m_rigidbody; } }

    // 캐릭터 속성부분

    /// <summary>캐릭터의 정면 방향을 반환. (1, 0, 0) 또는 (-1, 0, 0)</summary>
    public Vector3 Forward
    {
        get
        {
            Vector3 forward = Vector3.zero;
            forward.x = transform.localScale.x;
            return forward;
        }
    }

    private void Awake()
    {
        m_playerManager = GetComponentInParent<PlayerManager>();
        m_mainController = GetComponentInParent<PlayerController>();

        m_checkGround = GetComponent<CheckGround2D>();

        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>땅인지 체크를 한 후 땅이 아닐 경우 중력을 적용시킨다</summary>
    public void ApplyGravity()
    {
        // 땅인지 체크
        m_mainController.IsGrounded = m_checkGround.Check();

        // 땅이아닐경우 중력적용
        if(!m_mainController.IsGrounded)
        {
            Vector3 newVelocity = m_rigidbody.velocity;
            newVelocity.y += m_playerManager.Stat.Gravity * Time.deltaTime;

            m_rigidbody.velocity = newVelocity;
        }
        // 땅일 경우
        else
        {
            //y속도가 0아래일 경우에만 땅이랑 살짝 띄워줌
            if (m_rigidbody.velocity.y <= 0f)
            {
                Vector3 onGroundPosition = transform.position;
                onGroundPosition.y = m_checkGround.OnGroundPositionY;

                transform.position = onGroundPosition;
            }
        }
    }

    /// <summary>이동 방향 벡터를 반환. 입력이 없으면 (0, 0, 0)을 반환</summary>
    public Vector2 GetMoveDirection()
    {
        Vector2 moveDirection = Vector2.zero;

        float moveX = Input.GetAxis("Horizontal");
        float zero = 0f;

        if (moveX.Equals(zero))
            return moveDirection;

        if (moveX > zero)
            moveDirection += Vector2.right;
        if (moveX < zero)
            moveDirection += Vector2.left;

        return moveDirection.normalized;
    }

    /// <summary>direction방향으로 이동 및 회전을 함</summary>
    public void MoveAndRotate(Vector2 direction)
    {
        // 이동방향  * 속도 계산 및 y는 기존의 속도 사용
        Vector2 movement = direction * m_playerManager.Stat.MoveSpeed_Forward;
        movement.y = m_rigidbody.velocity.y;

        // 이동
        m_rigidbody.velocity = movement;

        // 이동입력이 없다면 리턴
        if (direction.Equals(Vector2.zero))
            return;

        // 스케일 변화로 회전값 구하기
        Vector3 scale = Vector3.one;
        scale.x = direction.x;

        // 스케일 변경으로 회전
        // 만약 스케일 값이 같지 않다면 스케일 변경
        if (!transform.localScale.x.Equals(scale.x))
            transform.localScale = scale;
    }

    /// <summary>moveDireciton방향으로 Stat에 MoveSpeed_Jump속도로 이동함</summary>
    public void JumpMoveAndRotate(Vector2 direction)
    {
        // 이동방향 * 속도 계산 및 y는 기존의 속도 사용
        Vector2 movement = direction * m_playerManager.Stat.MoveSpeed_Jump;
        movement.y = m_rigidbody.velocity.y;

        // 이동
        m_rigidbody.velocity = movement;

        if (direction.Equals(Vector2.zero))
            return;

        // 스케일 변화로 회전값 구하기
        Vector3 scale = Vector3.one;
        scale.x = direction.x;

        // 스케일 변경으로 회전
        // 만약 스케일 값이 같지 않다면 스케일 변경
        if (!transform.localScale.x.Equals(scale.x))
            transform.localScale = scale;
    }

    /// <summary>x 속도를 멈춤</summary>
    public void MoveStopX()
    {
        Vector2 newVelocity = Vector3.zero;
        newVelocity.y = m_rigidbody.velocity.y;

        m_rigidbody.velocity = newVelocity;
    }
    
    /// <summary>모든 속도를 멈춤</summary>
    public void MoveStopAll()
    {
        m_rigidbody.velocity = Vector2.zero;
    }

    /// <summary>스탯에 있는 jumpPower로 Rigidbody에 힘을 가함</summary>
    public void Jump()
    {
        Vector3 newVelocity = m_rigidbody.velocity;
        newVelocity.y = m_playerManager.Stat.JumpPower;
        m_rigidbody.velocity = newVelocity;
    }
}
