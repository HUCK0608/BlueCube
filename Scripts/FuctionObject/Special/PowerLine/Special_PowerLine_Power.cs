using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Special_PowerLine_Power : MonoBehaviour
{
    private static string m_powerLineTag = "Special_PowerLine_Line";

    private static float m_multiplyDirection2D = 1.05f;

    private static float m_checkDistance3D = 1.05f;
    private static float m_checkDistance2D = 0.05f;

    /// <summary>월드 오브젝트</summary>
    private WorldObject m_worldObject;

    /// <summary>체크했었던 라인 모음</summary>
    private Dictionary<Transform, Special_PowerLine_Line> m_checkedLines;

    /// <summary>전송 방향 모음</summary>
    private Vector3[] m_transmitDirections;
    /// <summary>전송 방향 개수</summary>
    private int m_transmitDirectionsCount;

    private void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();

        m_checkedLines = new Dictionary<Transform, Special_PowerLine_Line>();

        InitTransmitDirection();
    }

    /// <summary>전송 방향 초기화</summary>
    private void InitTransmitDirection()
    {
        m_transmitDirections = new Vector3[4] { Vector3.up, Vector3.right, Vector3.left, Vector3.down };
        m_transmitDirectionsCount = 4;
    }

    private void Start()
    {
        // 파워 전송 로직 시작
        StartCoroutine(TransmitPowerLogic());
    }

    /// <summary>파워 전송 로직</summary>
    private IEnumerator TransmitPowerLogic()
    {
        while(true)
        {
            if(!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                E_ViewType currentView = PlayerManager.Instance.CurrentView;

                if (currentView.Equals(E_ViewType.View3D))
                    TransmitPower3D();
                else
                    TransmitPower2D();
            }

            yield return null;
        }
    }

    /// <summary>3D 상태에서 파워 전송</summary>
    private void TransmitPower3D()
    {
        for(int i = 0; i < m_transmitDirectionsCount; i++)
        {
            Vector3 origin = transform.position;
            Vector3 direction = m_transmitDirections[i];
            RaycastHit hit;

            if(GameLibrary.Raycast3D(origin, direction, out hit, m_checkDistance3D))
            {
                // 라인일 경우에만
                if(hit.transform.tag.Equals(m_powerLineTag))
                {
                    // 체크했던 라인 리스트에 없을 경우 리스트에 추가
                    if (!m_checkedLines.ContainsKey(hit.transform))
                        m_checkedLines.Add(hit.transform, hit.transform.GetComponentInParent<Special_PowerLine_Line>());

                    Special_PowerLine_Line line = m_checkedLines[hit.transform];

                    // 해당 라인에 대한 라인을 가지고 있으면 파워 전송
                    if(line.LineDirection.IsHaveLine(-direction))
                        m_checkedLines[hit.transform].TransmitPower(-m_transmitDirections[i]);
                }
            }
        }
    }

    /// <summary>2D 상태에서 파워 전송</summary>
    private void TransmitPower2D()
    {
        for (int i = 0; i < m_transmitDirectionsCount; i++)
        {
            Vector2 origin = transform.position + m_transmitDirections[i] * m_multiplyDirection2D;
            Vector2 direction = m_transmitDirections[i];
            RaycastHit2D hit;
            
            if (GameLibrary.Raycast2D(origin, direction, out hit, m_checkDistance2D))
            {
                // 라인일 경우에만
                if (hit.transform.tag.Equals(m_powerLineTag))
                {
                    // 체크했던 라인 리스트에 없을 경우 리스트에 추가
                    if (!m_checkedLines.ContainsKey(hit.transform))
                        m_checkedLines.Add(hit.transform, hit.transform.GetComponentInParent<Special_PowerLine_Line>());

                    Special_PowerLine_Line line = m_checkedLines[hit.transform];

                    // 해당 라인에 대한 라인을 가지고 있으면 파워 전송
                    if (line.LineDirection.IsHaveLine(-direction))
                        m_checkedLines[hit.transform].TransmitPower(-m_transmitDirections[i]);
                }
            }
        }
    }
}
