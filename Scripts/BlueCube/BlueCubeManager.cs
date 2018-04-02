using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BlueCubeManager : MonoBehaviour
{
    private static BlueCubeManager m_instance;
    public static BlueCubeManager Instance { get { return m_instance; } }

    // 큐브
    private GameObject m_blueCube2D;
    private GameObject m_blueCube3D;
    public GameObject BlueCube3D { get { return m_blueCube3D; } }

    // 2D 랜더러
    SpriteRenderer m_renderer2D;

    // 레드, 블루 큐브
    Color32 m_redColor;
    Color32 m_blueColor;

    private void Awake()
    {
        m_instance = this;

        m_blueCube2D = transform.Find("2D").gameObject;
        m_blueCube3D = transform.Find("3D").gameObject;

        m_renderer2D = m_blueCube2D.GetComponent<SpriteRenderer>();

        m_redColor = Color.red;
        m_blueColor = Color.blue;
    }

    private void Start()
    {
        ChangeCube();
    }

    private void Update()
    {
        FixedCube();
        ChangeColor2D();
    }

    // 큐브 교체
    public void ChangeCube()
    {
        // 2D
        if(PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D))
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
        if (PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D))
        {
            // 2D플레이어 위치
            Vector3 playerPosition = PlayerManager.Instance.Player2D_Object.transform.position;

            // 이동
            transform.position = playerPosition;

            Vector3 newDirection = Vector3.one;
            newDirection.x = PlayerManager.Instance.SubController2D.Forward.x;

            // 방향회전
            transform.localScale = newDirection;
        }
        // 3D
        else
        {
            // 현재 스케일이 1, 1, 1이 아니면 1, 1, 1로 바꿈
            if (!transform.localScale.Equals(Vector3.one))
                transform.localScale = Vector3.one;

            // 3D 플레이어 위치
            Vector3 playerPosition = PlayerManager.Instance.Player3D_Object.transform.position;

            // 이동
            transform.position =  playerPosition;
        }
    }

    // 2D에서 3D위치에 땅이 있는지 체크해서 색을 바꿈
    private void ChangeColor2D()
    {
        // 2D일 경우에만 실행
        if (PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D))
        {
            // 3D상태에 무언가 있다면 파란색으로 바꿈
            if (PlayerManager.Instance.SubController3D.CheckGround.Check())
                m_renderer2D.color = m_blueColor;
            // 없다면 빨간색으로 바꿈
            else
                m_renderer2D.color = m_redColor;
        }
    }
}
