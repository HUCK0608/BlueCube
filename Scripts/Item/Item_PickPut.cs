using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_PickPut : MonoBehaviour
{
    private Transform m_fadeBox;
    private Material m_fadeBoxMaterial;

    // 아이템을 놓을 수 있는 최대거리
    [SerializeField]
    private float m_putMaxDistance;

    // 아이템을 드는 속도
    [SerializeField]
    private float m_pickUpMoveSpeed;

    // 어느 정도 거리까지 왔을 때 고정상태로 변경할 건지 체크하는 변수
    [SerializeField]
    private float m_changeFixedDistance;

    // 고정되어 있을 때 위 아래 왔다갔다할 범위
    [SerializeField]
    private float m_fixedUpDownRange;
    // 위 아래 왔다갔다할 스피드
    [SerializeField]
    private float m_fixedUpDownSpeed;

    private Vector3 m_currentPutPosition;
    /// <summary>아이템을 놓을 위치를 반환</summary>
    public Vector3 CurrentPutPosition { get { return m_currentPutPosition; } }

    private bool m_isPick;
    /// <summary>아이템 들어올리기를 완료했을경우 true를 반환</summary>
    public bool IsPick { get { return m_isPick; } }

    private bool m_isPut;
    /// <summary>아이템 놓기를 완료했을경우 true를 반환</summary>
    public bool IsPut { get { return m_isPut; } }

    private bool m_isCanPut;
    /// <summary>아이템을 놓을 수 있으면 true를 반환</summary>
    public bool IsCanPut { get { return m_isCanPut; } }

    private void Awake()
    {
        m_fadeBox = GameObject.Find("GameLibrary").transform.Find("Interaction_PickPutBox_Fade");
        m_fadeBoxMaterial = m_fadeBox.GetComponent<MeshRenderer>().material;
    }

    /// <summary>아이템 들기</summary>
    public void PickItem()
    {
        StartCoroutine(PickInit());
    }

    /// <summary>아이템 놓기</summary>
    public void PutItem()
    {
        m_isPick = false;
        StartCoroutine(PickEnd());
    }

    // 상자가 고정될 지점으로 올라가는 코루틴
    private IEnumerator PickInit()
    {
        m_isPick = false;

        // 아이템이 고정될 위치
        Vector3 pickItemPosition3D = PlayerManager.Instance.Hand.PickItemPosition3D.position;

        while(true)
        {
            // 아이템 들기
            transform.position = Vector3.Slerp(transform.position, pickItemPosition3D, m_pickUpMoveSpeed * Time.deltaTime);

            // 고정될 위치에 근접했으면 반복문 종료
            if (Vector3.Distance(transform.position, pickItemPosition3D) <= m_changeFixedDistance)
                break;

            yield return null;
        }

        m_isPick = true;
        StartCoroutine(FixedItem());
    }

    // 상자를 아이템 위치에 고정하며 위아래 왔다갔다 함
    private IEnumerator FixedItem()
    {
        // 반투명 상자 활성화
        m_fadeBox.gameObject.SetActive(true);

        Transform pickItemPosition = PlayerManager.Instance.Hand.PickItemPosition3D;

        // 상자가 올라가는 상태일경우 true를 반환
        bool isUp = true;
        // 처음 기존 상자 위치
        Vector3 startDefaultPosition = transform.position;
        // 시작 아이템 고정 위치
        Vector3 startDefaultFixedPosition = pickItemPosition.position;
        // 기존 위치에 더해줄 y위치
        float addPositionY = 0f;

        while(IsPick)
        {
            // 기존 위치에 더해줄 위치 계산
            if (isUp)
                addPositionY += m_fixedUpDownSpeed * Time.deltaTime;
            else
                addPositionY -= m_fixedUpDownSpeed * Time.deltaTime;

            // 고정될 위치 계산
            Vector3 fixedPosition = startDefaultPosition + (pickItemPosition.position - startDefaultFixedPosition);
            fixedPosition.y += addPositionY;
            transform.position = fixedPosition;

            // 상자가 올라갈지 내려갈지 결정
            if (addPositionY >= m_fixedUpDownRange)
                isUp = false;
            else if (addPositionY <= -m_fixedUpDownRange)
                isUp = true;

            // 떨어질 위치 그려주기
            DrawFadeBox();

            yield return null;
        }
    }

    // 반투명 상자로 떨어질 위치를 그려줌
    private void DrawFadeBox()
    {
        // 떨어질 위치 받아오기
        Vector3 putPosition = CalcPutPositon();

        // 플레이어 위치를 가져오고 높이는 현재 계산된 위치의 높이랑 같게 함
        Vector3 playerPosition = PlayerManager.Instance.Player3D_Object.transform.position;
        Vector3 playerPositionXZ = playerPosition;
        playerPositionXZ.y = putPosition.y;

        // 새로 입력될 색
        Color newColor = Color.white;
        // 플레이어까지의 거리
        float distanceToPlayer = 0f;

        // 아이템을 놓을 수 있는 위치일 경우
        if (!putPosition.Equals(Vector3.one))
        {
            // 계산된 놓을 위치에서 플레이어까지 거리 측정하기
            distanceToPlayer = Vector3.Distance(putPosition, playerPositionXZ);
        }
        // 놓을 수 없는 위치일 경우
        else
        {
            // 떨어질 위치에 기존 위치를 입력
            putPosition = m_fadeBox.position;

            // 기존 위치에서 플레이어까지 거리 측정하기
            distanceToPlayer = Vector3.Distance(m_fadeBox.position, playerPositionXZ);
        }

        // 해당 거리가 최대 놓을 수 있는 거리를 벗어나지 않고 제한높이를 벗어나지 않을경우
        if (distanceToPlayer <= m_putMaxDistance && putPosition.y < transform.position.y && putPosition.y > playerPosition.y)
        {
            newColor = Color.white;
            newColor.a = m_fadeBoxMaterial.color.a;
            m_isCanPut = true;
            m_currentPutPosition = putPosition;
        }
        // 벗어 났을 경우
        else
        {
            newColor = Color.red;
            newColor.a = m_fadeBoxMaterial.color.a;
            m_isCanPut = false;
        }

        // 반투명 상자 색변경
        m_fadeBoxMaterial.color = newColor;

        // 반투명 상자 이동
        m_fadeBox.position = putPosition;
    }

    // 상자 내려놓기
    private IEnumerator PickEnd()
    {
        // 반투명 상자 비활성화
        m_fadeBox.gameObject.SetActive(false);

        m_isPut = false;

        // 떨어질 최종 위치
        Vector3 putPosition = m_currentPutPosition;
        // 이동할 x, z 위치
        Vector3 putPositionXZ = putPosition;
        putPositionXZ.y = transform.position.y;

        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, putPositionXZ, 10f * Time.deltaTime);

            if (transform.position.Equals(putPositionXZ))
                break;

            yield return null;
        }

        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, putPosition, 10f * Time.deltaTime);

            if (transform.position.Equals(putPosition))
                break;

            yield return null;
        }

        m_isPut = true;
    }

    /// <summary>상자가 떨어질 위치를 계산하여 반환. 떨어질 수 있는 위치가 없을경우 (1, 1, 1)를 반환<summary>
    private Vector3 CalcPutPositon()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 putPosition = Vector3.one;

        // 무시할 레이어마스크
        int layermask = (-1) - (GameLibrary.LayerMask_Bullet |
                                     GameLibrary.LayerMask_Player |
                                     GameLibrary.LayerMask_IgnoreRaycast |
                                     GameLibrary.LayerMask_BackgroundTrigger |
                                     gameObject.layer.Shift());

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            // 오브젝트의 높이가 다 다를 수 있으므로 피벗을 찾기 위해 계산
            float one = 1f;
            float two = 2f;

            float putPositionY = hit.point.y;
            float floorY = Mathf.Floor(putPositionY);

            if ((floorY % two).Equals(one))
                putPositionY = floorY + one;
            else
                putPositionY = floorY;

            Vector3 hitPivot = hit.transform.position;
            hitPivot.y = putPositionY;

            // 충돌 오브젝트의 피벗을 구해서 충돌 로컬좌표를 구함
            Vector3 localHitPosition = hit.point - hitPivot;

            // 절대값으로 변경
            float absX = Mathf.Abs(localHitPosition.x);
            float absY = Mathf.Abs(localHitPosition.y);
            float absZ = Mathf.Abs(localHitPosition.z);

            // 절대값이 제일 높은 값을 기준으로 방향을 구함
            if(absX > absY)
            {
                if(absX > absZ)
                {
                    localHitPosition.y = 0f;
                    localHitPosition.z = 0f;
                }
                else
                {
                    localHitPosition.x = 0f;
                    localHitPosition.y = 0f;
                }
            }
            else
            {
                if(absY > absZ)
                {
                    localHitPosition.x = 0f;
                    localHitPosition.z = 0f;
                }
                else
                {
                    localHitPosition.x = 0f;
                    localHitPosition.y = 0f;
                }
            }

            // 놓을위치를 계산
            putPosition = hitPivot + localHitPosition.normalized * two;
            putPosition.y = putPositionY;

            // 놓을위치에서 아래쪽에 레이를 쏨
            if (GameLibrary.Raycast3D(putPosition, Vector3.down, out hit, Mathf.Infinity, layermask))
            {
                // 무언가 있다면 그 오브젝트 위에 최종 위치를 구함
                putPosition = hit.point + Vector3.up;
            }
            else
            {
                // 무언가 없다면 놓을 수 없다고 알림
                putPosition = Vector3.one;
            }
        }

        // 반환
        return putPosition;
    }

}
