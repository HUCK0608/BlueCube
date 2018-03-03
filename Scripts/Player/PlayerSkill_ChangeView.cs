using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시점타입
public enum E_ViewType { View3D, View2D }

public sealed class PlayerSkill_ChangeView : MonoBehaviour
{
    // 플레이어 매니저
    private PlayerManager m_playerManager;

    // 2D로 변하는 공간 오브젝트
    private GameObject m_changeViewRect_GO;
    // 2D로 변하는 공간 스크립트
    private ChangeViewRect m_changeViewRect_S;

    // 2D 벽
    private GameObject m_leftWall2D;
    private GameObject m_rightWall2D;
    private GameObject m_upWall2D;

    // 벽 위치를 구하기위한 벡터
    private Vector3 m_wallPos;

    // 2D로 변하는 공간 최대 사이즈
    [SerializeField]
    private int m_maxSizeX, m_maxSizeY, m_maxSizeZ;

    // x,y 증가 수치 퍼센트
    [SerializeField]
    private float m_increaseSizePerXY;
    // z 증가 수치 퍼센트
    [SerializeField]
    private float m_increaseSizePerZ;

    // 블루큐브 사이즈
    private Vector3 m_blueCubeSize;

    // 증가수치
    private float m_increaseValueX;
    private float m_increaseValueY;
    private float m_increaseValueZ;

    // 증가수치 벡터
    private Vector3 m_increaseVectorXY;
    private Vector3 m_increaseVectorZ;

    // z 감소수치 벡터
    private Vector3 m_decreaseVector;

    // 현재 시점 변경중인지
    private bool m_isChnaging;
    public bool IsChanging { get { return m_isChnaging; } }
    
    // 현재 시점
    private E_ViewType m_viewType;
    public E_ViewType ViewType { get { return m_viewType; } }

    private static E_ViewType m_view2D = E_ViewType.View2D;
    private static E_ViewType m_view3D = E_ViewType.View3D;

    private void Awake()
    {
        m_playerManager = GetComponent<PlayerManager>();

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
        m_changeViewRect_GO = transform.Find("Skill_ChangeView").Find("ChangeViewRect").gameObject;
        m_changeViewRect_S = m_changeViewRect_GO.GetComponent<ChangeViewRect>();

        // 상자 증가수치 구하기
        m_increaseValueX = (m_maxSizeX - m_blueCubeSize.x) * m_increaseSizePerXY * 0.01f;
        m_increaseValueY = (m_maxSizeY - m_blueCubeSize.y) * m_increaseSizePerXY * 0.01f;
        m_increaseValueZ = (m_maxSizeZ - m_blueCubeSize.z) * m_increaseSizePerZ * 0.01f;

        // x, y 증가수치 벡터
        m_increaseVectorXY = new Vector3(m_increaseValueX, m_increaseValueY, 0f);
        // z 증가수치 벡터
        m_increaseVectorZ = new Vector3(0, 0, m_increaseValueZ);
        // 감소수치 벡터
        m_decreaseVector = -(m_increaseVectorXY + m_increaseVectorZ);
    }

    private void Update()
    {
        // 시점 변경중이 아니고 카메라 변경 키를 눌렀을 때
        if (!m_isChnaging && Input.GetKeyDown(m_playerManager.ChangeViewKey))
        {
            // 현재 시점이 3D이면 2D로 변경
            if (m_viewType.Equals(m_view3D))
                StartCoroutine(ChangeView2D());
            // 현재 시점이 2D이면 3D로 변경
            else if (m_view2D.Equals(m_view2D))
                StartCoroutine(ChangeView3D());
        }
    }

