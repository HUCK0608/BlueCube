using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switch_On_MoveDestination : Switch_On
{
    [Space(20f)]
    [Header("[Switch On MoveDestination]")]
    [Space(-5f)]
    [Header("- Can Change")]

    /// <summary>도착지</summary>
    [SerializeField]
    private Vector3 m_destination;

    /// <summary>이동 속도</summary>
    [SerializeField]
    private float m_moveSpeed;

    [Header("- Don't Touch")]

    /// <summary>이동 피벗</summary>
    [SerializeField]
    private E_MovePivot m_movePivot;

    /// <summary>이동할 오브젝트</summary>
    [SerializeField]
    private Transform m_moveObject;

    /// <summary>스위치가 켜지는 로직(MoveDestination)</summary>
    protected override IEnumerator SwitchOnLogic()
    {
        while(true)
        {
            // 시간이 멈춰있지 않은 경우 실행
            if(!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                // 이동 피벗이 로컬형태일 때
                if(m_movePivot.Equals(E_MovePivot.Local))
                {
                    m_moveObject.localPosition = Vector3.MoveTowards(m_moveObject.localPosition, m_destination, m_moveSpeed * Time.deltaTime);

                    if (m_moveObject.localPosition.Equals(m_destination))
                        break;
                }
                // 이동 피벗이 월드형태일 때
                else if(m_movePivot.Equals(E_MovePivot.World))
                {
                    m_moveObject.position = Vector3.MoveTowards(m_moveObject.position, m_destination, m_moveSpeed * Time.deltaTime);

                    if (m_moveObject.position.Equals(m_destination))
                        break;
                }
            }
            yield return null;
        }

        m_isOnLogic = false;
        m_isOn = true;

        ChangeOnMesh();
    }

    // 버튼 이동
    //private IEnumerator MoveButton()
    //{
    //    m_isButtonMove = true;

    //    while (true)
    //    {
    //        if (!GameLibrary.Bool_IsGameStop(m_worldObject))
    //        {
    //            m_button.localPosition = Vector3.MoveTowards(m_button.localPosition, m_buttonOnPosition, m_buttonMoveSpeed * Time.deltaTime);

    //            if (m_button.localPosition.Equals(m_buttonOnPosition))
    //                break;
    //        }

    //        yield return null;
    //    }

    //    m_isButtonMove = false;
    //    m_isOn = true;
    //    m_buttonMeshFilter.mesh = m_buttonOnMesh;
    //}
}
