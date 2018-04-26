using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager Instance { get { return m_instance; } }

    [SerializeField]
    private E_ViewType m_startView;

    private void Awake()
    {
        m_instance = this;
    }

    private void Start()
    {
        if (m_startView.Equals(E_ViewType.View2D))
            PlayerManager.Instance.Skill.StartView2D();
        else
            PlayerManager.Instance.PlayerChange3D();
    }

    /// <summary>커서 활성화</summary>
    public void SetCursorEnable(bool value)
    {
        Cursor.visible = value;

        if (value)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}
