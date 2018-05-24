using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public enum E_PlayerState3D { Idle, Move, JumpUp, Falling, PickInit, PickIdle, PickMove, PickJumpUp, PickFalling, PutInit, PutDefault, PutThrow, PushChase, PushInit, PushIdle, Pushing, ChangeViewInit, Hold, LadderInit, LadderIdle, LadderMove  }
public enum E_PlayerState2D { Idle, Move, JumpUp, Falling, Landing }

public sealed class PlayerController : MonoBehaviour
{
    // 플레이어 3D, 2D 상태 모음
    private Dictionary<E_PlayerState3D, PlayerState> m_states3D;
    private Dictionary<E_PlayerState2D, PlayerState> m_states2D;

    // 플레이어 3D, 2D 현재 상태
    private E_PlayerState3D m_currentState3D;
    private E_PlayerState2D m_currentState2D;
    public E_PlayerState3D CurrentState3D { get { return m_currentState3D; } }
    public E_PlayerState2D CurrentState2D { get { return m_currentState2D; } }
    
    private bool m_isGrounded;
    /// <summary>캐릭터가 땅에 있을경우 true를 반환</summary>
    public bool IsGrounded { get { return m_isGrounded; } set { m_isGrounded = value; } }

    private bool m_isInit;

    private void Awake()
    {
        InitStates3D();
        InitStates2D();
    }

    private void Start()
    {
        m_isInit = true;
        ChangeState3D(E_PlayerState3D.Idle);
        ChangeState2D(E_PlayerState2D.Idle);
        m_isInit = false;
    }

    private void InitStates3D()
    {
        // 딕셔너리 초기화
        m_states3D = new Dictionary<E_PlayerState3D, PlayerState>();

        // 플레이어3D
        GameObject player3D = transform.Find("Player3D").gameObject;
        // 찾을 상태 컴포넌트의 앞 글자
        string stateFirstPath = "PlayerState3D_";
        // E_PlayerState3D Value 가져오기
        E_PlayerState3D[] enumValues = (E_PlayerState3D[])System.Enum.GetValues(typeof(E_PlayerState3D));
        // Value들의 개수
        int enumCount = enumValues.Length;

        for(int i = 0; i <enumCount; i++)
        {
            // 찾을 상태 컴포넌트의 풀 네임
            string statePath = stateFirstPath + enumValues[i].ToString("G");
            // 상태 컴포넌트 찾기
            PlayerState state = player3D.GetComponent(statePath) as PlayerState;
            // 상태 컴포넌트 저장
            m_states3D.Add(enumValues[i], state);
            // 상태 컴포넌트 update 비활성화
            state.enabled = false;
        }
    }
    
    private void InitStates2D()
    {
        // 딕셔너리 초기화
        m_states2D = new Dictionary<E_PlayerState2D, PlayerState>();

        // 플레이어2D
        GameObject player2D = transform.Find("Player2D").gameObject;
        // 찾을 상태 컴포넌트의 앞 글자
        string stateFirstPath = "PlayerState2D_";
        // E_PlayerState2D Value 가져오기
        E_PlayerState2D[] enumValues = (E_PlayerState2D[])System.Enum.GetValues(typeof(E_PlayerState2D));
        // Value들의 개수
        int enumCount = enumValues.Length;

        for(int i = 0; i < enumCount; i++)
        {
            // 찾을 상태 컴포넌트의 풀 네임
            string statePath = stateFirstPath + enumValues[i].ToString("G");
            // 상태 컴포넌트 찾기
            PlayerState state = player2D.GetComponent(statePath) as PlayerState;
            // 상태 컴포넌트 저장
            m_states2D.Add(enumValues[i], state);
            // 상태 컴포넌트 update 비활성화
            state.enabled = false;
        }
    }

    /// <summary>플레이어 3D 상태 변경</summary>
    public void ChangeState3D(E_PlayerState3D newState)
    {
        if(!m_isInit)
        {
            m_states3D[m_currentState3D].enabled = false;
            m_states3D[m_currentState3D].EndState();
        }

        m_currentState3D = newState;

        m_states3D[m_currentState3D].InitState();
        m_states3D[m_currentState3D].enabled = true;
    }

    /// <summary>플레이어 2D 상태 변경</summary>
    public void ChangeState2D(E_PlayerState2D newState)
    {
        if(!m_isInit)
        {
            m_states2D[m_currentState2D].enabled = false;
            m_states2D[m_currentState2D].EndState();
        }

        m_currentState2D = newState;

        m_states2D[m_currentState2D].InitState();
        m_states2D[m_currentState2D].enabled = true;
    }
}
