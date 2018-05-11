using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public sealed class UIManager : MonoBehaviour
{
    private static UIManager m_instance;
    public static UIManager Instance { get { return m_instance; } }

    [Header("Player Hp")]
    [SerializeField]
    private Text m_playerHpText;
    [SerializeField]
    private Color m_playerHpChangeColor;
    [SerializeField]
    private float m_playerHpChangeColorDurationTime;
    private Coroutine m_playerHpTextColorChangeCor;
    
    private void Awake()
    {
        m_instance = this;
    }

    private void Start()
    {
        InitUI();
    }

    /// <summary>UI 초기화</summary>
    private void InitUI()
    {
        SetPlayerHpText(PlayerManager.Instance.Stat.Hp);
    }

    /// <summary>플레이어 체력 텍스트를 변경</summary>
    public void SetPlayerHpText(int hp)
    {
        m_playerHpText.text = hp.ToString();

        if(m_playerHpTextColorChangeCor != null)
            StopCoroutine(m_playerHpTextColorChangeCor);
        m_playerHpTextColorChangeCor = StartCoroutine(PlayerHpTextColorChange());
    }

    /// <summary>플레이어 체력 텍스트의 색을 변경</summary>
    private IEnumerator PlayerHpTextColorChange()
    {
        m_playerHpText.color = m_playerHpChangeColor;

        float addTime = 0f;

        while(true)
        {
            addTime += Time.deltaTime;

            if (addTime >= m_playerHpChangeColorDurationTime)
                break;

            yield return null;
        }

        m_playerHpText.color = Color.white;

        m_playerHpTextColorChangeCor = null;
    }
}
