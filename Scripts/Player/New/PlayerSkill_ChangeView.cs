using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerSkill_ChangeView : MonoBehaviour
{
    private PlayerManager m_playerManager;

    private ChangeViewRect m_changeViewRect;

    private void Awake()
    {
        m_playerManager = GetComponent<PlayerManager>();

        m_changeViewRect = GetComponentInChildren<ChangeViewRect>();
    }
}
