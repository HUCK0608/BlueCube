using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour
{
    // 에임 및 에임설정 함수
    private Image m_aim;
    public void SetAimEnabled(bool value) { m_aim.enabled = value; }

    private void Awake()
    {
        m_aim = transform.Find("Aim").GetComponent<Image>();
    }
}
