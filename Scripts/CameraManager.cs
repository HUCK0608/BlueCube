using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraManager : MonoBehaviour
{
    // 2D 카메라, 3D 카메라
    private Camera m_camera2D, m_camera3D;

    // 애니메이터
    private Animator m_animator;

    // 2D 플레이어, 3D 플레이어
    private GameObject m_player2D, m_player3D;

    private void Awake()
    {
        m_camera2D = transform.Find("2D").GetComponent<Camera>();
        m_camera3D = transform.Find("3D").GetComponent<Camera>();

        m_animator = GetComponent<Animator>();

        m_player2D = GameObject.Find("Player").transform.Find("2D").gameObject;
        m_player3D = GameObject.Find("Player").transform.Find("3D").gameObject;
    }

    private void Start()
    {
        ChangeCamera();
    }

    private void Update()
    {
        Move();
    }

    // 카메라 이동
    public void Move()
    {
        if(GameManager.Instance.ViewType == E_ViewType.View2D)
        {
            transform.position = m_player2D.transform.position;
        }
        else if(GameManager.Instance.ViewType == E_ViewType.View3D)
        {
            transform.position = m_player3D.transform.position;
        }
    }

    // 카메라 스위칭
    public void ChangeCamera()
    {
        // 3D카메라를 끈 후 2D카메라를 켬
        if(GameManager.Instance.ViewType == E_ViewType.View2D)
        {
            // 조준상태일 경우 조준 끄기
            if (m_animator.GetBool("isScoped"))
                ChangeScoped();

            m_camera3D.enabled = false;
            m_camera2D.enabled = true;
        }
        // 2D카메라를 끈 후 3D카메라를 켬
        else if(GameManager.Instance.ViewType == E_ViewType.View3D)
        {
            m_camera2D.enabled = false;
            m_camera3D.enabled = true;
        }
    }

    // 스코프 관련 (3D에서만 사용됨)
    public void ChangeScoped()
    {
        bool isScoped = m_animator.GetBool("isScoped");

        // 애니메이션 설정
        m_animator.SetBool("isScoped", !isScoped);
        // 에임설정
        GameManager.Instance.UIManager.SetAimEnabled(!isScoped);
    }
}
