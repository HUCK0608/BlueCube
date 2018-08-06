using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldManager : MonoBehaviour
{
    private static WorldManager m_instance;
    public static WorldManager Instance { get { return m_instance; } }

    // WorldObject 리스트 및 개수
    private List<WorldObject> m_worldObjects;
    private int m_worldObjectCount;

    // 끼임 오브젝트 표시 반복 회수
    [SerializeField]
    private int m_maxShowBlockCycleCount;
    public int MaxShowBlockCycleCount { get { return m_maxShowBlockCycleCount; } }

    // 끼임 오브젝트 표시 시간 간격
    [SerializeField]
    private float m_showBlockCycleTime;
    public float ShowBlockCycleTime { get { return m_showBlockCycleTime; } }

    private void Awake()
    {
        m_instance = this;

        m_worldObjects = new List<WorldObject>();
        m_worldObjects.AddRange(GetComponentsInChildren<WorldObject>());
        m_worldObjectCount = m_worldObjects.Count;
    }

    /// <summary>월드의 오브젝트를 2D상태로 시작</summary>
    public void StartView2D()
    {
        for (int i = 0; i < m_worldObjectCount; i++)
            m_worldObjects[i].StartView2D();
    }

    /// <summary>월드의 오브젝트를 2D상태에 맞게 변경한다.</summary>
    public void Change2D()
    {
        for (int i = 0; i < m_worldObjectCount; i++)
        {
            m_worldObjects[i].Change2D();
        }
    }

    /// <summary>월드의 오브젝트를 3D상태에 맞게 변경한다.</summary>
    public void Change3D()
    {
        for (int i = 0; i < m_worldObjectCount; i++)
            m_worldObjects[i].Change3D();
    }

    /// <summary>새로 생성된 오브젝트를 리스트에 포함시킴</summary>
    public void AddWorldObject(WorldObject worldObject)
    {
        m_worldObjects.Add(worldObject);
        m_worldObjectCount++;
    }
}
