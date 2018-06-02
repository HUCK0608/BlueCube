using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_SoopState3D { Idle = 0, ShootInit, Shoot, Dead }
public sealed class EnemyController_Soop : EnemyController
{
    private EnemyStat_Soop m_stat;
    /// <summary>스텟</summary>
    public EnemyStat_Soop Stat { get { return m_stat; } }

    /// <summary>3D 상태 모음</summary>
    private Dictionary<E_SoopState3D, EnemyState> m_states3D;

    /// <summary>3D 현재 상태</summary>
    private E_SoopState3D m_currentState3D;

    private bool m_isInit;

    private void Awake()
    {
        InitStates3D();
    }

    private void Start()
    {
        m_isInit = true;
        ChangeState3D(E_SoopState3D.Idle);
        m_isInit = false;
    }

    private void InitStates3D()
    {
        m_states3D = new Dictionary<E_SoopState3D, EnemyState>();

        string stateFirstPath = "EnemyState3D_Soop_";
        // E_SoopState3D Value 가져오기
        E_SoopState3D[] enumValues = (E_SoopState3D[])System.Enum.GetValues(typeof(E_SoopState3D));
        // Value들의 개수
        int enumCount = enumValues.Length;

        for (int i = 0; i < enumCount; i++)
        {
            // 찾을 상태 컴포넌트의 풀 네임
            string statePath = stateFirstPath + enumValues[i].ToString("G");
            // 상태 컴포넌트 찾기
            EnemyState state = m_enemy3D.GetComponent(statePath) as EnemyState;
            // 상태 컴포넌트 저장
            m_states3D.Add(enumValues[i], state);
            // 상태 컴포넌트 update 비활성화
            state.enabled = false;
        }
    }

    /// <summary>Soop 3D 상태 변경</summary>
    public void ChangeState3D(E_SoopState3D newState)
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
}
