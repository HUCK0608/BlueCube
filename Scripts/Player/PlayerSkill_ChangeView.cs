using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시점타입
public enum E_ViewType { View3D, View2D }

public sealed class PlayerSkill_ChangeView : MonoBehaviour
{
    // 매니저
    private PlayerManager m_manager;

    // 2D로 변하는 공간
    private GameObject m_changeViewRect;

    // 2D 벽
    private GameObject m_leftWall2D;
    private GameObject m_rightWall2D;
    private GameObject m_upWall2D;

    // 벽 위치를 구하기위한 벡터
    private Vector3 m_wallPos;

    // 2D로 변하는 공간 최대 사이즈
    [SerializeField]
    private int m_maxSizeX, m_maxSizeY, m_maxSizeZ;

    // 증가 수치 퍼센트
    [SerializeField]
    private float m_increaseSizePer;

    // 블루큐브 사이즈
    private Vector3 m_blueCubeSize;

    // 증가수치
    private float m_increaseValueX;
    private float m_increaseValueY;
    private float m_increaseValueZ;

    // 증가수치 벡터
    private Vector3 m_increaseValue;

    // 현재 시점 변경중인지
    private bool m_isChnaging;
    
    // 현재 시점
    private E_ViewType m_viewType;
    public E_ViewType ViewType { get { return m_viewType; } }

    private void Awake()
    {
        m_manager = GetComponent<PlayerManager>();

        m_blueCubeSize = GameManager.Instance.BlueCubeManager.transform.localScale;

        InitChangeViewRect();

        InitWall2D();
    }

    // 2D 벽 초기화
    private void InitWall2D()
    {
        // 2D 벽 구하기
        m_leftWall2D = transform.Find("Skill_ChangeView").Find("LeftWall2D").gameObject;
        m_rightWall2D = transform.Find("Skill_ChangeView").Find("RightWall2D").gameObject;
        m_upWall2D = transform.Find("Skill_ChangeView").Find("UpWall2D").gameObject;

        BoxCollider2D leftWallCollider2D = m_leftWall2D.GetComponent<BoxCollider2D>();
        BoxCollider2D rightWallCollider2D = m_rightWall2D.GetComponent<BoxCollider2D>();
        BoxCollider2D upWallCollider2D = m_upWall2D.GetComponent<BoxCollider2D>();

        // 콜라이더 박스 크기 설정
        leftWallCollider2D.size = new Vector2(1, m_maxSizeY + 2);
        rightWallCollider2D.size = leftWallCollider2D.size;
        upWallCollider2D.size = new Vector2(m_maxSizeX + 2, 1);
    }

    // 2D 공간 관련 초기화
    private void InitChangeViewRect()
    {
        m_changeViewRect = transform.Find("Skill_ChangeView").Find("ChangeViewRect").gameObject;

        // 상자 증가수치 구하기
        m_increaseValueX = (m_maxSizeX - m_blueCubeSize.x) * m_increaseSizePer * 0.01f;
        m_increaseValueY = (m_maxSizeY - m_blueCubeSize.y) * m_increaseSizePer * 0.01f;
        m_increaseValueZ = (m_maxSizeZ - m_blueCubeSize.z) * m_increaseSizePer * 0.01f;

        // 증가수치 벡터로 변경
        m_increaseValue = new Vector3(m_increaseValueX, m_increaseValueY, m_increaseValueZ);
    }

    private void Update()
    {
        // 시점 변경중이 아니고 카메라 변경 키를 눌렀을 때
        if (!m_isChnaging && Input.GetKeyDown(m_manager.ChangeViewKey))
        {
            // 현재 시점이 3D이면 2D로 변경
            if (m_viewType == E_ViewType.View3D)
                StartCoroutine(ChangeView2D());
        }
    }

    // 2D로 변경
    private IEnumerator ChangeView2D()
    {
        m_isChnaging = true;
        m_viewType = E_ViewType.View2D;

        // 2D 변경 상자의 시작 위치를 블루큐브 위치로 잡고 활성화
        m_changeViewRect.transform.localScale = m_blueCubeSize;
        m_changeViewRect.transform.position = GameManager.Instance.BlueCubeManager.transform.position;
        m_changeViewRect.SetActive(true);

        // 2D 변경 상자 크기 커지게 하기
        while (true)
        {
            m_changeViewRect.transform.localScale += m_increaseValue;

            if (m_changeViewRect.transform.localScale.x >= m_maxSizeX)
                break;

            yield return new WaitForFixedUpdate();
        }

        // 카메라 무빙워크 (쿼터뷰에서 사이드뷰로 이동)
        yield return StartCoroutine(GameManager.Instance.CameraManager.MovingWork3D());

        // 2D 벽 생성
        Wall2D_SetActive(true);

        // 모든 설정이 끝나면 2D 변경 상자를 비활성화
        m_changeViewRect.SetActive(false);

        m_isChnaging = false;
    }

    // 2D 벽 Active
    private void Wall2D_SetActive(bool value)
    {
        m_leftWall2D.SetActive(value);
        m_rightWall2D.SetActive(value);
        m_upWall2D.SetActive(value);

        // 벽이 켜지면 위치 구하기
        if(value)
        {
            // 왼쪽 벽 위치 구하기
            m_wallPos = m_changeViewRect.transform.position;
            m_wallPos.x -= m_maxSizeX / 2 + 1;
            m_leftWall2D.transform.position = m_wallPos;

            // 오른쪽 벽 위치 구하기
            m_wallPos.x += m_maxSizeX + 2;
            m_rightWall2D.transform.position = m_wallPos;

            // 위쪽 벽 위치 구하기
            m_wallPos = m_changeViewRect.transform.position;
            m_wallPos.y += m_maxSizeY / 2 + 1;
            m_upWall2D.transform.position = m_wallPos;
        }
    }
}