    // 2D로 변경
    private IEnumerator ChangeView2D()
    {
        m_isChnaging = true;
        m_viewType = m_view2D;

        Vector3 blueCubePosition = GameManager.Instance.BlueCubeManager.transform.position;

        // 2D 변경 상자의 시작 위치를 블루큐브 위치로 잡고 활성화
        m_changeViewRect_GO.transform.localScale = m_blueCubeSize;
        m_changeViewRect_GO.transform.position = blueCubePosition;
        m_changeViewRect_GO.SetActive(true);
        // 충돌체크 켜기
        m_changeViewRect_S.CheckIncludeWO(true);

        // 2D 변경 상자 x, y 커지게 하기
        while (true)
        {
            // x, y 크기 키우기
            m_changeViewRect_GO.transform.localScale += m_increaseVectorXY;

            if (m_changeViewRect_GO.transform.localScale.x >= m_maxSizeX)
                break;

            yield return new WaitForFixedUpdate();
        }

        // 2D 변경 상자 z 커지게 하기 (시점변환 키를 누르고 있는동안에만 적용)
        // 시점변환 키를 땔 경우 다음으로 넘어감
        // 상자는 z양의 방향으로만 커짐
        while(Input.GetKey(m_playerManager.ChangeViewKey))
        {
            // 최대제한 z크기보다 작을경우에만 크게 만들기
            if (m_changeViewRect_GO.transform.localScale.z < m_maxSizeZ)
            {
                // 크기변환
                m_changeViewRect_GO.transform.localScale += m_increaseVectorZ;

                // 이동
                Vector3 newPosition = m_changeViewRect_GO.transform.position;
                float newPositionZ = m_blueCubeSize.z * m_changeViewRect_GO.transform.localScale.z * 0.5f;
                newPosition.z = newPositionZ + blueCubePosition.z;
                m_changeViewRect_GO.transform.position = newPosition;
            }

            yield return new WaitForFixedUpdate();
        }

        // 월드 모든 오브젝트의 renderer 끄기
        GameManager.Instance.WorldManager.RendererEnable(false);

        // changeBox 안에 오브젝트만 renderer 및 collider2D 켜기
        m_changeViewRect_S.IncludeWOEnable(true);
        // 메테리얼 원래상태로 만들기
        m_changeViewRect_S.SetDefaultMaterial();

        // 플레이어 변경
        m_playerManager.ChangePlayer();

        // 블루큐브 변경
        GameManager.Instance.BlueCubeManager.ChangeCube();

        // 카메라 무빙워크 (쿼터뷰에서 사이드뷰로 이동)
        yield return StartCoroutine(GameManager.Instance.CameraManager.MovingWork3D());

        // 2D 벽 생성
        Wall2D_SetActive(true);

        // 모든 설정이 끝나면 2D 변경 상자를 비활성화
        m_changeViewRect_GO.SetActive(false);

        // 그림자 끄기
        GameManager.Instance.LightManager.ShadowEnable(false);

        m_isChnaging = false;
    }

    // 3D로 변경
    private IEnumerator ChangeView3D()
    {
        m_isChnaging = true;
        m_viewType = m_view3D;


        // 2D 벽 삭제
        Wall2D_SetActive(false);

        // 충돌 체크 끄기
        m_changeViewRect_S.CheckIncludeWO(false);
        // 2D 변경 상자 켜기
        m_changeViewRect_GO.SetActive(true);

        // changeBox 안에 오브젝트만 renderer 및 collider2D 끄기
        m_changeViewRect_S.IncludeWOEnable(false);

        // 월드의 모든 오브젝트의 renderer 켜기
        GameManager.Instance.WorldManager.RendererEnable(true);

        // 플레이어 변경
        m_playerManager.ChangePlayer();

        // 블루큐브 변경
        GameManager.Instance.BlueCubeManager.ChangeCube();

        // 그림자 켜기
        GameManager.Instance.LightManager.ShadowEnable(true);

        // 카메라 무빙워크 (사이드뷰에서 쿼터뷰로 이동)
        yield return StartCoroutine(GameManager.Instance.CameraManager.MovingWork2D());

        // z 감소수치 새로 계산
        // z 감소수치는 x,y랑 똑같이 줄게하기위해 x,y 증가수치로 계산
        m_decreaseVector.z = -((m_changeViewRect_GO.transform.localScale.z - m_blueCubeSize.z) * m_increaseSizePerXY * 0.01f);

        // 2D 변경 상자 크기 줄이기
        while (true)
        {
            // 크기 줄이기
            m_changeViewRect_GO.transform.localScale += m_decreaseVector;

            // 블루큐브 사이즈만큼 작아지면 다음으로 넘어감
            if (m_changeViewRect_GO.transform.localScale.x <= m_blueCubeSize.x)
                break;

            yield return null;
        }

        // 2D 변경 상자 끄기
        m_changeViewRect_GO.SetActive(false);

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
            m_wallPos = m_changeViewRect_GO.transform.position;
            m_wallPos.x -= m_maxSizeX / 2 + 1;
            m_leftWall2D.transform.position = m_wallPos;

            // 오른쪽 벽 위치 구하기
            m_wallPos.x += m_maxSizeX + 2;
            m_rightWall2D.transform.position = m_wallPos;

            // 위쪽 벽 위치 구하기
            m_wallPos = m_changeViewRect_GO.transform.position;
            m_wallPos.y += m_maxSizeY / 2 + 1;
            m_upWall2D.transform.position = m_wallPos;
        }
    }
}
