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
            Vector3 newPutPosition = hit.point + hit.normal;

            if (GameLibrary.Raycast3D(newPutPosition, Vector3.down, out hit, Mathf.Infinity, layermask))
                putPosition = hit.point + Vector3.up;
        }

        return putPosition;
    }

    // 이동
    private IEnumerator Move()
    {
        CalcStartVelocity(m_putPosition);

        m_rigidbody.isKinematic = false;
        m_rigidbody.AddForce(m_velocity, ForceMode.Impulse);

        float stopVelocityDistanceY = 1.2f;
        int layerMask = (-1) - (GameLibrary.LayerMask_BackgroundTrigger |
                                     GameLibrary.LayerMask_Bullet |
                                     GameLibrary.LayerMask_IgnoreRaycast);

        while(true)
        {
            m_velocity = m_rigidbody.velocity;
            m_velocity.y = m_velocity.y + m_gravity * Time.fixedDeltaTime;
            m_rigidbody.velocity = m_velocity;

            // 밑에 무언가 있다면 이동 코루틴 종료
            if (GameLibrary.Raycast3D(m_model.position, Vector3.down, stopVelocityDistanceY, layerMask))
                break;

            yield return new WaitForFixedUpdate();
        }

        m_rigidbody.isKinematic = true;
    }

    // 떨어질 지점을 이용하여 초기 속도 계산
    private void CalcStartVelocity(Vector3 putPosition)
    {
        float zero = 0f;

        Vector3 thisPositionXZ = m_model.position;
        thisPositionXZ.y = zero;
        Vector3 putPositionXZ = putPosition;
        putPositionXZ.y = zero;

        float distanceXZ = Vector3.Distance(thisPositionXZ, putPositionXZ);
        m_fianlTime = Mathf.Abs(distanceXZ) * m_timeToDistancePer;

        float distanceX = putPosition.x - m_model.position.x;
        float distanceY = putPosition.y - m_model.position.y;
        float distanceZ = putPosition.z - m_model.position.z;

        float componentX = distanceX / m_fianlTime;
        float componentZ = distanceZ / m_fianlTime;
        float componentY = (distanceY / m_fianlTime) - (0.5f * m_gravity * m_fianlTime);

        m_velocity.x = componentX;
        m_velocity.y = componentY;
        m_velocity.z = componentZ;
    }
}
