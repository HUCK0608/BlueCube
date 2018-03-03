using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_EnemyState_Monster1 { Idle, Chase, Attack, Return }

// 스탯이 컴포넌트로 따라오게 함
[RequireComponent(typeof(EnemyStat_Monster1))]
// 상태가 컴포넌트로 따라오게 함
[RequireComponent(typeof(EnemyState_Monster1_Idle))]
[RequireComponent(typeof(EnemyState_Monster1_Chase))]
[RequireComponent(typeof(EnemyState_Monster1_Attack))]
[RequireComponent(typeof(EnemyState_Monster1_Return))]
public sealed class EnemyManager_Monster1 : EnemyManager
{
    private EnemyStat_Monster1 m_stat;
    /// <summary>능력치</summary>
    public EnemyStat_Monster1 Stat { get { return m_stat; } }

    // 상태 모음
    private Dictionary<E_EnemyState_Monster1, EnemyState> m_states;

    // 현재 상태
    private EnemyState m_currentState;

    private void Awake()
    {
        m_stat = GetComponent<EnemyStat_Monster1>();
    }

    private void Start()
    {
        InitState();
        ChangeState(E_EnemyState_Monster1.Idle);
    }

    // 상태 초기화
    private void InitState()
    {
        m_states = new Dictionary<E_EnemyState_Monster1, EnemyState>();

        string firstStateName = "EnemyState_Monster1_";

        E_EnemyState_Monster1[] enumValues = (E_EnemyState_Monster1[])System.Enum.GetValues(typeof(E_EnemyState_Monster1));

        int stateCount = enumValues.Length;

        for (int i = 0; i < stateCount; i++)
        {
            string stateName = firstStateName + enumValues[i].ToString("G");

            EnemyState state = GetComponent(stateName) as EnemyState;
            m_states.Add(enumValues[i], state);
            state.enabled = false;
        }
    }

    /// <summary>몬스터를 새로운 상태(newState)로 바꿉니다.</summary>
    public void ChangeState(E_EnemyState_Monster1 newState)
    {
        if(m_currentState != null)
        {
            m_currentState.EndState();
            m_currentState.enabled = false;
        }

        m_currentState = m_states[newState];
        m_currentState.InitState();
        m_currentState.enabled = true;
    }

}
