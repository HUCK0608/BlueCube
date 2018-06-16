using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_SoopState { Idle = 0, ShootInit, Shoot, Dead }
public sealed class EnemyController_Soop : EnemyController
{
    [Header("Don't Touch!")]
    /// <summary>3D 서브 컨트롤러</summary>
    [SerializeField]
    EnemyController3D_Soop m_subController3D;
    /// <summary>2D 서브 컨트롤러</summary>
    [SerializeField]
    EnemyController2D_Soop m_subController2D;

    private EnemyStat_Soop m_stat;
    /// <summary>스텟</summary>
    public EnemyStat_Soop Stat { get { return m_stat; } }

    /// <summary>3D 상태 모음</summary>
    private Dictionary<E_SoopState, EnemyState> m_states3D;
    /// <summary>2D 상태 모음</summary>
    private Dictionary<E_SoopState, EnemyState> m_states2D;

    private E_SoopState m_currentState3D;
    /// <summary>3D 현재 상태</summary>
    public E_SoopState CurrentState3D { get { return m_currentState3D; } }
    private E_SoopState m_currentState2D;
    /// <summary>2D 현재 상태</summary>
    public E_SoopState CurrentState2D { get { return m_currentState2D; } }

    private bool m_isInit;

    protected override void Awake()
    {
        base.Awake();

        m_stat = GetComponent<EnemyStat_Soop>();

        InitStates3D();
        InitStates2D();
    }

    private void Start()
    {
        m_isInit = true;
        ChangeState3D(E_SoopState.Idle);
        m_isInit = false;
    }

    private void InitStates3D()
    {
        m_states3D = new Dictionary<E_SoopState, EnemyState>();

        string stateFirstPath = "EnemyState3D_Soop_";
        // E_SoopState3D Value 가져오기
        E_SoopState[] enumValues = (E_SoopState[])System.Enum.GetValues(typeof(E_SoopState));
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

    private void InitStates2D()
    {
        m_states2D = new Dictionary<E_SoopState, EnemyState>();

        string stateFirstPath = "EnemyState2D_Soop_";
        // E_SoopState3D Value 가져오기
        E_SoopState[] enumValues = (E_SoopState[])System.Enum.GetValues(typeof(E_SoopState));
        // Value들의 개수
        int enumCount = enumValues.Length;

        for (int i = 0; i < enumCount; i++)
        {
            // 찾을 상태 컴포넌트의 풀 네임
            string statePath = stateFirstPath + enumValues[i].ToString("G");
            // 상태 컴포넌트 찾기
            EnemyState state = m_enemy2D.GetComponent(statePath) as EnemyState;
            // 상태 컴포넌트 저장
            m_states2D.Add(enumValues[i], state);
            // 상태 컴포넌트 update 비활성화
            state.enabled = false;
        }
    }

    /// <summary>Soop 3D 상태 변경</summary>
    public void ChangeState3D(E_SoopState newState)
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

    /// <summary>Soop 2D 상태 변경</summary>
    public void ChangeState2D(E_SoopState newState)
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

    /// <summary>적을 3D 상태로 변경</summary>
    public override void ChangeEnemy3D()
    {
        if (m_worldObject.IsIncludeChangeViewRect)
        {
            if (!m_enemy3D.activeSelf)
            {
                m_enemy2D.SetActive(false);

                if (!m_isDead)
                    ChangeState3D(E_SoopState.Idle);
            }

            m_worldObject.IsIncludeChangeViewRect = false;
        }

        m_enemy3D.SetActive(true);

        if (m_isDead)
            ChangeState3D(E_SoopState.Dead);
    }

    /// <summary>적을 2D 상태로 변경</summary>
    public override void ChangeEnemy2D()
    {
        m_enemy3D.SetActive(false);

        if (m_worldObject.IsIncludeChangeViewRect)
        {
            m_enemy2D.SetActive(true);

            if (!m_isDead)
                ChangeState2D(E_SoopState.Idle);
            else
                ChangeState2D(E_SoopState.Dead);
        }
    }

    public override void DeadLogic()
    {
        if(m_isDead)
        {
            if (PlayerManager.Instance.CurrentView.Equals(E_ViewType.View3D))
                m_subController3D.Animator.Play(m_deadAnimatorStatePath, -1, 1f);
            else
                m_subController2D.Animator.Play(m_deadAnimatorStatePath, -1, 1f);
        }

        base.DeadLogic();
    }
}
