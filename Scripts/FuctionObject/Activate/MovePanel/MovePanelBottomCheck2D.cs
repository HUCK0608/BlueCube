using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovePanelBottomCheck2D : MonoBehaviour
{
    private MovePanel m_movePanel;

    private void Awake()
    {
        m_movePanel = GetComponentInParent<MovePanel>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag.Equals(GameLibrary.String_Player))
            m_movePanel.StopMovePanel();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag.Equals(GameLibrary.String_Player))
            m_movePanel.PlayMovePanel();
    }
}
