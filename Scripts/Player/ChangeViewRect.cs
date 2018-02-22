using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ChangeViewRect : MonoBehaviour
{
    private List<WorldObject> m_includeWO;

    private Collider m_collider;

    private static string m_playerTag = "Player";

    private void Awake()
    {
        m_includeWO = new List<WorldObject>();

        m_collider = GetComponent<Collider>();
    }

    public void CheckIncludeWO(bool value)
    {
        m_collider.enabled = value;
    }

    public void IncludeWOEnable(bool value)
    {
        int m_includeWOCount = m_includeWO.Count;

        for(int i = 0; i < m_includeWOCount; i++)
        {
            m_includeWO[i].RendererEnable(value);
            m_includeWO[i].Collider2DEnable(value);
        }

        if (!value)
            m_includeWO.Clear();
    }

    public void SetDefaultMaterial()
    {
        int m_includeWOCount = m_includeWO.Count;

        for(int i = 0; i < m_includeWOCount; i++)
        {
            m_includeWO[i].ChangeMaterial(GameLibrary.Enum_Material_Default);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != m_playerTag)
        {
            WorldObject worldObject = other.GetComponentInParent<WorldObject>();

            if (worldObject != null)
            {
                worldObject.ChangeMaterial(GameLibrary.Enum_Material_Change);
                m_includeWO.Add(worldObject);
            }
        }
    }
}
