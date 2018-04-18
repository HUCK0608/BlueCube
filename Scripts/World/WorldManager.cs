using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldManager : MonoBehaviour
{
    private static WorldManager m_instance;
    public static WorldManager Instance { get { return m_instance; } }

    private List<WorldObject> m_worldObjects;

    private int m_worldObjectCount;

    [SerializeField]
    private int m_maxShowBlockCycle;
    public int MaxShowBlockCycle { get { return m_maxShowBlockCycle; } }

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

    /// <summary>새로 생성된 오브젝트를 리스트에 포함시킴</summary>
    public void AddWorldObject(WorldObject worldObject)
    {
        worldObject.transform.parent = transform;
        m_worldObjects.Add(worldObject);
        m_worldObjectCount++;
    }

    /// <summary>월드의 오브젝트를 2D상태에 맞게 변경한다.</summary>
    public void Change2D()
    {
        for(int i = 0; i < m_worldObjectCount; i++)
        {
            // 오브젝트가 상자에 포함되어 있을경우
            if(m_worldObjects[i].isIncludeChangeViewRect)
            {
                // 2D 콜라이더 활성화
                m_worldObjects[i].SetCollider2DEnable(true);
                // 기본 메테리얼로 변경
                m_worldObjects[i].SetMaterial(E_MaterialType.Default);
            }
            // 오브젝트가 시점변환 상자에 포함되어 있지 않을 경우
            else
            {
                // 랜더러를 비활성화
                m_worldObjects[i].SetRendererEnable(false);
            }
        }
    }

    /// <summary>월드의 오브젝트를 3D상태에 맞게 변경한다.</summary>
    public void Change3D()
    {
        for(int i = 0; i < m_worldObjectCount; i++)
        {
            // 오브젝트가 시점변환 상자에 포함되어 있을 경우
            if(m_worldObjects[i].isIncludeChangeViewRect)
            {
                // 2D 콜라이더 비활성화
                m_worldObjects[i].SetCollider2DEnable(false);
                // 포함되어 있지 않은 상태로 변경
                m_worldObjects[i].isIncludeChangeViewRect = false;
            }
            // 오브젝트가 시점변환 상자에 포함되어 있지 않을 경우
            else
            {
                // 랜더러 활성화
                m_worldObjects[i].SetRendererEnable(true);
            }
        }
    }

    /// <summary>상자에 포함되어있는 놈들의 메테리얼을 기본메테리얼로 변경</summary>
    public void SetDefaultMaterialIsInclude()
    {
        for(int i = 0; i < m_worldObjectCount; i++)
        {
            if (m_worldObjects[i].isIncludeChangeViewRect)
            {
                m_worldObjects[i].SetMaterial(E_MaterialType.Default);
                m_worldObjects[i].isIncludeChangeViewRect = false;
            }
        }
    }
}
