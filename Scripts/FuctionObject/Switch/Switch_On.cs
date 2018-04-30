using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_On : Switch
{
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>스위치를 킨다</summary>
    public virtual void SwitchOn()
    {
        if (!m_isButtonMove && !m_isOn)
            StartCoroutine(MoveButton());
    }

    // 버튼 이동
    private IEnumerator MoveButton()
    {
        m_isButtonMove = true;

        while(true)
        {
            if(!GameLibrary.Bool_IsGameStop(m_worldObject))
            {
                m_button.localPosition = Vector3.MoveTowards(m_button.localPosition, m_buttonOnPosition, m_buttonMoveSpeed * Time.deltaTime);

                if (m_button.localPosition.Equals(m_buttonOnPosition))
                    break;
            }

            yield return null;
        }

        m_isButtonMove = false;
        m_isOn = true;
        m_buttonMeshFilter.mesh = m_buttonOnMesh;
    }
}
