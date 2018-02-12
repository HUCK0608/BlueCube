using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraManager : MonoBehaviour
{
    // 카메라
    private Camera m_camera;

    // 카메라 센터포인트
    private Transform m_centerPoint;

    // 회전값 벡터
    private Vector3 m_rotation2D;
    private Vector3 m_rotation3D;

    // 이동속도
    [SerializeField]
    private float m_moveSpeed;

    // 회전속도
    [SerializeField]
    private float m_rotateSpeed;

    // 이동 최대 제한 거리 (플레이어와 이동지점 거리)
    [SerializeField]
    private float m_moveMaxDis;

    private void Awake()
    {
        m_camera = transform.Find("CenterPoint").Find("Camera").GetComponent<Camera>();

        m_centerPoint = transform.Find("CenterPoint");

        m_rotation2D = Vector3.zero;
        m_rotation3D = m_centerPoint.localEulerAngles;
    }

    private void Update()
    {
        FollowPlayer3D();
    }

    // 3D 플레이어를 따라가는 카메라
    private void FollowPlayer3D()
    {
        transform.position = GameManager.Instance.PlayerManager.Player3D_GO.transform.position;
    }

    // 해당 지점으로 카메라 좌표 이동
    public void MoveToDirection(Vector3 direction)
    {
        // 센터포인트에서 해당뱡향 최대값까지 가게 만듬
        Vector3 movePoint = direction * m_moveMaxDis;

        m_centerPoint.localPosition = Vector3.Lerp(m_centerPoint.localPosition, movePoint, m_moveSpeed * Time.deltaTime);
    }

    // 카메라 무빙워크 (쿼터뷰에서 사이드뷰로 이동)
    public IEnumerator MovingWork3D()
    {
        while (true)
        {
            m_centerPoint.localRotation = Quaternion.RotateTowards(m_centerPoint.localRotation, Quaternion.Euler(m_rotation2D), m_rotateSpeed * Time.deltaTime);

            if (m_centerPoint.localRotation.Equals(Quaternion.Euler(m_rotation2D)))
                break;

            yield return null;
        }
    }

    // 카메라 무빙워크 (사이드뷰에서 쿼터뷰로 이동)
    public IEnumerator MovingWork2D()
    {
        while(true)
        {
            m_centerPoint.localRotation = Quaternion.RotateTowards(m_centerPoint.localRotation, Quaternion.Euler(m_rotation3D), m_rotateSpeed * Time.deltaTime);

            if (m_centerPoint.localRotation.Equals(Quaternion.Euler(m_rotation3D)))
                break;

            yield return null;
        }
    }
}
