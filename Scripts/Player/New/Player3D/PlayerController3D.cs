using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerController3D : MonoBehaviour
{
    // 플레이어 매니저 및 메인 컨트롤러
    private PlayerManager m_playerManager;
    private PlayerController m_mainController;

    // 땅 체크 스크립트
    private CheckGround3D m_checkGround;
    public CheckGround3D CheckGround { get { return m_checkGround; } }
    // 사다리 체크 스크립트
    private CheckLadder m_checkLadder;
    public CheckLadder CheckLadder { get { return m_checkLadder; } }

    // 리지드바디
    private Rigidbody m_rigidbody;
    public Rigidbody Rigidbody { get { return m_rigidbody; } }

    // 머리, 몸
    private Transform m_head;
    private Transform m_body;
    public Transform Body { get { return m_body; } }

    private int m_moveDirection;
    /// <summary>이동방향 (Forward = 0, Back = 1, Left = 2, Right = 3)</summary>
    public int MoveDirection { get { return m_moveDirection; } }

    private Ladder m_currentLadder;
    /// <summary>현재 사용중인 사다리</summary>
    public Ladder CurrentLadder { get { return m_currentLadder; } set { m_currentLadder = value; } }

    private void Awake()
    {
        m_playerManager = GetComponentInParent<PlayerManager>();
        m_mainController = GetComponentInParent<PlayerController>();

        m_checkGround = GetComponent<CheckGround3D>();
        m_checkLadder = GetComponent<CheckLadder>();

        m_rigidbody = GetComponent<Rigidbody>();

        m_head = transform.Find("Head");
        m_body = transform.Find("Body");
    }

    /// <summary>땅인지 체크를 한 후 땅이 아닐 경우 중력을 적용시키며 땅일경우 땅이랑 살짝 띄워준다</summary>
    public void ApplyGravity()
    {
        // 땅인지 체크
        m_mainController.IsGrounded = m_checkGround.Check();

        // 땅이아닐경우 중력 적용
        if(!m_mainController.IsGrounded)
        {
            Vector3 newVelocity = m_rigidbody.velocity;
            newVelocity.y += m_playerManager.Stat.Gravity * Time.deltaTime;

            m_rigidbody.velocity = newVelocity;
        }
        // 땅일 경우
        else
        {
            // y속도가 0아래일경우에만 땅이랑 살짝 띄워줌
            if (m_rigidbody.velocity.y <= 0f)
            {
                Vector3 onGroundPosition = transform.position;
                onGroundPosition.y = m_checkGround.OnGroundPositionY;

                transform.position = onGroundPosition;
            }
        }
    }

    /// <summary>이동 방향 벡터를 반환. 입력이 없으면 (0, 0, 0)을 반환</summary>
    public Vector3 GetMoveDirection()
    {
        Vector3 moveDirection = Vector3.zero;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float zero = 0f;

        if (moveX.Equals(zero) && moveZ.Equals(zero))
            return moveDirection;

        Vector3 rightKeyPos = Vector3.right + Vector3.back;
        Vector3 upKeyPos = Vector3.forward + Vector3.right;

        if (moveX > zero)
            moveDirection += rightKeyPos;
        if (moveX < zero)
            moveDirection += -rightKeyPos;
        if (moveZ > zero)
            moveDirection += upKeyPos;
        if (moveZ < zero)
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

    /// <summary>moveDirection방향으로 Stat에 MoveSpeed_Ladder속도로 이동함</summary>
    public void LadderMove(Vector3 moveDirection)
    {
        m_rigidbody.velocity = moveDirection * m_playerManager.Stat.MoveSpeed_Ladder;
    }

    /// <summary>x, z의 속도를 멈춤</summary>
    public void MoveStopXZ()
    {
        Vector3 newVelocity = Vector3.zero;
        newVelocity.y = m_rigidbody.velocity.y;

        m_rigidbody.velocity = newVelocity;
    }

    /// <summary>모든 속도를 멈춤</summary>
    public void MoveStopAll()
    {
        m_rigidbody.velocity = Vector3.zero;
    }

    /// <summary>스탯에 있는 jumPower로 Rigidbody에 힘을 가함</summary>
    public void Jump()
    {
        m_rigidbody.AddForce(Vector3.up * m_playerManager.Stat.JumpPower, ForceMode.Impulse);
    }

    /// <summary>direction방향으로 머리 LookRotation 회전</summary>
    public void RotateHead(Vector3 direction)
    {
        m_head.rotation = Quaternion.LookRotation(direction);
    }

    /// <summary>direction방향으로 몸 slerp 회전</summary>
    public void RotateBody(Vector3 direction)
    {
        m_body.rotation = Quaternion.Slerp(m_body.rotation, Quaternion.LookRotation(direction), m_playerManager.Stat.RotationSpeed * Time.deltaTime);
    }

    /// <summary>direction방향으로 머리와 몸 회전(머리는 LookDirection, 몸은 slerp)</summary>
    public void RotateHeadAndBody(Vector3 direction)
    {
        RotateHead(direction);
        RotateBody(direction);
    }

    /// <summary>direction방향으로 머리와 몸 LookRotation 회전</summary>
    public void LookRotationHeadAndBody(Vector3 direction)
    {
        RotateHead(direction);
        m_body.rotation = Quaternion.LookRotation(direction);
    }
}
