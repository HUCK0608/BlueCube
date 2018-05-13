using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Item_Story : MonoBehaviour
{
    [SerializeField]
    private string m_titleName;
    public string TitleName { get { return m_titleName; } }

    [TextArea(5, 5)]
    [SerializeField]
    private string m_contents;
    public string Contents { get { return m_contents; } }

    /// <summary>스토리 획득</summary>
    public void GetStory()
    {
        StoryManager.Instance.AddStory(this);

        gameObject.SetActive(false);
    }
}
