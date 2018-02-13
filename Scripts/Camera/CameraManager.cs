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

    private float m_SXDivideSY;
    private float m_SYDivideSX;

    private void Awake()
    {
        m_camera = transform.Find("CenterPoint").Find("Camera").GetComponent<Camera>();

        m_centerPoint = transform.Find("CenterPoint");

        m_rotation2D = Vector3.zero;
        m_rotation3D = m_centerPoint.localEulerAngles;

        Camera mainCamera = Camera.main;

        float screenWidth = mainCamera.pixelWidth;
        float screenHeight = mainCamera.pixelHeight;

        m_SXDivideSY = screenWidth / screenHeight;
        m_SYDivideSX = screenHeight / screenWidth;
    }

    private void Update()
    {
        FollowPlayer3D();
        MoveToMouseDirection();
    }

    // 3D 플레이어를 따라가는 카메라
    private void FollowPlayer3D()
    {
        if(GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(GameLibrary.Enum_View3D))
            transform.position = GameManager.Instance.PlayerManager.Player3D_GO.transform.position;
    }

    // 마우스 포인터 위치의 방향을 구해서 카메라 이동
    public void MoveToMouseDirection()
    {
        // 3D이거나 시점 변환중이면 리턴
        if (!GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(GameLibrary.Enum_View3D) || GameManager.Instance.PlayerManager.Skill_CV.IsChanging)
            return;

        // 플레이어3D
        GameObject player3D = GameManager.Instance.PlayerManager.Player3D_GO;

        // 마우스 포인터 위치에서 플레이어의 스크린 좌표값을 빼줌
        Vector2 mousePosition = Input.mousePosition;
        // 플레이어를 스크린 좌표로 가져오기
        Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(player3D.transform.position);
        Vector2 mousePosInCenter = mousePosition - playerScreenPosition;

        // 마우스 x에 대한 위치 계산, 마우스 y가 증가할 때 x방향이 증가 z방향이 감소 (z방향은 마우스 y계산에 맞게하기위해 스크린 y / x 의 값을 곱해줌)
        Vector3 calcMouseX = (Vector3.right * mousePosInCenter.x) + (Vector3.back * m_SYDivideSX * mousePosInCenter.x);
        // 마우스 y에 대한 위치 계산, 마우스 y가 증가할 때 x방향이 증가 z방향이 증가 (x방향은 마우스 x계산에 맞게하기위해 스크린 x / y 의 값을 곱해줌)
        Vector3 calcMouseY = (Vector3.right * m_SXDivideSY * mousePosInCenter.y) + (Vector3.forward * mousePosInCenter.y);

        // 마우스 방향의 월드 방향 구하기
        Vector3 mouseDirection = calcMouseX + calcMouseY;

        // 이동방향 * 거리
        Vector3 movePoint = mouseDirection.normalized * m_moveMaxDis;

        // 이동
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
