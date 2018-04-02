using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_PickPut : MonoBehaviour
{
    private Transform m_fadeBox;

    private bool m_isPick;
    /// <summary>아이템 들어올리기를 완료했을경우 true를 반환</summary>
    public bool IsPick { get { return m_isPick; } }

    private bool m_isPut;
    /// <summary>아이템 놓기를 완료했을경우 true를 반환</summary>
    public bool IsPut { get { return m_isPut; } }

    // 아이템을 드는 속도
    [SerializeField]
    private float m_pickUpSlerpSpeed;

    // 어느 정도 거리까지 왔을 때 고정상태로 변경할 건지 체크하는 변수
    [SerializeField]
    private float m_changeFixedDistance;

    // 고정되어 있을 때 위 아래 왔다갔다할 범위
    [SerializeField]
    private float m_fixedUpDownRange;
    // 위 아래 왔다갔다할 스피드
    [SerializeField]
    private float m_fixedUpDownSpeed;

    private void Awake()
    {
        m_fadeBox = GameObject.Find("GameLibrary").transform.Find("Item_Box_PickPut_Fade");
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
            transform.position = Vector3.Slerp(transform.position, pickItemPosition3D, m_pickUpSlerpSpeed * Time.deltaTime);

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

            DrawFadeBox();

            yield return null;
        }
    }

    // 반투명 상자로 떨어질 위치를 그려줌
    private void DrawFadeBox()
    {
        m_fadeBox.position = CalcPutPositon();
    }

    // 상자 내려놓기
    private IEnumerator PickEnd()
    {
        // 반투명 상자 비활성화
        m_fadeBox.gameObject.SetActive(false);

        m_isPut = false;

        // 떨어질 최종 위치
        Vector3 putPosition = CalcPutPositon();
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

    // 상자가 떨어질 위치를 계산해서 반환
    private Vector3 CalcPutPositon()
    {
        // 최종 계산 위치
        Vector3 putPosition = transform.position;

        // 계산에 필요한 변수
        float zero = 0f;
        float one = 1f;
        float two = 2f;

        // x, z 좌표 내림
        float calcX = Mathf.Floor(putPosition.x);
        float calcZ = Mathf.Floor(putPosition.z);

        // x 계산
        // 내림한 값에 나누기2를 한 나머지가 0이아니면 1을 더해줌
        if(!(calcX % two).Equals(zero))
            calcX += one;

        // z 계산
        // 내림한 값에 나누기2를 한 나머지가 0이아니면 1을 더해줌
        if (!(calcZ % two).Equals(zero))
            calcZ += one;

        // y 계산
        // y는 밑으로 레이를 쏴서 부딪히는 곳에서 1f를 더함
        float calcY = zero;
        RaycastHit hit;

        if (GameLibrary.Raycast3D(transform.position, Vector3.down, out hit, Mathf.Infinity, GameLibrary.LayerMask_Ignore_RBP))
            calcY = hit.point.y + one;

        // 계산된 값을 적용
        putPosition.x = calcX;
        putPosition.z = calcZ;
        putPosition.y = calcY;

        // 반환
        return putPosition;
    }
}
