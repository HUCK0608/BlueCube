using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Push : MonoBehaviour
{
    private Collider m_collider;

    // 현재위치에서 더해줄 위치 및 개수
    private Vector3[] m_hangAddPosition;
    private int m_hangCount;

    private float m_moveDistance;
    /// <summary>한번 밀 때 이동거리</summary>
    public float MoveDistance { get { return m_moveDistance; } }

    private bool m_isMove;
    /// <summary>이동중일 경우 true를 반환</summary>
    public bool IsMove { get { return m_isMove; } }

    private void Awake()
    {
        m_collider = GetComponent<Collider>();

        m_moveDistance = 2f;

        InitHangAddPositions();
    }

    // 고정될 위치 초기화
    private void InitHangAddPositions()
    {
        Vector3 zero = Vector3.zero;
        // 초기화
        m_hangAddPosition = new Vector3[4] { zero, zero, zero, zero };

        m_hangCount = 4;

        float boundXZ = 1.7f;

        m_hangAddPosition[0].x = boundXZ;
        m_hangAddPosition[1].x = -boundXZ;
        m_hangAddPosition[2].z = boundXZ;
        m_hangAddPosition[3].z = -boundXZ;
    }

    /// <summary>target에서 제일 가까운 고정 위치를 반환</summary>
    public Vector3 GetNearHangPosition(Vector3 target)
    {
        // 첫 번째 고정위치가 제일 가까운 위치라고 설정
        Vector3 nearPosition = transform.position + m_hangAddPosition[0];
        float nearDistance = Vector3.Distance(target, nearPosition);

        // 나머지 고정 위치만큼 돌면서 제일 가까운 거리를 계산
        for(int i = 1; i < m_hangCount; i++)
        {
            Vector3 hangPosition = transform.position + m_hangAddPosition[i];
            float distance = Vector3.Distance(target, hangPosition);

            // 새로 구해진 거리가 기존의 거리보다 가까운 경우 가까운 위치값을 변경
            if(distance < nearDistance)
            {
                nearPosition = hangPosition;
                nearDistance = distance;
            }
        }

        return nearPosition;
    }

    /// <summary>direction방향으로 이동할 수 있다면 true를 반환</summary>
    public bool IsCanMove(Vector3 direction)
    {

        float temp = 0.1f;
        float boundY = m_collider.bounds.extents.y;
        
        // 최소 최대 체크 위치 계산
        Vector3 minCheckPosition = transform.position;
        Vector3 maxCheckPosition = transform.position;

        minCheckPosition.y -= (boundY - temp);
        maxCheckPosition.y += (boundY - temp);

        // 최소 최대 위치 체크
        if (GameLibrary.Raycast3D(minCheckPosition, direction, m_moveDistance, GameLibrary.LayerMask_Ignore_RBP))
            return false;
        else if (GameLibrary.Raycast3D(maxCheckPosition, direction, m_moveDistance, GameLibrary.LayerMask_Ignore_RBP))
            return false;

        // 시작 체크 높이
        float startCheckPositionY = transform.position.y - boundY + 1f;

        Vector3 checkPosition = transform.position;
        checkPosition.y = startCheckPositionY;

        bool canMove = true;

        while(true)
        {
            // 무언가 충돌하면 충돌했다고 알리고 반복문 종료
            if(GameLibrary.Raycast3D(checkPosition, direction, m_moveDistance, GameLibrary.LayerMask_Ignore_RBP))
            {
                canMove = false;
                break;
            }

            checkPosition.y += m_moveDistance;

            // 만약 체크높이가 현재 오브젝트의 최대 높이를 넘어갈 경우 반복문 종료
            if (checkPosition.y > transform.position.y + boundY)
                break;
        }

        return canMove;
    }

    /// <summary>direction 방향으로 아이템을 민다</summary>
    public void PushItem(Vector3 direction)
    {
        StartCoroutine(Move(direction));
    }

    // 상자 이동
    private IEnumerator Move(Vector3 direction)
    {
        m_isMove = true;

        // 위쪽에 다른 Item이 있을경우 자식에 포함
        RaycastHit[] anotherItems = AddAnotherItem();

        // 이동 위치 계산
        Vector3 movePosition = transform.position + (direction * m_moveDistance);

        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePosition, PlayerManager.Instance.Stat.MoveSpeed_Push * Time.deltaTime);

            if (transform.position.Equals(movePosition))
                break;

            yield return null;
        }

        // 포함되었던 다른 Item을 제외시킴
        RemoveAnotherItem(anotherItems);

        m_isMove = false;
    }

    // 위쪽에 다른 아이템이 있는지 체크해서 있을 경우 자식으로 포함시킴
    private RaycastHit[] AddAnotherItem()
    {
        int layerMask = (1 << 9);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.up, Mathf.Infinity, layerMask);
        int hitCount = hits.Length;

        for (int i = 0; i < hitCount; i++)
            hits[i].transform.parent = transform;

        return hits;
    }

    // 포함되어 있는 아이템을 제외시킴
    private void RemoveAnotherItem(RaycastHit[] hits)
    {
        int hitCount = hits.Length;

        for (int i = 0; i < hitCount; i++)
            hits[i].transform.parent = transform.parent;
    }
}
