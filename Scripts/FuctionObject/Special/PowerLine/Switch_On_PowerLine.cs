using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switch_On_PowerLine : Switch_On
{
    private const string m_powerLineTag = "Special_PowerLine_Line";
    private const string m_brightnessPath = "_Brightness";
    private const float m_onBrightness = 1.0f;

    private static Vector3[] m_checkDirection = new Vector3[4] { Vector3.up, Vector3.right, Vector3.left, Vector3.down };
    private const int m_checkDirectionCount = 4;

    private const float m_multiplyDirection2D = 1.05f;

    private const float m_checkDistance3D = 1.05f;
    private const float m_checkDistance2D = 0.05f;

    private Dictionary<Transform, Special_PowerLine_Line> m_checkedLine;

    [SerializeField]
    private MeshRenderer m_lineMeshRenderer;

    protected override void Awake()
    {
        base.Awake();

        m_checkedLine = new Dictionary<Transform, Special_PowerLine_Line>();
    }

    private void Start()
    {
        StartCoroutine(SwitchOnLogic());
    }

    /// <summary>스위치가 켜지는 로직(PowerLine)</summary>
    protected override IEnumerator SwitchOnLogic()
    {
        while(true)
        {
            if(!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                E_ViewType currentView = PlayerManager.Instance.CurrentView;

                if(currentView.Equals(E_ViewType.View3D))
                {
                    if (CheckConnectedPower3D())
                        break;
                }
                else
                {
                    if (CheckConnectedPower2D())
                        break;
                }
            }

            yield return null;
        }

        m_isOn = true;
        m_lineMeshRenderer.material.SetFloat(m_brightnessPath, m_onBrightness);
    }

    /// <summary>3D 상태에서 파워와 연결되어 있을경우 true를 반환</summary>
    private bool CheckConnectedPower3D()
    {
        for(int i = 0; i < m_checkDirectionCount; i++)
        {
            Vector3 origin = transform.position;
            Vector3 direction = m_checkDirection[i];
            RaycastHit hit;

            if(GameLibrary.Raycast3D(origin, direction, out hit, m_checkDistance3D))
            {
                if(hit.transform.tag.Equals(m_powerLineTag))
                {
                    if (!m_checkedLine.ContainsKey(hit.transform))
                        m_checkedLine.Add(hit.transform, hit.transform.GetComponentInParent<Special_PowerLine_Line>());

                    Special_PowerLine_Line line = m_checkedLine[hit.transform];

                    if (line.LineDirection.IsHaveLine(-direction) && line.IsConnectedPower)
                    {
                        line.ConnectedSwitch();
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>2D 상태에서 파워와 연결되어 있을경우 true를 반환</summary>
    private bool CheckConnectedPower2D()
    {
        for (int i = 0; i < m_checkDirectionCount; i++)
        {
            Vector2 origin = transform.position + m_checkDirection[i] * m_multiplyDirection2D;
            Vector2 direction = m_checkDirection[i];
            RaycastHit2D hit;

            if (GameLibrary.Raycast2D(origin, direction, out hit, m_checkDistance2D))
            {
                if (hit.transform.tag.Equals(m_powerLineTag))
                {
                    if (!m_checkedLine.ContainsKey(hit.transform))
                        m_checkedLine.Add(hit.transform, hit.transform.GetComponentInParent<Special_PowerLine_Line>());

                    Special_PowerLine_Line line = m_checkedLine[hit.transform];

                    if (line.LineDirection.IsHaveLine(-direction) && line.IsConnectedPower)
                    {
                        line.ConnectedSwitch();
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
