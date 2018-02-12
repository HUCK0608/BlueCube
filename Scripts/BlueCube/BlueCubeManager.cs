using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BlueCubeManager : MonoBehaviour
{
    // 큐브
    private GameObject m_blueCube2D;
    private GameObject m_blueCube3D;

    // 시점에 따른 큐브 고정위치
    [SerializeField]
    private Vector3 m_fixedPos2D;
    [SerializeField]
    private Vector3 m_fixedPos3D;

    // 2D 랜더러
    SpriteRenderer m_renderer2D;

    // 레드, 블루 큐브
    Color32 m_redColor;
    Color32 m_blueColor;

    // 땅 체크를 위한 변수
    private Ray m_ray;
    private float m_rayOriginY;

    private void Awake()
    {
        m_blueCube2D = transform.Find("2D").gameObject;
        m_blueCube3D = transform.Find("3D").gameObject;

        m_renderer2D = m_blueCube2D.GetComponent<SpriteRenderer>();

        m_redColor = Color.red;
        m_blueColor = Color.blue;

        InitCheckGorund3D();
    }

    // 땅 체크 변수 초기화
    private void InitCheckGorund3D()
    {
        m_ray = new Ray();
        m_ray.direction = Vector3.down;

        m_rayOriginY = 0.5f;
    }

    private void Start()
    {
        ChangeCube();
    }

    private void Update()
    {
        FixedCube();
        CheckGround3D();
    }

    // 큐브 교체
    public void ChangeCube()
    {
        // 2D
        if(GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(GameLibrary.Enum_View2D))
        {
            m_blueCube3D.SetActive(false);
            m_blueCube2D.SetActive(true);
        }
        // 3D
        else
        {
            m_blueCube2D.SetActive(false);
            m_blueCube3D.SetActive(true);
        }
    }

    // 큐브 고정
    private void FixedCube()
    {
        // 2D
        if (GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(GameLibrary.Enum_View2D))
        {
            // 2D방향을 담을 변수
            int direction = 0;

            // 왼쪽
            if (GameManager.Instance.PlayerManager.Player2D_S.LookDirection.Equals(GameLibrary.Enum_LD2D_Left))
                direction = -1;
            // 오른쪽
            else
                direction = 1;

            // 2D방향 적용
            m_fixedPos2D.x *= direction;

            // 2D 플레이어 위치
            Vector3 player2D_Position = GameManager.Instance.PlayerManager.Player2D_GO.transform.position;

            // 큐브위치 이동
            transform.position = player2D_Position + m_fixedPos2D;
        }
        // 3D
        else
        {
            // 3D 플레이어 위치
            Vector3 player3D_Position = GameManager.Instance.PlayerManager.Player3D_GO.transform.position;

            // 큐브위치 이동
            transform.position =  player3D_Position + m_fixedPos3D;
        }
    }

    // 3D의 지형이 있는지 체크
    private void CheckGround3D()
    {
        Vector3 rayOrigin = GameManager.Instance.PlayerManager.Player2D_GO.transform.position;
        rayOrigin.y += m_rayOriginY;

        m_ray.origin = rayOrigin;

        float rayDistance = 1f;

        if (Physics.Raycast(m_ray, rayDistance, GameLibrary.IgonoreLM_PEE))
            m_renderer2D.color = m_blueColor;
        else
            m_renderer2D.color = m_redColor;
    }
}
