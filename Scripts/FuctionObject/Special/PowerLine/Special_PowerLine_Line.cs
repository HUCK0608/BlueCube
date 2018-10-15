using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Special_PowerLine_Line : MonoBehaviour
{
    private const string m_PowerLineTag = "Special_PowerLine_Line";
    private const string m_PowerTag = "Special_PowerLine_Power";
    private const string m_brightnessPath = "_Brightness";
    private const float m_onBrightness = 1.0f;
    private const float m_offBrightness = 0.0f;

    private const float m_multiplyDirection2D = 1.05f;
    private const float m_checkDistance = 1.05f;

    [Header("Don't Touch")]
    [SerializeField]
    private MeshRenderer m_lineMeshRenderer;

    /// <summary>월드 오브젝트</summary>
    private WorldObject m_worldObject;

    private Special_PowerLine_LineDirection m_lineDirection;
    /// <summary>라인이 연결되어있는 방향</summary>
    public Special_PowerLine_LineDirection LineDirection { get { return m_lineDirection; } }

    /// <summary>체크했었던 라인 모음</summary>
    private Dictionary<Transform, Special_PowerLine_Line> m_checkedLines;

    /// <summary>연결된 파워 라인</summary>
    private Special_PowerLine_Line m_connectedPowerLine;

    /// <summary>파워가 연결된 방향</summary>
    private Vector3 m_connectedPowerDirections;

    private bool m_isConnectedPower;
    /// <summary>전원과 연결되어 있을 경우 true를 반환</summary>
    public bool IsConnectedPower { get { return m_isConnectedPower; } }

    private void Awake()
    {
        m_worldObject = GetComponent<WorldObject>();
        m_lineDirection = GetComponent<Special_PowerLine_LineDirection>();
        m_checkedLines = new Dictionary<Transform, Special_PowerLine_Line>();
    }

    /// <summary>파워 전송(연결된 방향)</summary>
    public void TransmitPower(Vector3 connectedDirection)
    {
        m_connectedPowerDirections = connectedDirection;

        // 로직 시작
        if(!m_isConnectedPower)
            StartCoroutine(TransmitPowerLogic());
    }

    public void TransmitPower(Special_PowerLine_Line connectedPowerLine, Vector3 connectedDirection)
    {
        m_connectedPowerLine = connectedPowerLine;
        m_connectedPowerDirections = connectedDirection;

        // 로직 시작
        if (!m_isConnectedPower)
            StartCoroutine(TransmitPowerLogic());
    }

    /// <summary>파워 전송 로직</summary>
    private IEnumerator TransmitPowerLogic()
    {
        m_isConnectedPower = true;
        m_lineMeshRenderer.material.SetFloat(m_brightnessPath, m_onBrightness);

        // 렌더러가 활성화 되어있는 경우에만 실행
        while(m_worldObject.IsOnRenderer)
        {
            // 화면전환중이 아닐경우에만 실행
            if (!PlayerManager.Instance.IsViewChange)
            {
                E_ViewType currentView = PlayerManager.Instance.CurrentView;

                // 3D 상태
                if (currentView.Equals(E_ViewType.View3D))
                {
                    CheckAnotherLine3D();

                    // 연결되어 있던 파워들이 모두 꺼져있다면 로직 종료
                    if (!IsKeepConnectedPower3D())
                        break;
                }
                // 2D 상태
                else
                {
                    CheckAnotherLine2D();

                    // 연결되어 있던 파워들이 모두 꺼져있다면 로직 종료
                    if (!IsKeepConnectedPower2D())
                        break;
                }
            }

            yield return null;
        }

        m_lineMeshRenderer.material.SetFloat(m_brightnessPath, m_offBrightness);
        m_isConnectedPower = false;
    }

    /// <summary>3D 상태에서 다른 라인들을 체크</summary>
    private void CheckAnotherLine3D()
    {
        for(int i = 0; i < m_lineDirection.LineDirectionCount; i++)
        {
            // 파워가 들어온 방향 이외의 방향만 파워를 보냄
            if (!m_connectedPowerDirections.Equals(m_lineDirection.LineDirection[i]))
            {
                Vector3 origin = transform.position;
                Vector3 direction = m_lineDirection.LineDirection[i];
                RaycastHit hit;

                if (GameLibrary.Raycast3D(origin, direction, out hit, m_checkDistance))
                {
                    if (hit.transform.tag.Equals(m_PowerLineTag))
                    {
                        // 체크했던 라인 리스트에 없다면 리스트에 저장
                        if (!m_checkedLines.ContainsKey(hit.transform))
                            m_checkedLines.Add(hit.transform, hit.transform.GetComponentInParent<Special_PowerLine_Line>());

                        Special_PowerLine_Line line = m_checkedLines[hit.transform];

                        // 이 라인과 연결된 라인이 있을 경우 파워 전송
                        if (line.LineDirection.IsHaveLine(-direction))
                            line.TransmitPower(this, -direction);
                    }
                }
            }
        }
    }

    /// <summary>2D 상태에서 다른 라인들을 체크</summary>
    private void CheckAnotherLine2D()
    {
        for (int i = 0; i < m_lineDirection.LineDirectionCount; i++)
        {
            // 파워가 들어온 방향 이외의 방향만 파워를 보냄
            if (!m_connectedPowerDirections.Equals(m_lineDirection.LineDirection[i]))
            {
                Vector2 origin = transform.position + m_lineDirection.LineDirection[i] * m_multiplyDirection2D;
                Vector2 direction = m_lineDirection.LineDirection[i];
                RaycastHit2D hit;

                if (GameLibrary.Raycast2D(origin, direction, out hit, m_checkDistance))
                {
                    if (hit.transform.tag.Equals(m_PowerLineTag))
                    {
                        // 체크했던 라인 리스트에 없다면 리스트에 저장
                        if (!m_checkedLines.ContainsKey(hit.transform))
                            m_checkedLines.Add(hit.transform, hit.transform.GetComponentInParent<Special_PowerLine_Line>());

                        Special_PowerLine_Line line = m_checkedLines[hit.transform];

                        // 이 라인과 연결된 라인이 있을 경우 파워 전송
                        if (line.LineDirection.IsHaveLine(-direction))
                            line.TransmitPower(this, -direction);
                    }
                }
            }
        }
    }

    /// <summary>3D 상태에서 파워가 계속 연결되어있는지 확인</summary>
    private bool IsKeepConnectedPower3D()
    {
        bool isKeepConnected = false;

        Vector3 origin = transform.position;
        Vector3 direction = m_connectedPowerDirections;
        RaycastHit hit;

        if (GameLibrary.Raycast3D(origin, direction, out hit, m_checkDistance))
        {
            // 파워일 경우 무조건 연결되어있다고 설정
            if (hit.transform.tag.Equals(m_PowerTag))
                isKeepConnected = true;
            // 라인일 경우 라인에 파워가 유지되어있는지 확인 후 설정
            else if (hit.transform.tag.Equals(m_PowerLineTag))
            {
                if (!m_checkedLines.ContainsKey(hit.transform))
                    m_checkedLines.Add(hit.transform, hit.transform.GetComponentInParent<Special_PowerLine_Line>());

                Special_PowerLine_Line line = m_checkedLines[hit.transform];

                if (line.IsConnectedPower)
                    isKeepConnected = true;
            }
        }

        return isKeepConnected;
    }

    /// <summary>2D 상태에서 파워가 계속 연결되어있는지 확인</summary>
    private bool IsKeepConnectedPower2D()
    {
        bool isKeepConnected = false;

        Vector2 origin = transform.position + m_connectedPowerDirections * m_multiplyDirection2D;
        Vector2 direction = m_connectedPowerDirections;
        RaycastHit2D hit;

        if (GameLibrary.Raycast2D(origin, direction, out hit, m_checkDistance))
        {
            // 파워일 경우 무조건 연결되어있다고 설정
            if (hit.transform.tag.Equals(m_PowerTag))
                isKeepConnected = true;
            // 라인일 경우 라인에 파워가 유지되어있는지 확인 후 설정
            else if (hit.transform.tag.Equals(m_PowerLineTag))
            {
                if (!m_checkedLines.ContainsKey(hit.transform))
                    m_checkedLines.Add(hit.transform, hit.transform.GetComponentInParent<Special_PowerLine_Line>());

                Special_PowerLine_Line line = m_checkedLines[hit.transform];

                if (line.IsConnectedPower)
                    isKeepConnected = true;
            }
        }

        return isKeepConnected;
    }

    /// <summary>스위치랑 연결되었을 경우 실행</summary>
    public void ConnectedSwitch()
    {
        StopAllCoroutines();

        if(m_connectedPowerLine != null)
            m_connectedPowerLine.ConnectedSwitch();
    }
}
