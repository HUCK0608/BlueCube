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

    private void Awake()
    {
        m_blueCube2D = transform.Find("2D").gameObject;
        m_blueCube3D = transform.Find("3D").gameObject;

        m_player2D = GameObject.Find("Player").transform.Find("2D").GetComponent<Player2D>();
        m_player3D = GameObject.Find("Player").transform.Find("3D");
    }

    private void Start()
    {
        ChangeCube();
    }

    private void Update()
    {
        FixedCube();
    }

    // 큐브 교체
    public void ChangeCube()
    {
        if(GameManager.Instance.ViewType == E_ViewType.View2D)
        {
            m_blueCube3D.SetActive(false);
            m_blueCube2D.SetActive(true);
        }
        else if(GameManager.Instance.ViewType == E_ViewType.View3D)
        {
            m_blueCube2D.SetActive(false);
            m_blueCube3D.SetActive(true);
        }
    }

    // 큐브 고정
    private void FixedCube()
    {
        if (GameManager.Instance.ViewType == E_ViewType.View2D)
        {
            transform.position = m_player2D.transform.position + new Vector3(m_fixedPos2D.x * (int)m_player2D.LookDirection,
                                                                             m_fixedPos2D.y,
                                                                             m_fixedPos2D.z);
        }
        else if (GameManager.Instance.ViewType == E_ViewType.View3D)
        {
            transform.position = m_player3D.position + m_fixedPos3D;
        }
    }
}
