using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerSkill_ChangeView : MonoBehaviour
{
    // 플레이어 매니저
    private PlayerManager m_playerManager;

    // 시점변환 상자
    private ChangeViewRect m_changeViewRect;

    // 키 입력
    [SerializeField]
    private KeyCode m_doChangeKey;

    [SerializeField]
    private KeyCode m_notChangeKey;

    // 현재 상태
    private E_ViewType m_currentView;
    /// <summary>현재 시점을 반환 (View2D, View3D)</summary>
    public E_ViewType CurrentView { get { return m_currentView; } }

    private bool m_isViewChange;
    /// <summary>현재 시점변환 중일경우 true를 반환</summary>
    public bool IsViewChange { get { return m_isViewChange; } }

    // 변경을 할지 안할지 체크하는 변수
    private bool m_isDoChange;
    /// <summary>시점변경을 허용했을경우 true를 반환</summary>
    public bool IsDoChange { get { return m_isDoChange; } }

    private bool m_isNotChange;
    /// <summary>시점변경을 허용하지않았을 경우 true를 반환</summary>
    public bool IsNotChange { get { return m_isNotChange; } }

    private void Awake()
    {
        m_playerManager = GetComponent<PlayerManager>();

        m_changeViewRect = GetComponentInChildren<ChangeViewRect>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine(ViewChange2D());
    }

    // 시점변환을 허용할 건지 체크하는 코루틴
    private IEnumerator CheckKey()
    {
        m_isDoChange = false;
        m_isNotChange = false;

        // 모든 키가 안눌렸을 경우 반복
        while(!m_isDoChange && !m_isNotChange)
        {
            if(Input.GetKeyDown(m_doChangeKey))
            {
                m_isDoChange = true;
            }
            else if(Input.GetKeyDown(m_notChangeKey))
            {
                m_isNotChange = true;
            }

            yield return null;
        }
    }

    // 3D에서 2D로 변경
    private IEnumerator ViewChange2D()
    {
        m_isViewChange = true;

        // x, y 사이즈 커짐
        yield return StartCoroutine(m_changeViewRect.SetIncreaseSizeXY());
        // 키 체크 코루틴 활성화
        StartCoroutine(CheckKey());
        // 마우스 좌표로 z크기가 커짐
        yield return StartCoroutine(m_changeViewRect.SetSizeZToMousePoint());

        // 변경이 허용됬을 경우
        if(IsDoChange)
        {
            // 2D상태로 변경됬다고 설정
            m_currentView = E_ViewType.View2D;

            // 오브젝트를 2D상태에 맞게 변경
            WorldManager.Instance.Change2D();

            // 플레이어 변경 넣어야함

            // 블루큐브 변경
            BlueCubeManager.Instance.ChangeCube();

            // 그림자 끄기
            LightManager.Instance.ShadowEnable(false);

            // 쿼터뷰에서 사이드뷰로 카메라가 이동함
            yield return StartCoroutine(GameManager.Instance.CameraManager.MovingWork3D());

            // 2D벽 생성 넣어야함
        }
        // 변경이 허용되지 않았을 경우
        else
        {
            // 포함되었던 오브젝트들의 메테리얼을 기본 메테리얼로 변경
            WorldManager.Instance.SetDefaultMaterialIsInclude();
        }

        // 상자 크기 감소
        yield return StartCoroutine(m_changeViewRect.SetDecreaseSize());

        m_isViewChange = false;
    }

    // 2D에서 3D상태로 변경
    //private IEnumerator ViewChange3D()
    //{
    //    m_isViewChange = true;

    //    m_currentView = E_ViewType.View2D;

    //    // 2D벽 삭제 넣어야함

    //    // 변경상자 활성화
    //    m_changeViewRect.SetActive(true);

    //}
}
