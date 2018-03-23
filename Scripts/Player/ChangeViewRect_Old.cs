using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ChangeViewRect_Old : MonoBehaviour
{
    private List<WorldObject> m_includeWO;

    private Collider m_collider;

    private static string m_playerTag = "Player";

    private void Awake()
    {
        m_includeWO = new List<WorldObject>();

        m_collider = GetComponent<Collider>();
    }

    /// <summary>2D변경상자의 충돌체크 설정</summary>
    public void CollisionCheckEnable(bool value)
    {
        m_collider.enabled = value;
    }

    public void IncludeWOEnable(bool value)
    {
        int m_includeWOCount = m_includeWO.Count;

        for(int i = 0; i < m_includeWOCount; i++)
        {
            m_includeWO[i].SetRendererEnable(value);
            m_includeWO[i].SetCollider2DEnable(value);
        }

        if (!value)
            ClearList();
    }

    public void SetDefaultMaterial()
    {
        int m_includeWOCount = m_includeWO.Count;

        for(int i = 0; i < m_includeWOCount; i++)
        {
            m_includeWO[i].SetMaterial(GameLibrary.Enum_Material_Default);
        }
    }

    public void ClearList()
    {
        m_includeWO.Clear();
    }

    // worldObject 리스트에 포함하기
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != m_playerTag)
        {
            WorldObject worldObject = other.GetComponentInParent<WorldObject>();

            if (worldObject != null)
            {
                // 메테리얼 변경
                worldObject.SetMaterial(GameLibrary.Enum_Material_Change);
                // 포함
                m_includeWO.Add(worldObject);
            }
        }
    }

    // 포함된 worldObejct 리스트에서 제외하기
    private void OnTriggerExit(Collider other)
    {
        if(other.tag != m_playerTag)
        {
            WorldObject worldObject = other.GetComponentInParent<WorldObject>();

            if(worldObject != null)
            {
                // 메테리얼 변경
                worldObject.SetMaterial(GameLibrary.Enum_Material_Default);
                // 제외
                m_includeWO.Remove(worldObject);
            }
        }
    }
}
