﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraManager : MonoBehaviour
{
    private static CameraManager m_instance;
    public static CameraManager Instance { get { return m_instance; } }

    private Camera m_camera;
    private Animator m_animator;
    private static string m_animatorParameterName = "CurrentView";

    // 카메라 센터포인트
    private Transform m_centerPoint;

    // 회전값 벡터
    private Vector3 m_rotation2D;
    private Vector3 m_rotation3D;

    // 카메라 위치
    [SerializeField]
    private Vector3 m_cameraPos2D;
    [SerializeField]
    private Vector3 m_cameraPos3D;

    [SerializeField]
    private float m_movingWorkMoveSpeed;

    // 회전속도
    [SerializeField]
    private float m_movingWorkRotSpeed;
    
    // 무빙워크 각도회전할 때 체크할 최소 값
    [SerializeField]
    private float m_movingWorkSlerpCheckDistance;

    // 마우스 방향으로 이동하는 속도
    [SerializeField]
    private float m_moveDirecitonSpeed;

    // 이동 최대 제한 거리 (플레이어와 이동지점 거리)
    [SerializeField]
    private float m_moveDirectionMaxDis;

    // 관찰용시점 입력 키
    [SerializeField]
    private KeyCode m_observeViewLeftKey;
    [SerializeField]
    private KeyCode m_observeViewRightKey;

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

    private bool m_isOnMovingWork;

    private void Awake()
    {
        m_instance = this;

        m_camera = GetComponentInChildren<Camera>();
        m_animator = GetComponent<Animator>();

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
        CheckObserveView();
    }

    // 3D 플레이어를 따라가는 카메라
    private void FollowPlayer3D()
    {
        if(PlayerManager.Instance.CurrentView.Equals(E_ViewType.View3D))
            transform.position = PlayerManager.Instance.Player3D_Object.transform.position;
    }

    /// <summary>pivot에서 마우스 위치의 방향을 구함</summary>
    public Vector3 GetMouseDirectionToPivot(Vector3 pivot)
    {
        // 법선이 y양의 방향을 보고있고 pivot위치에 있는 평면을 생성
        Plane plane = new Plane(Vector3.up, pivot);

        // 마우스 위치의 광선 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 충돌된 거리를 담을 변수
        float rayDistance;

        // 충돌 위치를 담을 변수
        Vector3 hitPoint = Vector3.zero;

        // 평면에서 광선 발사
        if (plane.Raycast(ray, out rayDistance))
        {
            // 충돌 위치 구하기
            hitPoint = ray.GetPoint(rayDistance);
        }

        Vector3 direction = hitPoint - pivot;

        //위치 반환
        return direction.normalized;
    }

    /// <summary>pivot높이를 중심으로 마우스의 충돌위치를 구함</summary>
    public Vector3 GetMouseHitPointToPivot(Vector3 pivot)
    {
        // 법선이 y양의 방향을 보고있고 pivot위치에 있는 평면을 생성
        Plane plane = new Plane(Vector3.up, pivot);

        // 마우스 위치의 광선 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 충돌된 거리를 담을 변수
        float rayDistance;

        // 충돌 위치를 담을 변수
        Vector3 hitPoint = Vector3.zero;

        // 평면에서 광선 발사
        if (plane.Raycast(ray, out rayDistance))
        {
            // 충돌 위치 구하기
            hitPoint = ray.GetPoint(rayDistance);
        }

        //위치 반환
        return hitPoint;
    }

    // 관찰시점을 사용할것인지 체크
    private void CheckObserveView()
    {
        // 시점변환 준비중이거나 시점변환중이거나 2D이거나 땅이아닐경우 탐지시점 발동
        if(!PlayerManager.Instance.IsViewChangeReady &&
           !PlayerManager.Instance.IsViewChange && 
           !PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D)
           && PlayerManager.Instance.IsGrounded)
        {
            if(Input.GetKeyDown(m_observeViewLeftKey) || Input.GetKeyDown(m_observeViewRightKey))
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
        while(true)
        {
            // 두개의 키 입력 받기
            bool isOnLeftKey = Input.GetKey(m_observeViewLeftKey);
            bool isOnRightKey = Input.GetKey(m_observeViewRightKey);

            // 두개의 키가 모두 입력되지 않았을경우 반복문 종료
            if (!isOnLeftKey && !isOnRightKey)
                break;

            if (isOnLeftKey)
                newCameraAngle.y += m_observeViewSensitivity;
            else if (isOnRightKey)
                newCameraAngle.y -= m_observeViewSensitivity;

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

    /// <summary>카메라 무빙워크 시작</summary>
    public IEnumerator StartMovingWork()
    {
        // 애니메이션 실행
        m_animator.SetInteger(m_animatorParameterName, (int)PlayerManager.Instance.CurrentView);

        m_isOnMovingWork = true;

        // 카메라 플레이어 위치로 이동
        while (m_isOnMovingWork)
        {
            yield return null;
        }
    }

    /// <summary>카메라 무빙워크가 끝났다고 설정</summary>
    public void CompleteMovingWork()
    {
        m_isOnMovingWork = false;
    }
}
