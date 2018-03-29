using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_PlayerState { Idle, Move, Attack, JumpUp, JumpDown, LadderInit, LadderIdle, LadderMove }

public sealed class PlayerController : MonoBehaviour
{
    // 플레이어 3D 상태 모음
    private Dictionary<E_PlayerState, PlayerState> m_states3D;

    // 플레이어 3D 현재 상태
    private PlayerState m_currentState3D;

    private bool m_isGrounded;
    /// <summary>캐릭터가 땅에 있을경우 true를 반환</summary>
    public bool IsGrounded { get { return m_isGrounded; } set { m_isGrounded = value; } }

    private void Awake()
    {
        InitStates3D();
    }

    private void Start()
    {
        ChangeState3D(E_PlayerState.Idle);
    }

    private void InitStates3D()
    {
        // 딕셔너리 초기화
        m_states3D = new Dictionary<E_PlayerState, PlayerState>();

        // 플레이어3D
        GameObject player3D = transform.Find("Player3D").gameObject;
        // 찾을 상태 컴포넌트의 앞 글자
        string stateFirstPath = "PlayerState3D_";
        // E_PlayerState Value 가져오기
        E_PlayerState[] enumValues = (E_PlayerState[])System.Enum.GetValues(typeof(E_PlayerState));
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

    /// <summary>플레이어 3D 상태 변경</summary>
    public void ChangeState3D(E_PlayerState newState)
    {
        if(m_currentState3D != null)
        {
            m_currentState3D.enabled = false;
            m_currentState3D.EndState();
        }

        m_currentState3D = m_states3D[newState];

        m_currentState3D.InitState();
        m_currentState3D.enabled = true;
    }
}
