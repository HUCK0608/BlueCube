using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Interaction_Put_Defalut : Interaction_Put
{
    [SerializeField]
    private float m_moveSpeed;

    public override void Put()
    {
        StartCoroutine(Move());
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
            // 히트 피벗 구하기
            Vector3 hitPivot = hit.point.GetGamePivot();
            Vector3 newPutPosition = hitPivot + GameLibrary.GetDirectionAtPivot(hitPivot, hit.point) * 2f;

            if (GameLibrary.Raycast3D(newPutPosition, Vector3.down, out hit, Mathf.Infinity, layermask))
                putPosition = hit.point + Vector3.up;
        }

        return putPosition;
    }

    // 이동
    private IEnumerator Move()
    {
        m_isPutEnd = false;

        Vector3 putPosition = GetPutPosition();
        Vector3 putPositionXZ = putPosition;
        putPositionXZ.y = m_model.position.y;

        // x, z 이동
        while(true)
        {
            m_model.position = Vector3.MoveTowards(m_model.position, putPositionXZ, m_moveSpeed * Time.deltaTime);

            if (m_model.position.Equals(putPositionXZ))
                break;

            yield return null;
        }

        // 최종 위치로의 이동
        while(true)
        {
            m_model.position = Vector3.MoveTowards(m_model.position, putPosition, m_moveSpeed * Time.deltaTime);

            if (m_model.position.Equals(putPosition))
                break;

            yield return null;
        }

        m_isPutEnd = true;
    }
}
