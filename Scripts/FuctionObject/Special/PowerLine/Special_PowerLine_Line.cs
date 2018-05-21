using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Special_PowerLine_Line : MonoBehaviour
{
    private static string m_powerPath = "Special_PowerLine_Power";
    private static string m_linePath = "Special_PowerLine_Line";

    [Header("Can Change")]

    /// <summary>레이체크 거리</summary>
    [SerializeField]
    private float m_checkDistance;

    [Header("Don't Touch")]

    /// <summary>모델</summary>
    [SerializeField]
    private Transform m_model;
    public Transform Model { get { return m_model; } }

    /// <summary>라인이 어느방향에 있는지에 대한 변수</summary>
    private Special_PowerLine_LineDirection m_lineDirection;

    /// <summary>충돌되었던 라인 모음</summary>
    private Dictionary<Transform, Special_PowerLine_Line> m_collisionLines;

    /// <summary>파워와 연결되어 있다면 true를 반환</summary>
    [SerializeField]
    private bool m_isOn;
    public bool IsOn { get { return m_isOn; } }

    private void Awake()
    {
        m_lineDirection = GetComponent<Special_PowerLine_LineDirection>();
        m_collisionLines = new Dictionary<Transform, Special_PowerLine_Line>();
    }

    private void Start()
    {
        StartCoroutine(CheckPowerLine2D());
    }

    /// <summary>해당 방향으로 연결되어 있으면 true를 반환</summary>
    public bool CheckConnected(Vector3 direction)
    {
        if (m_lineDirection.IsOnDirection.Contains(-direction))
            return true;

        return false;
    }

    /// <summary>2D상태에서 파워라인을 체크</summary>
    private IEnumerator CheckPowerLine2D()
    {
        float multiplyOrigin = 1.05f;

        while (true)
        {
            for(int i = 0; i < m_lineDirection.IsOnDirectionCount; i++)
            {
                Vector2 origin = transform.position + m_lineDirection.IsOnDirection[i] * multiplyOrigin;
                Vector2 checkDirection = m_lineDirection.IsOnDirection[i];
                RaycastHit2D hit;

                if (GameLibrary.Raycast2D(origin, checkDirection, out hit, m_checkDistance))
                {
                    // 파워랑 붙어있을 경우 전원을 킴
                    if (hit.transform.tag.Equals(m_powerPath))
                    {
                        m_isOn = true;
                        break;
                    }

                    // 라인이랑 붙어있을 경우 예전에 충돌했던 적이 있는 라인인지 체크 후 그 라인이 켜져있는지 체크
                    if (hit.transform.tag.Equals(m_linePath))
                    {
                        // 한 번도 마주친 적 없는 라인일 경우 리스트에 추가
                        if (!m_collisionLines.ContainsKey(hit.transform))
                        {
                            m_collisionLines.Add(hit.transform, hit.transform.GetComponentInParent<Special_PowerLine_Line>());
                            Debug.Log("addList");
                        }

                        // 내 라인이 충돌된 라인과 연결되어 있고 켜져있는 라인이라면 내 라인도 켜져있다고 설정
                        if (m_collisionLines[hit.transform].CheckConnected(checkDirection) && m_collisionLines[hit.transform].IsOn)
                        {
                            m_isOn = true;
                            break;
                        }
                    }
                }

                if(i.Equals(m_lineDirection.IsOnDirectionCount - 1))
                {
                    m_isOn = false;
                    break;
                }
            }

            yield return null;
        }
    }
}
