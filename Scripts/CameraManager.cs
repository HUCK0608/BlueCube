using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraManager : MonoBehaviour
{
    // 2D 카메라, 3D 카메라
    private Camera m_camera2D, m_camera3D;

    // 2D 플레이어, 3D 플레이어
    private GameObject m_player2D, m_player3D;

    private Transform m_centerPoint;

    private GameObject m_changeViewEffect2D;
    private GameObject m_changeViewEffect3D;

    [SerializeField]
    private float m_rotateSpeed;

    private void Awake()
    {
        m_camera2D = transform.Find("2D").GetComponent<Camera>();
        m_camera3D = transform.Find("CenterPoint").Find("3D").GetComponent<Camera>();

        m_player2D = GameObject.Find("Player").transform.Find("2D").gameObject;
        m_player3D = GameObject.Find("Player").transform.Find("3D").gameObject;

        m_centerPoint = transform.Find("CenterPoint");

        m_changeViewEffect2D = m_camera2D.transform.Find("ChangeViewEffect").gameObject;
        m_changeViewEffect3D = m_camera3D.transform.Find("ChangeViewEffect").gameObject;
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

    // 3D 무빙워크
    public IEnumerator MovingWork3D()
    {
        while (true)
        {
            m_centerPoint.localRotation = Quaternion.RotateTowards(m_centerPoint.localRotation, Quaternion.Euler(Vector3.zero), m_rotateSpeed * Time.deltaTime);

            if (m_centerPoint.localRotation == Quaternion.Euler(Vector3.zero))
                break;

            yield return null;
        }
    }

    public void ChangeViewEffect(bool value)
    {
        m_changeViewEffect3D.SetActive(value);
        m_changeViewEffect2D.SetActive(value);
    }
}
