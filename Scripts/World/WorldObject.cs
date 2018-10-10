using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_WorldObject_ShaderType { Default3D, Default2D, CanChange, Block, BackGround }

[SelectionBase]
public class WorldObject : MonoBehaviour
{
    protected static string m_shader_ChoiceString = "_Choice";

    // 2D 텍스쳐를 사용할지 여부
    [SerializeField]
    protected bool m_isUse2DTexture;
    // 샤이닝 스펙트럼을 사용할지 여부
    [SerializeField]
    protected bool m_isUseShiningSpecular;

    protected bool m_isOnRenderer;
    /// <summary>오브젝트의 렌더러가 활성화 되어 있을 경우 true를 반환</summary>
    public bool IsOnRenderer { get { return m_isOnRenderer; } }

    protected bool m_isIncludeChangeViewRect;
    /// <summary>시점변환 상자에 포함되어 있으면 true를 반환</summary>
    public bool IsIncludeChangeViewRect { get { return m_isIncludeChangeViewRect; } set { m_isIncludeChangeViewRect = value; } }

    protected virtual void Awake()
    {
        m_isOnRenderer = true;
    }

    /// <summary>2D 상태로 시작</summary>
    public void StartView2D()
    {
        m_isIncludeChangeViewRect = true;
        Change2D();
    }

    /// <summary>오브젝트를 2D상태로 변경</summary>
    public virtual void Change2D() { }
    /// <summary>오브젝트를 3D상태로 변경</summary>
    public virtual void Change3D() { }
    /// <summary>오브젝트의 렌더러 활성화 여부를 설정</summary>
    public virtual void SetRendererEnable(bool value) { }
    /// <summary>오브젝트의 쉐이더를 변경</summary>
    public virtual void SetMaterial(E_WorldObject_ShaderType ShaderType) { }
}
