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

    [SerializeField]
    private List<Transform> m_itemCheckPoints;
    /// <summary>아이템 체크 위치모음</summary>
    public List<Transform> ItemCheckPoints { get { return m_itemCheckPoints; } }

    private Animator m_animator;
    public Animator Animator { get { return m_animator; } }

    // 리지드바디
    private Rigidbody m_rigidbody;
    public Rigidbody Rigidbody { get { return m_rigidbody; } }

    /// <summary>3D플레이어의 정면 방향을 반환</summary>
    public Vector3 Forward { get { return transform.forward; } }

    private void Awake()
    {
        m_playerManager = GetComponentInParent<PlayerManager>();
        m_mainController = GetComponentInParent<PlayerController>();

        m_checkGround = GetComponent<CheckGround3D>();
        m_checkLadder = GetComponent<CheckLadder>();

        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        ApplyGravity();
    }

    /// <summary>중력 적용</summary>
    public void ApplyGravity()
    {
        E_PlayerState3D currentState = m_mainController.CurrentState3D;

        if (m_playerManager.IsViewChange || m_playerManager.IsViewChangeReady || currentState.Equals(E_PlayerState3D.Hold))
        {
            MoveStopAll();
            return;
        }

        // 사다리일 경우 중력적용을 하지 않음
        if (currentState.Equals(E_PlayerState3D.LadderIdle) || currentState.Equals(E_PlayerState3D.LadderMove))
            return;

        // 땅인지 체크
        m_mainController.IsGrounded = m_checkGround.Check();

        Vector3 newVelocity = m_rigidbody.velocity;

        // 땅까지의 거리가 SkinWidth이내이고 현재 낙하 속도가 0이내일 경우 중력을 적용하지 않음
        if(m_checkGround.DistanceToGround <= m_playerManager.Stat.SkinWidth && m_rigidbody.velocity.y <= 0f)
        {
            newVelocity.y = 0f;
        }
        //  그 외엔 중력을 적용함
        else
        {
            newVelocity.y += m_playerManager.Stat.Gravity * Time.deltaTime;
        }

        // 속도적용
        m_rigidbody.velocity = newVelocity;
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

    /// <summary>direction방향으로 이동 및 회전을 함</summary>
    public void MoveAndRotate(Vector3 direction)
    {
        // 이동방향 * 속도 계산
        // y는 기존의 속도를 이용
        Vector3 movement = direction * m_playerManager.Stat.MoveSpeed;
        movement.y = m_rigidbody.velocity.y;

        // 속도 적용
        m_rigidbody.velocity = movement;

        // 이동 입력이 없다면 리턴
        if (direction.Equals(Vector3.zero))
            return;

        // 회전
        LerpRotation(direction);
    }

    /// <summary>position으로 이동 및 회전을 함</summary>
    public void MoveAndRotateTowards(Vector3 position)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, m_playerManager.Stat.MoveSpeed * Time.deltaTime);

        Vector3 direction = position - transform.position;
        direction.y = 0f;
        direction = direction.normalized;

        if(!direction.Equals(Vector3.zero))
            LerpRotation(direction.normalized);
    }

    /// <summary>Jump상태에서 direction방향으로 이동 및 회전을 함</summary>
    public void JumpMoveAndRotate(Vector3 direction)
    {
        // 이동방향 * 속도 계산
        // y는 기존의 속도를 이용
        Vector3 movement = direction * m_playerManager.Stat.MoveSpeed_Jump;
        movement.y = m_rigidbody.velocity.y;

        // 속도 적용
        m_rigidbody.velocity = movement;

        // 이동 입력이 없다면 리턴
        if (direction.Equals(Vector3.zero))
            return;

        // 회전
        LerpRotation(direction);
    }

    /// <summary>moveDirection방향으로 Stat에 MoveSpeed_Ladder속도로 이동함</summary>
    public void LadderMove(Vector3 moveDirection)
    {
        m_rigidbody.velocity = moveDirection * m_playerManager.Stat.MoveSpeed_Ladder;
    }

    /// <summary>점프</summary>
    public void Jump()
    {
        Vector3 newVelocity = m_rigidbody.velocity;
        newVelocity.y = m_playerManager.Stat.JumpSpeed;
        m_rigidbody.velocity = newVelocity;
    }

    /// <summary>플레이어가 lerp로 direction 방향을 바라보게 함</summary>
    public void LerpRotation(Vector3 direction)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), m_playerManager.Stat.RotationSpeed * Time.deltaTime);
    }

    /// <summary>플레이어가 direciton 방향을 바라보게 함</summary>
    public void LookRotation(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
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

    /// <summary>오디오 플레이</summary>
    public void AudioPlay(string playName)
    {
        m_playerManager.AudioPlayer.Play(playName);
    }
}
