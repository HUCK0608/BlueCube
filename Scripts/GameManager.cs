using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager Instance { get { return m_instance; } }

    private void Awake()
    {
        m_instance = this;
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
