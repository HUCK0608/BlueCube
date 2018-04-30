using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerController_Intro : MonoBehaviour
{
    private static PlayerController_Intro m_instance;
    public static PlayerController_Intro Instance { get { return m_instance; } }

    // 플레이어
    private Transform m_player;

    private PlayerStat_Intro m_stat;
    /// <summary>스탯</summary>
    public PlayerStat_Intro Stat { get { return m_stat; } }

    // 상태 모음
    private Dictionary<E_PlayerState2D, PlayerState_Intro> m_states;
    // 플레이어 현재 상태
    private E_PlayerState2D m_currentState;

    // 땅 체크 스크립트
    private CheckGround_Intro m_checkGround;

    // 리지드바디
    private Rigidbody2D m_rigidbody;
    public Rigidbody2D Rigidbody { get { return m_rigidbody; } }

    private bool m_isGrounded;
    /// <summary>땅일경우 true를 반환</summary>
    public bool IsGrounded { get { return m_isGrounded; } }

    private bool m_isInit;

    private void Awake()
    {
        m_instance = this;

        m_player = transform.Find("Player");

        m_stat = GetComponent<PlayerStat_Intro>();

        m_checkGround = GetComponentInChildren<CheckGround_Intro>();

        m_rigidbody = GetComponentInChildren<Rigidbody2D>();

        InitStates();
    }

    private void Start()
    {
        m_isInit = true;
        ChangeState(E_PlayerState2D.Idle);
        m_isInit = false;
    }

    private void LateUpdate()
    {
        ApplyGravity();
    }

    /// <summary>상태 초기화</summary>
    private void InitStates()
    {
        m_states = new Dictionary<E_PlayerState2D, PlayerState_Intro>();

        // 찾을 상태 컴포넌트의 앞 글자
        string stateFirstPath = "PlayerState_Intro_";
        // E_PlayerState2D Value 가져오기
        E_PlayerState2D[] enumValues = (E_PlayerState2D[])System.Enum.GetValues(typeof(E_PlayerState2D));
        // Value들의 개수
        int enumCount = enumValues.Length;

        for(int i = 0; i <enumCount; i++)
        {
            // 찾을 상태 컴포넌트의 풀 네임
            string statePath = stateFirstPath + enumValues[i].ToString("G");
            // 상태 컴포넌트 찾기
            PlayerState_Intro state = m_player.GetComponent(statePath) as PlayerState_Intro;
            // 상태 컴포넌트 저장
            m_states.Add(enumValues[i], state);
            // 상태 컴포넌트 update 비활성화
            state.enabled = false;
        }
    }

    /// <summary>플레이어 상태 변경</summary>
    public void ChangeState(E_PlayerState2D newState)
    {
        if(!m_isInit)
        {
            m_states[m_currentState].enabled = false;
            m_states[m_currentState].EndState();
        }

        m_currentState = newState;

        m_states[m_currentState].InitState();
        m_states[m_currentState].enabled = true;
    }

    /// <summary>땅인지 체크를 한 후 땅이 아닐 경우 중력을 적용시킨다</summary>
    private void ApplyGravity()
    {
        // 밑이 땅인지 체크
        m_isGrounded = m_checkGround.Check();

        Vector2 newVelocity = m_rigidbody.velocity;

        // 땅까지의 거리가 SkinWidth이내이고 현재 낙하 속도가 0이내일 경우 중력을 적용하지 않음
        if (m_checkGround.DistanceToGround <= m_stat.SkinWidth && m_rigidbody.velocity.y <= 0f)
        {
            newVelocity.y = 0f;
        }
        //  그 외엔 중력을 적용함
        else
        {
            newVelocity.y += m_stat.Gravity * Time.deltaTime;
        }

        // 속도적용
        m_rigidbody.velocity = newVelocity;
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
        // 이동방향 * 속도 계산
        // y는 기존의 속도를 이용
        Vector2 movement = direction * m_stat.MoveSpeed;
        movement.y = m_rigidbody.velocity.y;

        // 속도 적용
        m_rigidbody.velocity = movement;

        // 이동입력이 없다면 리턴
        if (direction.Equals(Vector2.zero))
            return;

        // 스케일 변화로 회전값 구하기
        Vector3 scale = Vector3.one;
        scale.x = direction.x;

        // 스케일 변경으로 회전
        // 만약 스케일 값이 같지 않다면 스케일 변경
        if (!m_player.localScale.x.Equals(scale.x))
            m_player.localScale = scale;
    }

    /// <summary>Jump상태에서 direction방향으로 이동 및 회전을 함</summary>
    public void JumpMoveAndRotate(Vector2 direction)
    {
        // 이동방향 * 속도 계산 및 y는 기존의 속도 사용
        Vector2 movement = direction * m_stat.MoveSpeed_Jump;
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
        if (!m_player.localScale.x.Equals(scale.x))
            m_player.localScale = scale;
    }

    /// <summary>스탯에 있는 jumpSpeed로 Rigidbody에 힘을 가함</summary>
    public void Jump()
    {
        Vector2 newVelocity = m_rigidbody.velocity;
        newVelocity.y = m_stat.JumpSpeed;
        m_rigidbody.velocity = newVelocity;
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
}
