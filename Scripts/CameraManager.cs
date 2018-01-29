using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraManager : MonoBehaviour
{
    // 카메라
    private Camera m_camera;

    // 3D 플레이어
    private GameObject m_player3D;

    private Transform m_centerPoint;

    [SerializeField]
    private float m_rotateSpeed;

    private void Awake()
    {
        m_camera = transform.Find("CenterPoint").Find("Camera").GetComponent<Camera>();

        m_player3D = GameObject.Find("Player").transform.Find("3D").gameObject;

        m_centerPoint = transform.Find("CenterPoint");
    }

    private void Update()
    {
        Move3D();
    }

    // 3D 에서의 카메라 이동
    public void Move3D()
    {
        if (GameManager.Instance.ViewType != E_ViewType.View3D)
            return;

        transform.position = m_player3D.transform.position;
    }

    // 카메라 무빙워크 (쿼터뷰에서 사이드뷰로 이동)
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
}
