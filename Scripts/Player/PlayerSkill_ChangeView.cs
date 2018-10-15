﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_ViewType { View3D, View2D }
public sealed class PlayerSkill_ChangeView : MonoBehaviour
{
    // 플레이어 매니저
    private PlayerManager m_playerManager;

    // 시점변환 상자
    private ChangeViewRect m_changeViewRect;
    public ChangeViewRect ChangeViewRect { get { return m_changeViewRect; } }

    // 끼임 체크 스크립트
    private CheckBlock m_checkBlock;

    // 현재 상태
    private E_ViewType m_currentView;
    /// <summary>현재 시점을 반환 (View2D, View3D)</summary>
    public E_ViewType CurrentView { get { return m_currentView; } }

    private bool m_isViewChangeReady;
    /// <summary>현재 시점변환이 준비중일경우 true를 반환</summary>
    public bool IsViewChangeReady { get { return m_isViewChangeReady; } }

    private bool m_isViewChange;
    /// <summary>현재 시점변환이 실행중일경우 true를 반환</summary>
    public bool IsViewChange { get { return m_isViewChange; } }

    // 변경을 할지 안할지 체크하는 변수
    private bool m_isDoChange;
    /// <summary>시점변경을 허용했을경우 true를 반환</summary>
    public bool IsDoChange { get { return m_isDoChange; } }

    private bool m_isNotChange;
    /// <summary>시점변경을 허용하지않았을 경우 true를 반환</summary>
    public bool IsNotChange { get { return m_isNotChange; } }

    /// <summary>스킬이 잠겨있는지 여부</summary>
    private bool m_isLock;
    /// <summary>스킬이 잠겨있을 경우 true를 반환</summary>
    public bool IsLock { get { return m_isLock; } }
    /// <summary>스킬 잠금 설정</summary>
    public void SetSkillLock(bool value) { m_isLock = value; }

    private void Awake()
    {
        m_playerManager = GetComponent<PlayerManager>();

        m_changeViewRect = GetComponentInChildren<ChangeViewRect>();

        m_checkBlock = GetComponentInChildren<CheckBlock>();
    }

    private void Start()
    {
        m_changeViewRect.SetActive(false);
    }

    /// <summary>시점변환 실행</summary>
    public void ChangeView()
    {
        // 스킬이 잠겨있을경우 리턴
        if (m_isLock)
            return;

        // 현재 시점이 3D일 때 2D로 변경
        if (m_currentView.Equals(E_ViewType.View3D))
        {
            StartCoroutine(ChangeView2D());
        }
        // 현재 시점이 2D일 때 3D로 변경
        else
        {
            StartCoroutine(ChangeView3D());
        }
    }

    /// <summary>2D 상태로 시작</summary>
    public void StartView2D()
    {
        GameManager.Instance.SetCursorEnable(false);

        m_currentView = E_ViewType.View2D;
        LightManager.Instance.ShadowEnable(false);
        m_changeViewRect.StartView2D();
        CameraManager.Instance.StartView2D();
        PlayerManager.Instance.PlayerChange2D();
        WorldManager.Instance.StartView2D();
    }
    
    // 시점변환을 허용할 건지 체크하는 코루틴
    private IEnumerator CheckKey()
    {
        m_isDoChange = false;
        m_isNotChange = false;

        // 모든 키가 안눌렸을 경우 반복
        while(!m_isDoChange && !m_isNotChange)
        {
            if (UIManager.Instance.IsOnTabUI || UIManager.Instance.IsOnPauseUI)
            {
                m_isNotChange = true;
                break;
            }

            // 끼임 오브젝트 체크
            m_checkBlock.Check();

            // 수행 키를 누를 경우
            if (!Input.GetKey(m_playerManager.Stat.AcceptKey))
            {
                // 끼일 오브젝트가 있다면 시각적인 이벤트 실행
                // 끼일 오브젝트가 없다면 시점변환 실행
                if (!m_checkBlock.IsBlock)
                    m_isDoChange = true;
                else
                    m_isNotChange = true;
            }
            // 취소 키를 누를 경우
            else if (Input.GetKeyDown(m_playerManager.Stat.CancelKey))
            {
                m_isNotChange = true;
            }

            yield return null;
        }
    }

