using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    private bool m_isIncludeChangeViewRect;
    /// <summary>시점변환 상자에 포함되어 있으면 true를 반환</summary>
    public bool isIncludeChangeViewRect { get { return m_isIncludeChangeViewRect; } set { m_isIncludeChangeViewRect = value; } }

    public virtual void SetRendererEnable(bool value)
    {
    }
     
    public virtual void SetCollider2DEnable(bool value)
    {
    }

    public virtual void SetMaterial(E_MaterialType materialType)
    {
    }
}
