using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BlueCubeManager : MonoBehaviour
{
    // 큐브
    private GameObject m_blueCube2D;
    private GameObject m_blueCube3D;

    // 플레이어
    private Player2D m_player2D;
    private Transform m_player3D;

    // 시점에 따른 큐브 고정위치
    [SerializeField]
    private Vector3 m_fixedPos2D;
    [SerializeField]
    private Vector3 m_fixedPos3D;

    SpriteRenderer m_renderer2D;

    Color32 m_redColor;
    Color32 m_blueColor;

    private Ray m_ray;
    private LayerMask m_layerMask;

    private void Awake()
    {
        m_blueCube2D = transform.Find("2D").gameObject;
        m_blueCube3D = transform.Find("3D").gameObject;

        m_player2D = GameObject.Find("Player").transform.Find("2D").GetComponent<Player2D>();
        m_player3D = GameObject.Find("Player").transform.Find("3D");

        m_renderer2D = m_blueCube2D.GetComponent<SpriteRenderer>();

        m_redColor = new Color32(255, 0, 0, 255);
        m_blueColor = new Color32(0, 0, 255, 255);

        m_layerMask = (-1) - ((1 << 8) | (1 << 11));
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
        if(GameManager.Instance.PlayerManager.Skill_CV.ViewType == E_ViewType.View2D)
        {
            m_blueCube3D.SetActive(false);
            m_blueCube2D.SetActive(true);
        }
        else if(GameManager.Instance.PlayerManager.Skill_CV.ViewType == E_ViewType.View3D)
        {
            m_blueCube2D.SetActive(false);
            m_blueCube3D.SetActive(true);
        }
    }

    // 큐브 고정
    private void FixedCube()
    {
        if (GameManager.Instance.PlayerManager.Skill_CV.ViewType == E_ViewType.View2D)
        {
            transform.position = m_player2D.transform.position + new Vector3(m_fixedPos2D.x * (int)m_player2D.LookDirection,
                                                                             m_fixedPos2D.y,
                                                                             m_fixedPos2D.z);
        }
        else if (GameManager.Instance.PlayerManager.Skill_CV.ViewType == E_ViewType.View3D)
        {
            transform.position = m_player3D.position + m_fixedPos3D;
        }
    }

    // 3D의 지형이 있는지 체크
    private void CheckGround3D()
    {
        m_ray = new Ray(m_player2D.transform.position + new Vector3(0, 0.5f, 0), Vector3.down);

        if (Physics.Raycast(m_ray, 1f, m_layerMask))
            m_renderer2D.color = m_blueColor;
        else
            m_renderer2D.color = m_redColor;
    }
}
