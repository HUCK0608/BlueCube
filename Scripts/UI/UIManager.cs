using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public sealed class UIManager : MonoBehaviour
{
    private Text m_playerHpUI;
    [SerializeField]
    private string m_drawPlayerHpFirstString;

    private void Awake()
    {
        m_playerHpUI = transform.Find("PlayerHpUI").GetComponent<Text>();
    }

    private void Update()
    {
        DrawPlayerHpUI();
    }

    // 플레이어 체력 UI를 그려줌
    private void DrawPlayerHpUI()
    {
        m_playerHpUI.text = m_drawPlayerHpFirstString + PlayerManager.Instance.Stat.Hp.ToString();
    }
}
