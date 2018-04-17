using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public sealed class Interaction_Push : MonoBehaviour
{
    private Collider m_collider;
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
        m_collider = GetComponentInChildren<Collider>();

        InitFadeBox();
    }

    // 반투명 상자 초기화
    private void InitFadeBox()
    {
        if (m_fadeBox == null)
        {
            m_fadeBox = GameObject.Find("Interaction_PushBox_Fade").transform;
            m_fadeBoxMaterial = m_fadeBox.GetComponent<MeshRenderer>().material;
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
                nearPosition = m_hangPoints[i].position;
                nearDistance = distance;
            }
        }

        // 밀기 로직 실행
        StartCoroutine(PushObjectLogic());

        return nearPosition;
    }

    // 밀기 로직
    private IEnumerator PushObjectLogic()
    {
        while(true)
        {
            E_PlayerState3D currentPlayerState = PlayerManager.Instance.MainController.CurrentState3D;
            // 플레이어 상태가 PushIdle 상태일 때
            if (currentPlayerState.Equals(E_PlayerState3D.PushIdle))
            {
                // 이동할 위치를 계산하여 구함
                Vector3 pushPosition = CalcPushPosition();
                // pushPosition 위치로 밀 수 있는지 체크
                m_isCanPush = CheckCanPush(pushPosition);
                // pushPosition을 시각화
                DrawFadeBox(pushPosition);
            }
            // 플레이어 상태가 PushMove 상태일 때
            else if(currentPlayerState.Equals(E_PlayerState3D.PushMove))
            {
                // 이동 코루틴 실행
                StartCoroutine(MovePushPosition(m_fadeBox.position));
                break;
            }
            // 플레이어 상태가 Idle 상태일 때
            else if(currentPlayerState.Equals(E_PlayerState3D.Idle))
            {
                break;
            }

            yield return null;
        }

        // FadeBox 비활성화
        m_fadeBox.localPosition = Vector3.zero;
    }

    // PushPosition으로 이동
    private IEnumerator MovePushPosition(Vector3 pushPosition)
    {
        // 이동중이라고 설정
        m_isPush = true;

        // 위쪽에 있는 다른 아이템을 같이 이동시키기 위해 자식으로 포함
        int layerMask = GameLibrary.LayerMask_InteractionPickPut;
        Debug.Log(layerMask);
        RaycastHit[] anotherItems = Physics.RaycastAll(transform.position, Vector3.up, Mathf.Infinity, layerMask);
        Debug.Log(anotherItems.Length);
        int anotherItemCount = anotherItems.Length;
        for (int i = 0; i < anotherItemCount; i++)
        {
            Debug.Log("call");
            anotherItems[i].transform.parent = transform;
        }

        while (true)
        {
            // 이동
            transform.position = Vector3.MoveTowards(transform.position, pushPosition, m_pushMoveSpeed * Time.deltaTime);

            // 이동위치까지 이동했을경우 반복문 종료
            if (transform.position.Equals(pushPosition))
                break;

            yield return null;
        }

        // 포함되었던 다른 아이템을 제외시킴
        for (int i = 0; i < anotherItemCount; i++)
            anotherItems[i].transform.parent = transform.parent;

        // 이동이 끝났다고 설정
        m_isPush = false;
    }

    // pushPosition에 FadeBox를 그려줌
    private void DrawFadeBox(Vector3 pushPosition)
    {
        // pushPosition이 갈 수 없는 위치일 경우 그려주지 않음
        if (pushPosition.Equals(Vector3.one))
        {
            m_fadeBox.localPosition = Vector3.one;
            return;
        }

        Color newColor = Color.white;

        // 그려진 위치로 이동 시킬 수 있는지 판단하여 Fadebox 색상 결정
        if (m_isCanPush)
        {
            newColor = Color.white;
            newColor.a = m_fadeBoxMaterial.color.a;
        }
        else
        {
            newColor = Color.red;
            newColor.a = m_fadeBoxMaterial.color.a;
        }

        m_fadeBoxMaterial.color = newColor;

        m_fadeBox.position = pushPosition;
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
            Vector3 hitPivot = hit.point.GetGamePivot();

            float one = 1f;

            // 플레이어의 방향이 x 양의 방향일 때
            if(Mathf.Round(playerDirection.x).Equals(one))
            {
                // 피벗의 x가 현재위치의 x보다 크고 피벗의 z는 현재위치의 z랑 같을 때
                if(hitPivot.x > transform.position.x && hitPivot.z.Equals(transform.position.z))
                {
                    pushPosition = hitPivot;
                    pushPosition.y = transform.position.y;
                }
            }
            // 플레이어의 방향이 x 음의 방향일 때
            else if(Mathf.Round(playerDirection.x).Equals(-one))
            {
                // 피벗의 x가 현재위치의 x보다 작고 피벗의 z는 현재위치의 z랑 같을 때
                if(hitPivot.x < transform.position.x && hitPivot.z.Equals(transform.position.z))
                {
                    pushPosition = hitPivot;
                    pushPosition.y = transform.position.y;
                }
            }
            // 플레이어의 방향이 z 양의 방향일 때
            else if(Mathf.Round(playerDirection.z).Equals(one))
            {
                // 피벗의 z가 현재위치의 z보다 크고 피벗의 x는 현재위치의 x랑 같을 때
                if(hitPivot.z > transform.position.z && hitPivot.x.Equals(transform.position.x))
                {
                    pushPosition = hitPivot;
                    pushPosition.y = transform.position.y;
                }
            }
            // 플레이어의 방향이 z 음의 방향일 때
            else if(Mathf.Round(playerDirection.z).Equals(-one))
            {
                // 피벗의 z가 현재위치의 z보다 작고 피벗의 x는 현재위치의 x랑 같을 때
                if (hitPivot.z < transform.position.z && hitPivot.x.Equals(transform.position.x))
                {
                    pushPosition = hitPivot;
                    pushPosition.y = transform.position.y;
                }
            }
        }

        return pushPosition;
    }

    /// <summary>pushPosition으로 밀 수 있을 경우 true를 반환</summary>
    private bool CheckCanPush(Vector3 pushPosition)
    {
        float temp = 0.1f;
        float boundXZ = m_collider.bounds.extents.x;
        float boundY = m_collider.bounds.extents.y;

        float distance = Vector3.Distance(pushPosition, transform.position) + (boundXZ - temp);
        Vector3 direction = pushPosition - transform.position;
        direction = direction.normalized;

        // 무시할 레이어 마스크
        int layerMask = (-1) - (GameLibrary.LayerMask_Player |
                                     GameLibrary.LayerMask_Bullet |
                                     GameLibrary.LayerMask_IgnoreRaycast |
                                     GameLibrary.LayerMask_BackgroundTrigger);
        
        // 최소 최대 위치 구하기
        Vector3 minCheckPosition = transform.position;
        Vector3 maxCheckPosition = transform.position;

        minCheckPosition.y -= (boundY - temp);
        maxCheckPosition.y += (boundY - temp);

        // 최소 최대 위치 체크
        if (GameLibrary.Raycast3D(minCheckPosition, direction, distance, layerMask))
            return false;
        else if (GameLibrary.Raycast3D(maxCheckPosition, direction, distance, layerMask))
            return false;

        // 최소 최대 위치에서 체크를 했을 때 아무것도 없을 경우 제일 아래 피벗 높이부터 체크
        float startCheckPositionY = transform.position.y - boundY + 1f;

        Vector3 checkPosition = transform.position;
        checkPosition.y = startCheckPositionY;

        float two = 2f;

        while(true)
        {
            // 각 피벗 높이에서 체크를 하면서 무언가 충돌할 경우 충돌했다고 알림
            if (GameLibrary.Raycast3D(checkPosition, direction, distance, layerMask))
                return false;

            // 다음 피벗 높이로 이동
            checkPosition.y += two;

            // 만약 체크 높이가 현재 오브젝트의 최대 높이를 넘어갈 경우 반복문 종료
            if (checkPosition.y > transform.position.y + boundY)
                break;
        }

        return true;
    }
}
