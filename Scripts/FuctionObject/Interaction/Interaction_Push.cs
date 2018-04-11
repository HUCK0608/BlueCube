using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Interaction_Push : MonoBehaviour
{
    private static Transform m_fadeBox;
    private static Material m_fadeBoxMaterial;

    // 고정 위치 및 고정위치 개수
    [SerializeField]
    private List<Transform> m_hangPoints;
    private static int m_hangPointCount = 4;

    // 밀기 이동 속도
    [SerializeField]
    private float m_pushMoveSpeed;

    private bool m_isCanPush;
    /// <summary>오브젝트를 밀 수 있으면 true를 반환</summary>
    public bool IsCanPush { get { return m_isCanPush; } }

    private bool m_isPush;
    /// <summary>오브젝트가 밀리는 중일 경우 true를 반환</summary>
    public bool IsPush { get { return m_isPush; } }

    private void Awake()
    {
        InitFadeBox();
    }

    // 반투명 상자 초기화
    private void InitFadeBox()
    {
        if (m_fadeBox == null)
        {
            m_fadeBox = GameObject.Find("Interaction_PushBox_Fade").transform;
            m_fadeBoxMaterial = m_fadeBox.GetComponent<Material>();
        }
    }

    /// <summary>target에서 가장 가까운 고정 위치를 반환</summary>
    public Vector3 GetHangPosition(Vector3 target)
    {
        // 첫 번째 고정위치가 제일 가까운 위치라고 설정
        Vector3 nearPosition = m_hangPoints[0].position;
        float nearDistance = Vector3.Distance(target, nearPosition);

        // 나머지 고정 위치만큼 돌면서 제일 가까운 거리를 계산
        for(int i = 1; i < m_hangPointCount; i++)
        {
            float distance = Vector3.Distance(target, m_hangPoints[i].position);

            // 새로 구해진 거리가 기존의 가까운 거리보다 가까울 경우 새로운 위치값을 저장
            if(distance < nearDistance)
            {
                nearPosition = m_hangPoints[1].position;
                nearDistance = distance;
            }
        }

        StartCoroutine(DrawFadeBox());

        return nearPosition;
    }

    private IEnumerator DrawFadeBox()
    {
        while(true)
        {
            E_PlayerState3D playerCurrentState3D = PlayerManager.Instance.MainController.CurrentState3D;

            // PickIdle 상태에서만 그려줌
            if(playerCurrentState3D.Equals(E_PlayerState3D.PickIdle))
            {
            }

            // PickMove 상태면 그리기 종료
            if (playerCurrentState3D.Equals(E_PlayerState3D.PickMove))
                break;

            yield return null;
        }
    }

    /// <summary>밀릴 위치를 반환. 밀릴 수 없는 위치일 경우 (1, 1, 1)을 반환</summary>
    private Vector3 CalcPushPosition()
    {
        Vector3 pushPosition = Vector3.one;

        // 마우스 광선 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 레이어 마스크 설정
        int layerMask = GameLibrary.LayerMask_CanPushWay;

        RaycastHit hit;

        // 레이 발사
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // 플레이어가 바라보는 방향
            Vector3 playerDirection = PlayerManager.Instance.SubController3D.Forward;
            // 충돌된 마우스 위치의 피벗 구하기
            Vector3 hitPivot = hit.point.GameNormalized();
        }

        return pushPosition;
    }
}
