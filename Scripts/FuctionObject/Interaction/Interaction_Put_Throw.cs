using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Interaction_Put_Throw : Interaction_Put
{
    private Rigidbody m_rigidbody;

    // 거리에 비례한 시간 퍼센티지
    [SerializeField]
    private float m_timeToDistancePer;

    // 중력
    [SerializeField]
    private float m_gravity;

    // 최종 시간
    private float m_fianlTime;

    // 현재 속도
    private Vector3 m_velocity;

    protected override void Awake()
    {
        base.Awake();

        m_rigidbody = GetComponentInChildren<Rigidbody>();
    }

    public override void Put()
    {
        m_isPutEnd = false;
        StartCoroutine(Move());
        m_isPutEnd = true;
    }

    /// <summary>정규화된 놓을 위치를 반환한다</summary>
    protected override Vector3 GetPutPosition()
    {
        Vector3 putPosition = Vector3.one;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 무시할 레이어마스크
        int layermask = (-1) - (GameLibrary.LayerMask_Bullet |
                                     GameLibrary.LayerMask_Player |
                                     GameLibrary.LayerMask_IgnoreRaycast |
                                     GameLibrary.LayerMask_BackgroundTrigger);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 newPutPosition = hit.point.GameNormalized();

            if (GameLibrary.Raycast3D(newPutPosition, Vector3.down, out hit, Mathf.Infinity, layermask))
                putPosition = hit.point + Vector3.up;
        }

        return putPosition;
    }

    // 이동
    private IEnumerator Move()
    {
        CalcStartVelocity(GetPutPosition());

        while(true)
        {
            m_rigidbody.velocity = m_velocity;
            m_velocity.y = m_velocity.y + m_gravity * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
    }

    // 떨어질 지점을 이용하여 초기 속도 계산
    private void CalcStartVelocity(Vector3 putPosition)
    {
        float zero = 0f;

        Vector3 thisPositionXZ = transform.position;
        thisPositionXZ.y = zero;
        Vector3 putPositionXZ = putPosition;
        putPositionXZ.y = zero;

        float distanceXZ = Vector3.Distance(thisPositionXZ, putPositionXZ);
        m_fianlTime = Mathf.Abs(distanceXZ) * m_timeToDistancePer;

        float distanceX = putPosition.x - transform.position.x;
        float distanceY = putPosition.y - transform.position.y;
        float distanceZ = putPosition.z - transform.position.z;

        float componentX = distanceX / m_fianlTime;
        float componentZ = distanceZ / m_fianlTime;
        float componentY = (distanceY / m_fianlTime) - (0.5f * m_gravity * m_fianlTime);

        m_velocity.x = componentX;
        m_velocity.y = componentY;
        m_velocity.z = componentZ;
    }
}
