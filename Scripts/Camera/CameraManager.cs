using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraManager : MonoBehaviour
{
    private static CameraManager m_instance;
    public static CameraManager Instance { get { return m_instance; } }

    // 애니메이터 및 파라미터 명 변수
    private Animator m_animator;
    private static string m_animatorParameterName = "CurrentView";

    // 무빙워크중이면 true를 반환
    private bool m_isOnMovingWork;

    private void Awake()
    {
        m_instance = this;

        m_animator = GetComponent<Animator>();
    }

    /// <summary>2D 상태로 시작</summary>
    public void StartView2D()
    {
        transform.position = PlayerManager.Instance.Player2D_Object.transform.position;
        m_animator.Play("View2D_Idle", -1, 0f);
        m_animator.SetInteger(m_animatorParameterName, 1);
    }

    private void LateUpdate()
    {
        FollowPlayer3D();
        PlayGanziCam();
    }

    private void PlayGanziCam()
    {
        if (Input.GetKeyDown(KeyCode.C))
            m_animator.Play("GanziCam", -1);
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

    /// <summary>카메라 무빙워크 시작</summary>
    public IEnumerator StartMovingWork()
    {
        // 애니메이션 실행
        m_animator.SetInteger(m_animatorParameterName, (int)PlayerManager.Instance.CurrentView);

        m_isOnMovingWork = true;

        // 무빙워크가 완료될 때 까지 기다림
        yield return new WaitUntil(() => !m_isOnMovingWork);
    }

    /// <summary>카메라 무빙워크가 끝났다고 설정(애니메이션에서 사용)</summary>
    public void CompleteMovingWork()
    {
        m_isOnMovingWork = false;
    }

    /// <summary>플레이어를 2D상태로 변경(애니메이션에서 사용)</summary>
    public void ChangePlayer2D()
    {
        PlayerManager.Instance.PlayerChange2D();
    }
}
