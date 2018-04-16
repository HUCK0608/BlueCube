using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Interaction_Put_Defalut : Interaction_Put
{
    public override void Put()
    {
        m_isPutEnd = false;
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
            Debug.Log("레이충돌 위치 : " + hit.point);
            Debug.Log("레이충돌 피벗 : " + hitPivot);
            Debug.Log("계산된 위치 : " + newPutPosition);
            if (GameLibrary.Raycast3D(newPutPosition, Vector3.down, out hit, Mathf.Infinity, layermask))
                putPosition = hit.point + Vector3.up;

            Debug.Log("최종 좌표 : " + putPosition);
        }

        return putPosition;
    }

    // 이동
    private IEnumerator Move()
    {
        while(true)
        {
            yield return null;
        }

        m_isPutEnd = false;
    }
}
