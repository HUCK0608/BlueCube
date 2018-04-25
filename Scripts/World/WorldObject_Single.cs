using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject_Single : WorldObject
{
    // 오브젝트 설정에 필요한 변수들
    private MeshRenderer m_renderer;
    private Material m_defaultMaterial;
    private Collider2D m_collider2D;
    private int m_defaultLightMapIndex;

    protected override void Awake()
    {
        base.Awake();

        m_renderer = GetComponentInChildren<MeshRenderer>();
        m_defaultMaterial = m_renderer.material;
        m_collider2D = GetComponentInChildren<Collider2D>();
        m_defaultLightMapIndex = m_renderer.lightmapIndex;
    }

    /// <summary>싱글 오브젝트를 3D 상태로 변경</summary>
    public override void Change3D()
    {
        // 2D전환상자에 포함되어 있었을 경우 2D콜라이더를 비활성화 시키고 2D변환상자에서 제외되었다고 설정
        if (m_isIncludeChangeViewRect)
        {
            SetCollider2DEnable(false);
            m_isIncludeChangeViewRect = false;
            SetMaterial(E_MaterialType.Default);

            if (m_isUse2DTexture)
                m_renderer.material.SetFloat(Shader_ChoiceString, 0);

            m_renderer.lightmapIndex = m_defaultLightMapIndex;
        }
        // 2D전환상자에 포함되어 있지 않았을 경우 렌더러가 비활성화 된 오브젝트의 렌더러를 킨다
        else
        {
            if (!m_isOnRenderer)
                SetRendererEnable(true);
        }
    }

    /// <summary>싱글 오브젝트를 2D 상태로 변경</summary>
    public override void Change2D()
    {
        // 2D전환상자에 포함되어 있을 경우 2D콜라이더를 활성화 시킨다
        if(m_isIncludeChangeViewRect)
        {
            SetCollider2DEnable(true);
            SetMaterial(E_MaterialType.Default);

            if (m_isUse2DTexture)
                m_renderer.material.SetFloat(Shader_ChoiceString, 1);

            m_renderer.lightmapIndex = -1;
        }
        // 2D전환상자에 포함되어 있지 않을 경우 렌더러를 끈다
        else
        {
            SetRendererEnable(false);
        }
    }

    /// <summary>싱글 오브젝트의 렌더러의 활성화여부를 설정</summary>
    public override void SetRendererEnable(bool value)
    {
        m_renderer.enabled = value;
        m_isOnRenderer = value;
    }

    /// <summary>싱글 오브젝트의 2D콜라이더의 활성화여부를 설정</summary>
    private void SetCollider2DEnable(bool value)
    {
        if(m_collider2D != null)
            m_collider2D.enabled = value;
    }

    /// <summary>싱글 오브젝트의 메테리얼을 설정</summary>
    public override void SetMaterial(E_MaterialType materialType)
    {
        if (materialType.Equals(E_MaterialType.Default))
        {
            m_renderer.material = m_defaultMaterial;
        }
        else if (materialType.Equals(E_MaterialType.Change))
        {
            m_renderer.material = GameLibrary.Material_CanChange;
        }
        else if (materialType.Equals(E_MaterialType.Block))
        {
            m_renderer.material = GameLibrary.Material_Block;
        }
    }
}
