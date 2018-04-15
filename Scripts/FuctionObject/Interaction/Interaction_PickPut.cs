using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Interaction_PickPut : MonoBehaviour
{ 
    // 놓기 스크립트
    private Interaction_Put m_put;

    // 오브젝트를 들 때 고정상태로 바뀔 최소 거리
    [SerializeField]
    private float m_changeFixedMinDistance;

    // 고정되었을 때 위 아래를 왔다갔다할 범위
    [SerializeField]
    private float m_fixedUpDownRange;
    [SerializeField]
    private float m_fixedUpDownSpeed;

    private bool m_isPick;
    /// <summary>오브젝트를 들고 고정되었을 경우 true를 반환</summary>
    public bool IsPick { get { return m_isPick; } }

    /// <summary>놓을 위치를 반환</summary>
    public Vector3 GetPutPosition() { return m_put.PutPosition; }

    /// <summary>오브젝트의 던지기 방식을 반환</summary>
    public E_PutType PutType { get { return m_put.PutType; } }

    /// <summary>오브젝트를 놓을 수 있을 경우 true를 반환</summary>
    public bool IsCanPut { get { return m_put.IsCanPut; } }

    /// <summary>오브젝트 놓기 과정이 끝났을경우 true를 반환</summary>
    public bool IsPutEnd { get { return m_put.IsPutEnd; } }

    private void Awake()
    {
        m_put = GetComponent<Interaction_Put>();
    }

    /// <summary>오브젝트를 든다</summary>
    public void PickObject()
    {
        StartCoroutine(MovePlayerPickPosition());
    }

    /// <summary>오브젝트를 놓는다</summary>
    public void PutObject()
    {
        m_isPick = false;

        m_put.Put();
    }

    /// <summary>오브젝트를 플레이어의 PickPosition으로 이동</summary>
    private IEnumerator MovePlayerPickPosition()
    {
        // 고정될 위치
        Vector3 pickPosition = PlayerManager.Instance.Hand.PickObjectPoint.position;
        float objectPickSpeed = PlayerManager.Instance.Stat.ObjectPickSpeed;

        while(true)
        {
            // 들기
            transform.position = Vector3.Slerp(transform.position, pickPosition, objectPickSpeed * Time.deltaTime);

            // 고정될 위치에 근접했을경우 반복문 종료
            if (Vector3.Distance(transform.position, pickPosition) <= m_changeFixedMinDistance)
                break;

            yield return null;
        }

        m_isPick = true;

        // put스크립트 로직 실행
        m_put.StartPutRogic();

        // 고정 코루틴 실행
        StartCoroutine(FixedObject());
    }

    /// <summary>오브젝트를 플레이어의 PickPosition에 고정시키며 둥둥 떠다니게 함</summary>
    private IEnumerator FixedObject()
    {
        Transform pickPoint = PlayerManager.Instance.Hand.PickObjectPoint;

        // 오브젝트가 올라가는 상태일경우 true
        bool isUp = true;

        // 현재 위치와 실제 고정될 위치와의 오차
        Vector3 errorPosition = pickPoint.position - transform.position;
        // 위 아래로 왔다갔다 하기 위한 값
        float upDownValue = 0f;

        // 이 부분에서 위 아래로 왔다갔다 함
        while(m_isPick)
        {
            if (isUp)
                upDownValue += m_fixedUpDownSpeed * Time.deltaTime;
            else
                upDownValue -= m_fixedUpDownSpeed * Time.deltaTime;

            Vector3 fixedPosition = pickPoint.position - errorPosition;
            fixedPosition.y += upDownValue;
            transform.position = fixedPosition;

            if (upDownValue >= m_fixedUpDownRange)
                isUp = false;
            else if (upDownValue <= -m_fixedUpDownRange)
                isUp = true;

            yield return null;
        }
    }
}
