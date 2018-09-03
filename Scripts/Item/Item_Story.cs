using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Item_Story : MonoBehaviour
{
    [SerializeField]
    private string m_titleName;
    public string TitleName { get { return m_titleName; } }

    [TextArea(4, 4)]
    [SerializeField]
    private string m_contents;
    public string Contents { get { return m_contents; } }

    [SerializeField]
    private int m_stroyNumber;
    public int StoryNumber { get { return m_stroyNumber; } }

    /// <summary>스토리 언락</summary>
    public void UnlcokStory()
    {
        UIManager.Instance.StoryUI.UnlcokStory(this);

        EffectManager.Instance.CreateEffect(Effect_Type.Player_Story, transform.position);
        gameObject.SetActive(false);
    }
}
