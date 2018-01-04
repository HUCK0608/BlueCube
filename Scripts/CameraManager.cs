using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraManager : MonoBehaviour
{
    // 마우스 민감도
    [SerializeField]
    private float m_sensitivity;

    // 최소, 최대각
    [SerializeField]
    private float m_minAngle, m_maxAngle;

    // 3D 카메라 중심
    private Transform m_centerPoint3D;

    // 카메라 회전값
    private float m_mouseX, m_mouseY;

    // 2D 카메라, 3D 카메라
    private Camera m_camera2D, m_camera3D;

    // 애니메이터
    private Animator m_animator;

    // 2D 플레이어, 3D 플레이어
    private GameObject m_player2D, m_player3D;

    private void Awake()
    {
        m_centerPoint3D = transform.Find("CenterPoint3D");

        m_camera2D = GameObject.Find("Camera2D").GetComponent<Camera>();
        m_camera3D = GameObject.Find("Camera3D").GetComponent<Camera>();

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
        Rotation3D();
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

    // 카메라 회전
    public void Rotation3D()
    {
        if (GameManager.Instance.ViewType != E_ViewType.View3D)
            return;

        m_mouseX += Input.GetAxis("Mouse X") * m_sensitivity;
        m_mouseY -= Input.GetAxis("Mouse Y") * m_sensitivity;

        // y축 회전값 제한
        m_mouseY = Mathf.Clamp(m_mouseY, m_minAngle, m_maxAngle);

        Vector3 rotation = new Vector3(m_mouseY, m_mouseX, 0);

        m_centerPoint3D.eulerAngles = rotation;
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

    // y축 각도 리턴
    public float GetYAngle3D() { return m_centerPoint3D.eulerAngles.y; }
}
