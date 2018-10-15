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
    private int m_storyNumber;
    public int StoryNumber { get { return m_storyNumber; } }

    [Header("UpDown Properties")]
    [SerializeField]
    private float m_upDownRange;

    [SerializeField]
    private float m_upDownMoveSpeed;

    private bool m_isUp;

    private Coroutine m_upDownMoveCor;

    private void Start()
    {
        m_upDownMoveCor = StartCoroutine(UpDownMove());
    }

    /// <summary>스토리 언락</summary>
    public void UnlcokStory()
    {
        UIManager.Instance.StoryUI.UnlcokStory(this);

        EffectManager.Instance.CreateEffect(Effect_Type.Player_Story, transform.position);
        StopCoroutine(m_upDownMoveCor);
        gameObject.SetActive(false);
    }

    private IEnumerator UpDownMove()
    {
        Vector3 oldPosition = transform.position;
        Vector3 upPosition = oldPosition;
        upPosition.y += m_upDownRange;
        Vector3 downPosition = oldPosition;
        downPosition.y -= m_upDownRange;

        while (true)
        {
            if (m_isUp)
                transform.position = Vector3.MoveTowards(transform.position, upPosition, m_upDownMoveSpeed * Time.deltaTime);
            else
                transform.position = Vector3.MoveTowards(transform.position, downPosition, m_upDownMoveSpeed * Time.deltaTime);

            if (m_isUp && transform.position.Equals(upPosition))
                m_isUp = false;
            else if (!m_isUp && transform.position.Equals(downPosition))
                m_isUp = true;

            yield return null;
        }
    }
}