    // 3D에서 2D로 변경
    private IEnumerator ChangeView2D()
    {
        // 전환준비중 설정
        m_isViewChangeReady = true;

        // 변경상자 활성화
        m_changeViewRect.SetActive(true);

        // x, y 사이즈 커짐
        yield return StartCoroutine(m_changeViewRect.SetIncreaseSizeXY());
        // 키 체크 코루틴 활성화
        StartCoroutine(CheckKey());
        // 마우스 좌표로 z크기가 커짐
        yield return StartCoroutine(m_changeViewRect.SetSizeZToMousePoint());

        // 변경이 허용됬을 경우
        if(IsDoChange)
        {
            // 시점변환 이펙트 생성
            EffectManager.Instance.CreateEffect(Effect_Type.Player_ChangeView_Capsule, m_playerManager.Player3D_Object.transform.position);

            // 시간 정지
            m_isViewChange = true;

            // 커서 비활성화
            GameManager.Instance.SetCursorEnable(false);

            // 2D상태로 변경됬다고 설정
            m_currentView = E_ViewType.View2D;

            // 오브젝트를 2D상태에 맞게 변경
            WorldManager.Instance.Change2D();

            // 그림자 끄기
            LightManager.Instance.ShadowEnable(false);

            // 쿼터뷰에서 사이드뷰로 카메라가 이동함
            yield return StartCoroutine(CameraManager.Instance.StartMovingWork());

            // 2D외벽 활성화
            m_changeViewRect.SetOutWallEnable(true);
        }
        // 변경이 허용되지 않았을 경우
        else
        {
            // 오브젝트를 다시 3D상태로 변경
            WorldManager.Instance.Change3D();

            // 상자 크기 줄이기
            //yield return StartCoroutine(m_changeViewRect.SetDecreaseSize());
        }

        // 상자 비활성화
        m_changeViewRect.SetActive(false);

        // 시간 정지를 풀기
        m_isViewChange = false;

        // 전환준비중 해제
        m_isViewChangeReady = false;
    }

    // 2D에서 3D상태로 변경
    private IEnumerator ChangeView3D()
    {
        m_isViewChange = true;

        m_currentView = E_ViewType.View3D;

        // 커서 활성화
        GameManager.Instance.SetCursorEnable(true);

        // 2D외벽 비활성화
        m_changeViewRect.SetOutWallEnable(false);

        // 오브젝트를 3D상태에 맞게 변경
        WorldManager.Instance.Change3D();

        // 플레이어 3D로 변경
        PlayerManager.Instance.PlayerChange3D();

        // 그림자 켜기
        LightManager.Instance.ShadowEnable(true);

        // 사이드뷰에서 쿼터뷰로 카메라가 이동함
        yield return StartCoroutine(CameraManager.Instance.StartMovingWork());

        // 상자 크기 감소
        //yield return StartCoroutine(m_changeViewRect.SetDecreaseSize());

        // 땅이 아니라면 Hold 상태로 변경
        if(!PlayerManager.Instance.SubController3D.CheckGround.Check())
            PlayerManager.Instance.MainController.ChangeState3D(E_PlayerState3D.Hold);

        m_isViewChange = false;
    }

    /// <summary>시점변환 스킬 초기화</summary>
    public void ResetSkill_ChangeView()
    {
        if (m_currentView.Equals(E_ViewType.View2D))
        {
            m_currentView = E_ViewType.View3D;
            GameManager.Instance.SetCursorEnable(true);
            m_changeViewRect.SetOutWallEnable(false);
            WorldManager.Instance.Change3D();
            PlayerManager.Instance.PlayerChange3D();
            LightManager.Instance.ShadowEnable(true);
            CameraManager.Instance.ResetCamera();
        }
    }
}
