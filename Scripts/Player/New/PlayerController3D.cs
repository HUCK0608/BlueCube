using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerController3D : MonoBehaviour
{
    private PlayerManager m_playerManager;
    private PlayerController m_mainController;

    private CheckGround m_checkGround;

    private Rigidbody m_rigidbody;
    public Rigidbody Rigidbody { get { return m_rigidbody; } }

    private Transform m_head;
    private Transform m_body;

    private int m_moveDirection;
    /// <summary>이동방향 (Forward = 0, Back = 1, Left = 2, Right = 3)</summary>
    public int MoveDirection { get { return m_moveDirection; } }

    private void Awake()
    {
        m_playerManager = GetComponentInParent<PlayerManager>();
        m_mainController = GetComponentInParent<PlayerController>();

        m_checkGround = GetComponent<CheckGround>();

        m_rigidbody = GetComponent<Rigidbody>();

        m_head = transform.Find("Head");
        m_body = transform.Find("Body");
    }

    private void Update()
    {
        m_mainController.IsGrounded = m_checkGround.Check();
        ApplyGravity();
    }

    // 땅이 아닐경우 중력을 적용한다.
    private void ApplyGravity()
    {
        if(!m_mainController.IsGrounded)
        {
            Vector3 newVelocity = m_rigidbody.velocity;
            newVelocity.y += m_playerManager.Stat.Gravity * Time.deltaTime;

            m_rigidbody.velocity = newVelocity;
        }
    }

    /// <summary>이동 방향 벡터를 반환. 입력이 없으면 (0, 0, 0)을 반환</summary>
    public Vector3 GetMoveDirection()
    {
        Vector3 moveDirection = Vector3.zero;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveX.Equals(0f) && moveZ.Equals(0f))
            return moveDirection;

        Vector3 rightKeyPos = Vector3.right + Vector3.back;
        Vector3 upKeyPos = Vector3.forward + Vector3.right;

        if (moveX > 0f)
            moveDirection += rightKeyPos;
        if (moveX < 0f)
            moveDirection += -rightKeyPos;
        if (moveZ > 0f)
            moveDirection += upKeyPos;
        if (moveZ < 0f)
            moveDirection += -upKeyPos;

        return moveDirection.normalized;
    }

    /// <summary>moveDirection방향으로 바라보는방향과 이동방향의 각도를 계산하여 정해진 속도로 이동</summary>
    public void Move(Vector3 lookDirection, Vector3 moveDirection)
    {
        // 현재 바라보는 방향에서 걷는방향의 각도를 구함
        float angle = Vector3.Angle(lookDirection, moveDirection);
        // 부호를 구하기 위한 연산
        Vector3 cross = Vector3.Cross(lookDirection, moveDirection);

        if (cross.y < 0) angle = -angle;

        // 바라보는 방향에서 걷는방향의 각도를 이용하여 걷기 애니메이션을 정함
        // Forward
        if (angle >= -50f && angle <= 50f)
            m_moveDirection = 0;
        // Left
        else if (angle > -130f && angle < -50f)
            m_moveDirection = 2;
        // Right
        else if (angle > 50f && angle < 130f)
            m_moveDirection = 3;
        // Back
        else
            m_moveDirection = 1;

        moveDirection.y = m_rigidbody.velocity.y;

        // 정면 이동일 경우 정면 속도로 이동
        if (m_moveDirection.Equals(0))
        {
            // 속도적용
            moveDirection *= m_playerManager.Stat.MoveSpeed_Forward;
        }
        // 정면이 아닌 이동일 경우 옆, 뒤 속도로 이동
        else
        {
            // 속도적용
            moveDirection *= m_playerManager.Stat.MoveSpeed_SideBack;
        }

        // y속도는 기존의 것을 사용
        moveDirection.y = m_rigidbody.velocity.y;

        m_rigidbody.velocity = moveDirection;
    }

    /// <summary>Rigidbody.velocity 를 Vector3.zero로 변경(y 속도는 변경하지 않음)</summary>
    public void MoveStop()
    {
        Vector3 newVelocity = Vector3.zero;
        newVelocity.y = m_rigidbody.velocity.y;

        m_rigidbody.velocity = newVelocity;
    }

    /// <summary>스탯에 있는 jumPower로 Rigidbody에 힘을 가함</summary>
    public void Jump()
    {
        m_rigidbody.AddForce(Vector3.up * m_playerManager.Stat.JumpPower, ForceMode.Impulse);
    }

    /// <summary>direction방향으로 머리 회전</summary>
    public void RotateHead(Vector3 direction)
    {
        m_head.rotation = Quaternion.LookRotation(direction);
    }

    /// <summary>direction방향으로 몸 회전</summary>
    public void RotateBody(Vector3 direction)
    {
        m_body.rotation = Quaternion.Slerp(m_body.rotation, Quaternion.LookRotation(direction), m_playerManager.Stat.RotationSpeed * Time.deltaTime);
    }

    /// <summary>direction방향으로 머리와 몸 회전</summary>
    public void RotateHeadAndBody(Vector3 direction)
    {
        RotateHead(direction);
        RotateBody(direction);
    }
}
