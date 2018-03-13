using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_EnemyState { Idle, Move, Attack, Return, Dead }
public enum E_EnemyAttackRangeType { Meele, Long }

public sealed class EnemyManager : MonoBehaviour
{
    private EnemyStat m_stat;
    /// <summary>적 스텟</summary>
    public EnemyStat Stat { get { return m_stat; } }

    /// <summary>상태 모음</summary>
    private Dictionary<E_EnemyState, EnemyState> m_states;
    /// <summary>현재 상태</summary>
    private EnemyState m_currentState;

    // 공격 사거리 타입
    [SerializeField]
    private E_EnemyAttackRangeType m_attackType;

    // 죽었는지
    private bool m_isDie;
    public bool IsDie { get { return m_isDie; } }

    private void Awake()
    {
        m_stat = GetComponent<EnemyStat>();

        InitStates();
    }

    private void InitStates()
    {
        m_states = new Dictionary<E_EnemyState, EnemyState>();

        string enemyTypeString = "";

        if (m_attackType.Equals(E_EnemyAttackRangeType.Meele))
            enemyTypeString = GameLibrary.String_EnemyState_Melee;
        else if (m_attackType.Equals(E_EnemyAttackRangeType.Long))
            enemyTypeString = GameLibrary.String_EnemyState_Long;

        E_EnemyState[] enumValues = (E_EnemyState[])System.Enum.GetValues(typeof(E_EnemyState));

        for(int i = 0; i < enumValues.Length; i++)
        {
            string stateName = enemyTypeString + enumValues[i].ToString("G");
            EnemyState state = GetComponent(stateName) as EnemyState;

            m_states.Add(enumValues[i], state);
            state.enabled = false;
        }
    }

    private void Start()
    {
        ChangeState(E_EnemyState.Idle);
    }

    /// <summary>상태 변경(새로운 상태)</summary>
    public void ChangeState(E_EnemyState newState)
    {
        if (m_currentState != null)
        {
            m_currentState.EndState();
            m_currentState.enabled = false;
        }

        m_currentState = m_states[newState];

        m_currentState.InitState();
        m_currentState.enabled = true;
    }

    private void Update()
    {
        if (GameLibrary.Bool_IsGameStop)
            return;

        CheckDie();
    }

    private void CheckDie()
    {
        if (m_isDie)
            gameObject.SetActive(false);
    }

    /// <summary>죽음처리</summary>
    public void Die()
    {
        m_isDie = true;
    }
}
