using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraManager : MonoBehaviour
{
    // 카메라 센터포인트
    private Transform m_centerPoint;

    // 회전값 벡터
    private Vector3 m_rotation2D;
    private Vector3 m_rotation3D;

    // 카메라 위치
    [SerializeField]
    private Vector3 m_cameraPos2D;

    [SerializeField]
    private float m_movingWordkMoveSpeed;

    // 회전속도
    [SerializeField]
    private float m_movingWorkdRotSpeed;

    // 마우스 방향으로 이동하는 속도
    [SerializeField]
    private float m_moveDirecitonSpeed;

    // 이동 최대 제한 거리 (플레이어와 이동지점 거리)
    [SerializeField]
    private float m_moveDirectionMaxDis;

    // 관찰용시점 입력 키
    [SerializeField]
    private KeyCode m_observeViewKey;

    // 관찰용시점 최소, 최대 허용 각
    [SerializeField]
    private float m_observeViewMinAngle, m_observeViewMaxAngle;

    // 관찰용시점 민감도
    [SerializeField]
    private float m_observeViewSensitivity;

    // 원래시점으로 돌아오는 회전 속도
    [SerializeField]
    private float m_returnDefaultViewRotationSpeed;

    // 카메라의 기본 EulerAngles
    private Vector3 m_cameraDefualtEulerAngles;

    // 관찰중인지
    private bool m_isObserve;
    /// <summary>현재 관찰용 시점인지 체크(관찰중일 경우 true를 반환)</summary>
    public bool IsObserve { get { return m_isObserve; } }

    private void Awake()
    {
        m_centerPoint = transform.Find("CenterPoint");

        m_rotation2D = Vector3.zero;
        m_rotation3D = m_centerPoint.localEulerAngles;

        Camera mainCamera = Camera.main;

        float screenWidth = mainCamera.pixelWidth;
        float screenHeight = mainCamera.pixelHeight;

        m_cameraDefualtEulerAngles = m_centerPoint.eulerAngles;
    }

    private void Update()
    {
        FollowPlayer3D();
        MoveToMouseDirection();
        CheckObserveView();
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
        // 시점변환중이거나 2D시점이거나 관찰시점일 경우 리턴
        if (GameLibrary.Bool_IsCOV2D)
            return;

        // 마우스 방향의 월드 방향 구하기
        Vector3 mouseDirection = GetMouseDirectionToWorld();

        // 이동방향 * 거리
        Vector3 movePoint = mouseDirection.normalized * m_moveDirectionMaxDis;

        // 이동
        m_centerPoint.localPosition = Vector3.Lerp(m_centerPoint.localPosition, movePoint, m_moveDirecitonSpeed * Time.deltaTime);
    }

    // 마우스의 월드방향을 구하는 함수
    public Vector3 GetMouseDirectionToWorld()
    {
        // 플레이어3D
        GameObject player3D = GameManager.Instance.PlayerManager.Player3D_GO;

        // y축을 보고있고 플레이어위치에 평면을 생성
        Plane plane = new Plane(Vector3.up, player3D.transform.position);

        // 마우스 위치의 광선 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 충돌된 거리를 담을 변수
        float rayDistance;

        // 충돌 위치를 담을 변수
        Vector3 hitPoint = Vector3.zero;

        // 평면에서 광선 발사
        if(plane.Raycast(ray, out rayDistance))
        {
            // 충돌 위치 구하기
            hitPoint = ray.GetPoint(rayDistance);
        }

        // 방향 계산
        Vector3 direction = hitPoint - player3D.transform.position;

        // 방향 반환
        return direction.normalized;
    }

    // 관찰시점을 사용할것인지 체크
    private void CheckObserveView()
    {
        if(!m_isObserve)
        {
            if(Input.GetKeyDown(m_observeViewKey))
            {
                StartCoroutine(ObserveView());
            }
        }
    }

    // 관찰시점
    private IEnumerator ObserveView()
    {
        m_isObserve = true;

        Vector3 newCameraAngle = m_centerPoint.eulerAngles;

        // 관찰시점 키가 눌러있을 경우에만 루턴
        while(Input.GetKey(m_observeViewKey))
        {
            float mouseX = Input.GetAxis("Mouse X") * m_observeViewSensitivity;

            newCameraAngle.y -= mouseX;

            newCameraAngle.y = Mathf.Clamp(newCameraAngle.y, m_observeViewMinAngle, m_observeViewMaxAngle);

            m_centerPoint.eulerAngles = newCameraAngle;

            yield return null;
        }

        // 원래시점으로 돌아가게 함
        yield return StartCoroutine(ReturnDefaultView());

        m_isObserve = false;
    }

    // 관찰시점에서 원래시점으로 돌아가기
    private IEnumerator ReturnDefaultView()
    {
        Quaternion cameraDefaultQuaternion = Quaternion.Euler(m_cameraDefualtEulerAngles);

        while(true)
        {
            m_centerPoint.rotation = Quaternion.RotateTowards(m_centerPoint.rotation, cameraDefaultQuaternion, m_returnDefaultViewRotationSpeed * Time.deltaTime);

            if (m_centerPoint.rotation.Equals(cameraDefaultQuaternion))
                break;

            yield return null;
        }
    }

    // 카메라 무빙워크 (쿼터뷰에서 사이드뷰로 이동)
    public IEnumerator MovingWork3D()
    {
        while(true)
        {
            m_centerPoint.localPosition = Vector3.MoveTowards(m_centerPoint.localPosition, m_cameraPos2D, 10f * Time.deltaTime);

            if (m_centerPoint.localPosition.Equals(m_cameraPos2D))
                break;

            yield return null;
        }

        while (true)
        {
            m_centerPoint.localRotation = Quaternion.RotateTowards(m_centerPoint.localRotation, Quaternion.Euler(m_rotation2D), m_movingWorkdRotSpeed * Time.deltaTime);

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
            m_centerPoint.localRotation = Quaternion.RotateTowards(m_centerPoint.localRotation, Quaternion.Euler(m_rotation3D), m_movingWorkdRotSpeed * Time.deltaTime);

            if (m_centerPoint.localRotation.Equals(Quaternion.Euler(m_rotation3D)))
                break;

            yield return null;
        }
    }
}
