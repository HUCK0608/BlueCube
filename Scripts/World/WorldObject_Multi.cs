using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject_Multi : WorldObject
{
    private List<MeshRenderer> m_renderers;
    private List<Material> m_defaultMaterials;
    private List<int> m_defaultLightMapIndex;
    private int m_rendererCount;

    private List<Collider2D> m_collider2D;
    private int m_collider2DCount;

    protected override void Awake()
    {
        base.Awake();

        m_renderers = new List<MeshRenderer>();
        m_defaultMaterials = new List<Material>();
        m_defaultLightMapIndex = new List<int>();
        m_collider2D = new List<Collider2D>();

        m_renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        m_collider2D.AddRange(GetComponentsInChildren<Collider2D>());

        m_rendererCount = m_renderers.Count;
        m_collider2DCount = m_collider2D.Count;

        for (int i = 0; i < m_rendererCount; i++)
        {
            m_defaultMaterials.Add(m_renderers[i].material);
            m_defaultLightMapIndex.Add(m_renderers[i].lightmapIndex);

            // 스페큘러를 사용할 경우 스페큘러를 켜줌
            if (m_isUseShiningSpecular)
                m_renderers[i].material.SetFloat("_Specular", 1f);
        }
    }

    /// <summary>멀티 오브젝트를 3D 상태로 변경</summary>
    public override void Change3D()
    {
        // 2D전환상자에 포함되어 있었을 경우 2D콜라이더를 비활성화 시키고 2D변환상자에서 제외되었다고 설정
        if (m_isIncludeChangeViewRect)
        {
            SetCollider2DEnable(false);
            m_isIncludeChangeViewRect = false;

            SetMaterial(E_WorldObject_ShaderType.Default3D);

            for (int i = 0; i < m_rendererCount; i++)
                m_renderers[i].lightmapIndex = m_defaultLightMapIndex[i];
        }
        // 2D전환상자에 포함되어 있지 않았을 경우 렌더러가 비활성화 된 오브젝트의 렌더러를 킨다
        else
        {
            if (!m_isOnRenderer)
                SetRendererEnable(true);
        }
    }

    /// <summary>멀티 오브젝트를 2D 상태로 변경</summary>
    public override void Change2D()
    {
        // 2D전환상자에 포함되어 있을 경우 2D콜라이더를 활성화 시킨다
        if (m_isIncludeChangeViewRect)
        {
            SetCollider2DEnable(true);

            if (m_isUse2DTexture)
                SetMaterial(E_WorldObject_ShaderType.Default2D);
            else
                SetMaterial(E_WorldObject_ShaderType.Default3D);

            for (int i = 0; i < m_rendererCount; i++)
                m_renderers[i].lightmapIndex = -1;
        }
        // 2D전환상자에 포함되어 있지 않을 경우 렌더러를 끈다
        else
        {
            SetRendererEnable(false);
        }
    }

    /// <summary>멀티 오브젝트의 활성화 여부를 설정</summary>
    public override void SetRendererEnable(bool value)
    {
        for (int i = 0; i < m_rendererCount; i++)
            m_renderers[i].enabled = value;
        m_isOnRenderer = value;
    }

    /// <summary>멀티 오브젝트의 2D콜라이더 활성화 여부를 설정</summary>
    private void SetCollider2DEnable(bool value)
    {
        if(m_collider2D != null)
        {
            for (int i = 0; i < m_collider2DCount; i++)
                m_collider2D[i].enabled = value;
        }
    }

    /// <summary>멀티 오브젝트의 메테리얼을 설정</summary>
    public override void SetMaterial(E_WorldObject_ShaderType materialType)
    {
        if (materialType.Equals(E_WorldObject_ShaderType.Default3D))
        {
            for (int i = 0; i < m_rendererCount; i++)
                m_renderers[i].material.SetFloat(m_shader_ChoiceString, 0f);
        }
        else if (materialType.Equals(E_WorldObject_ShaderType.Default2D))
        {
            for (int i = 0; i < m_rendererCount; i++)
                m_renderers[i].material.SetFloat(m_shader_ChoiceString, 1f);
        }
        else if (materialType.Equals(E_WorldObject_ShaderType.CanChange))
        {
            for (int i = 0; i < m_rendererCount; i++)
                m_renderers[i].material.SetFloat(m_shader_ChoiceString, 2f);
        }
        else if (materialType.Equals(E_WorldObject_ShaderType.Block))
        {
            for (int i = 0; i < m_rendererCount; i++)
                m_renderers[i].material.SetFloat(m_shader_ChoiceString, 3f);
        }
    }
}
