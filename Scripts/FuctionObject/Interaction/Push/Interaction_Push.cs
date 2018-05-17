using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Push : MonoBehaviour
{
    [Header("* Push Settings")]
    [Space(-5f)]
    [Header("Can Change")]
    /// <summary>현재위치에서 고정될 위치까지의 거리</summary>
    [SerializeField]
    private float m_hangDistanceToThis = 1.75f;
    /// <summary>이동 속도</summary>
    [SerializeField]
    private float m_moveSpeed = 10f;

    [Header("Don't touch")]
    /// <summary>반투명 오브젝트</summary>
    [SerializeField]
    private GameObject m_fadeObject;
    private Material m_fadeObjectMaterial;
    /// <summary>모델 콜라이더</summary>
    [SerializeField]
    private Collider m_modelCollider;

    /// <summary>고정 위치 모음</summary>
    private Vector3[] m_hangPositions;

    protected bool m_isCanPush;
    /// <summary>밀 수 있을 때 true를 반환</summary>
    public bool IsCanPush { get { return m_isCanPush; } }

    private bool m_isMove;
    /// <summary>오브젝트가 움직이는 중일 경우 true를 반환</summary>
    public bool IsMove { get { return m_isMove; } }

    protected virtual void Awake()
    {
        m_fadeObjectMaterial = m_fadeObject.GetComponent<MeshRenderer>().material;

        m_hangPositions = new Vector3[4];
    }

    /// <summary>플레이어 위치에서 제일 가까운 고정 위치를 반환</summary>
    public Vector3 GetNearHangPosition(Vector3 playerPosition)
    {
        // 고정 위치 계산
        CalcHangPosition();

        // 첫번째 고정 위치가 제일 가깝다고 설정
        float nearDistance = Vector3.Distance(m_hangPositions[0], playerPosition);
        int nearIndex = 0;

        // 나머지 고정 위치를 돌면서 제일 가까운 지점을 찾음
        for(int i = 1; i < 4; i ++)
        {
            float distance = Vector3.Distance(m_hangPositions[i], playerPosition);

            if(distance < nearDistance)
            {
                nearDistance = distance;
                nearIndex = i;
            }
        }

        // 구한 가까운 위치의 높이를 플레이어와 같게 만듬
        Vector3 nearHangPosition = m_hangPositions[nearIndex];
        nearHangPosition.y = playerPosition.y;

        // 밀기 로직 실행
        StartCoroutine(PushLogic());

        return nearHangPosition;
    }


    /// <summary>고정 위치 계산</summary>
    private void CalcHangPosition()
    {
        float one = 1f;

        // 배열에 현재위치값을 모두 넣음
        GameLibrary.SetSameValue(ref m_hangPositions, transform.position);

        for (int i = 0; i < 2; i++)
            m_hangPositions[i].x += m_hangDistanceToThis * Mathf.Pow(-one, i);
        for (int i = 2; i < 4; i++)
            m_hangPositions[i].z += m_hangDistanceToThis * Mathf.Pow(-one, i);
    }

    /// <summary>밀기 로직 코루틴</summary>
    private IEnumerator PushLogic()
    {
        // 플레이어가 PushIdle 상태가 될 때 까지 기다림
        yield return new WaitUntil(() => PlayerManager.Instance.MainController.CurrentState3D.Equals(E_PlayerState3D.PushIdle));
        // 밀기 대기 로직 실행
        yield return StartCoroutine(PushIdleLogic());
    }

    /// <summary>밀기 대기 로직</summary>
    private IEnumerator PushIdleLogic()
    {
        Vector3 pushPosition = Vector3.zero;

        while(true)
        {
            if (!UIManager.Instance.IsOnUI)
            {
                E_PlayerState3D currentPlayerState = PlayerManager.Instance.MainController.CurrentState3D;

                // 플레이어의 상태가 PushMove 상태일 경우 반복문 종료 후 PushMoveLogic 실행
                if (currentPlayerState.Equals(E_PlayerState3D.PushMove))
                {
                    StartCoroutine(PushMoveLogic(pushPosition));
                    break;
                }
                // 플레이어의 상태가 Idle 상태일 경우 반복문 종료
                else if(currentPlayerState.Equals(E_PlayerState3D.Idle))
                {
                    break;
                }

                pushPosition = GetPushPosition();
                m_isCanPush = CheckCanPush(pushPosition);
                DrawFadeObject(pushPosition);
            }
            yield return null;
        }

        m_fadeObject.SetActive(false);
    }

    /// <summary>밀릴 위치를 반환. 밀릴 수 없는 위치일 경우 (1, 1, 1)을 반환</summary>
    private Vector3 GetPushPosition()
    {
        Vector3 pushPosition = Vector3.one;

        // 마우스 광선 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 레이어 마스크 설정
        int layerMask = GameLibrary.LayerMask_CanPushWay;

        RaycastHit hit;

        // 레이 발사
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // 플레이어가 바라보는 방향
            Vector3 playerDirection = PlayerManager.Instance.SubController3D.Forward;
            // 충돌된 마우스 위치의 피벗 구하기
            Vector3 hitPivot = hit.point.GetGamePivot();

            float one = 1f;

            // 플레이어의 방향이 x 양의 방향일 때
            if (Mathf.Round(playerDirection.x).Equals(one))
            {
                // 피벗의 x가 현재위치의 x보다 크고 피벗의 z는 현재위치의 z랑 같을 때
                if (hitPivot.x > transform.position.x && hitPivot.z.Equals(transform.position.z))
                {
                    pushPosition = hitPivot;
                    pushPosition.y = transform.position.y;
                }
            }
            // 플레이어의 방향이 x 음의 방향일 때
            else if (Mathf.Round(playerDirection.x).Equals(-one))
            {
                // 피벗의 x가 현재위치의 x보다 작고 피벗의 z는 현재위치의 z랑 같을 때
                if (hitPivot.x < transform.position.x && hitPivot.z.Equals(transform.position.z))
                {
                    pushPosition = hitPivot;
                    pushPosition.y = transform.position.y;
                }
            }
            // 플레이어의 방향이 z 양의 방향일 때
            else if (Mathf.Round(playerDirection.z).Equals(one))
            {
                // 피벗의 z가 현재위치의 z보다 크고 피벗의 x는 현재위치의 x랑 같을 때
                if (hitPivot.z > transform.position.z && hitPivot.x.Equals(transform.position.x))
                {
                    pushPosition = hitPivot;
                    pushPosition.y = transform.position.y;
                }
            }
            // 플레이어의 방향이 z 음의 방향일 때
            else if (Mathf.Round(playerDirection.z).Equals(-one))
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

    /// <summary>pushPosition으로 밀 수 있을경우 true를 반환</summary>
    protected virtual bool CheckCanPush(Vector3 pushPosition)
    {
        if (pushPosition.Equals(Vector3.one))
            return false;

        float temp = 0.1f;
        float boundXZ = m_modelCollider.bounds.extents.x;
        float boundY = m_modelCollider.bounds.extents.y;

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

        while (true)
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

    /// <summary>반투명 오브젝트를 그림</summary>
    private void DrawFadeObject(Vector3 pushPosition)
    {
        if (pushPosition.Equals(Vector3.one) && m_fadeObject.activeSelf)
        {
            m_fadeObject.SetActive(false);
            return;
        }
        else if (!pushPosition.Equals(Vector3.one) && !m_fadeObject.activeSelf)
            m_fadeObject.SetActive(true);

        m_fadeObject.transform.position = pushPosition;

        if (m_isCanPush && m_fadeObjectMaterial.color.Equals(Color.red))
            m_fadeObjectMaterial.color = Color.white;
        else if (!m_isCanPush && m_fadeObjectMaterial.color.Equals(Color.white))
            m_fadeObjectMaterial.color = Color.red;
    }

    /// <summary>밀기 로직</summary>
    private IEnumerator PushMoveLogic(Vector3 pushPosition)
    {
        m_isMove = true;

        // 위쪽에 있는 다른 아이템을 같이 이동시키기 위해 자식으로 포함
        int layerMask = GameLibrary.LayerMask_InteractionPickPut | 
                             GameLibrary.LayerMask_Enemy;

        RaycastHit[] anotherObjects = Physics.RaycastAll(transform.position, Vector3.up, Mathf.Infinity, layerMask);
        int anotherObjectCount = anotherObjects.Length;

        for (int i = 0; i < anotherObjectCount; i++)
            anotherObjects[i].transform.parent.parent = transform;

        while (true)
        {
            // 이동
            transform.position = Vector3.MoveTowards(transform.position, pushPosition, m_moveSpeed * Time.deltaTime);

            // 이동위치까지 이동했을경우 반복문 종료
            if (transform.position.Equals(pushPosition))
                break;

            yield return null;
        }

        // 포함되었던 다른 아이템을 제외시킴
        for (int i = 0; i < anotherObjectCount; i++)
            anotherObjects[i].transform.parent.parent = transform.parent;

        m_isMove = false;
    }
}