using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryUI : MonoBehaviour
{
    private static StoryUI m_instance;
    public static StoryUI Instance { get { return m_instance; } }

    private void Awake()
    {
        m_instance = this;
    }
}
