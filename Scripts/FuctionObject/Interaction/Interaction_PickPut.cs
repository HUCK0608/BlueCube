using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_PutType { Default, Throw }
public sealed class Interaction_PickPut : MonoBehaviour
{
    private Collider m_collider;

    [SerializeField]
    private E_PutType m_putType;

    // 아이템을 들 때 고정상태로 바뀔 최소 거리
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

    private bool m_isPutEnd;
    /// <summary>놓기 과정이 모두 끝났을 경우 true를 반환</summary>
    public bool IsPutEnd { get { return m_isPutEnd; } }

    private void Awake()
    {
        m_collider = GetComponentInChildren<Collider>();
    }

    /// <summary>오브젝트를 든다</summary>
    public void PickObject()
    {
        StartCoroutine(MovePlayerPickPosition());
    }

    /// <summary>아이템을 놓는다</summary>
    public void PutObject()
    {
        m_isPick = false;

        // 던지기 타입에 따라 오브젝트를 놓는다
        if(m_putType.Equals(E_PutType.Throw))
            PutThrow();
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

            Vector3 fixedPosition = pickPoint.position + errorPosition;
            fixedPosition.y += upDownValue;
            transform.position = fixedPosition;

            if (upDownValue >= m_fixedUpDownRange)
                isUp = false;
            else if (upDownValue <= -m_fixedUpDownRange)
                isUp = true;

            yield return null;
        }
    }

    /// <summary>포물선 방식으로 던진다</summary>
    private void PutThrow()
    {
        m_isPutEnd = false;
    }

    /// <summary>정규화 되지 않은 놓을 위치를 반환한다. 놓을 수 없는 위치일 경우 (1, 1, 1)을 반환한다.</summary>
    public Vector3 GetPutPosition()
    {
        Vector3 putPosition = Vector3.one;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

        }
    }

    /// <summary>정규화된 놓을 위치를 반환한다</summary>
    public void GetNormalizedPutPosition()
    {
        
    }
}
