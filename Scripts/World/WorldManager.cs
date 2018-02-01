using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldManager : MonoBehaviour
{
    private List<WorldObject> m_worldObjects;

    private int m_worldObjectCount;

    private void Awake()
    {
        m_worldObjects = new List<WorldObject>();

        m_worldObjects.AddRange(GetComponentsInChildren<WorldObject>());

        m_worldObjectCount = m_worldObjects.Count;
    }

    public void RendererEnable(bool value)
    {
        for (int i = 0; i < m_worldObjectCount; i++)
        {
            m_worldObjects[i].RendererEnable(value);
        }
    }
}
