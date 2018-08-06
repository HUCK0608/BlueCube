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

    // 시작 시 랜더러가 꺼져있는지 여부
    private bool m_isOnStartRendererEnable;

    protected override void Awake()
    {
        base.Awake();

        m_renderer = GetComponentInChildren<MeshRenderer>();
        if (m_renderer != null)
        {
            m_defaultMaterial = m_renderer.material;
            m_defaultLightMapIndex = m_renderer.lightmapIndex;
            m_isOnStartRendererEnable = m_renderer.enabled;
        }

        m_collider2D = GetComponentInChildren<Collider2D>();

        // 스페큘러를 사용할 경우 스페큘러를 켜줌
        if (m_isUseShiningSpecular)
            m_renderer.material.SetFloat("_Specular", 1f);
    }

    /// <summary>싱글 오브젝트를 3D 상태로 변경</summary>
    public override void Change3D()
    {
        // 2D전환상자에 포함되어 있었을 경우 2D콜라이더를 비활성화 시키고 2D변환상자에서 제외되었다고 설정
        if (m_isIncludeChangeViewRect)
        {
            SetCollider2DEnable(false);
            m_isIncludeChangeViewRect = false;

            SetMaterial(E_WorldObject_ShaderType.Default3D);

            if(m_renderer != null)
                m_renderer.lightmapIndex = m_defaultLightMapIndex;

            if (!m_isOnStartRendererEnable)
            {
                if(m_renderer != null)
                    m_renderer.enabled = false;
            }
        }
        else
        {
            // 시작 랜더가 켜져있었을 경우에만 랜더를 킨다.
            if(m_isOnStartRendererEnable)
                SetRendererEnable(true);
        }
    }

    /// <summary>싱글 오브젝트를 2D 상태로 변경</summary>
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

            if(m_renderer != null)
                m_renderer.lightmapIndex = -1;

            if (!m_isOnStartRendererEnable)
            {
                if(m_renderer != null)
                    m_renderer.enabled = true;
            }
        }
        else
        {
            SetRendererEnable(false);
        }
    }

    /// <summary>싱글 오브젝트의 렌더러의 활성화여부를 설정</summary>
    public override void SetRendererEnable(bool value)
    {
        if (m_renderer == null)
            return;

        if(!m_renderer.enabled.Equals(value))
            m_renderer.enabled = value;

        m_isOnRenderer = value;
    }

    /// <summary>싱글 오브젝트의 2D콜라이더의 활성화여부를 설정</summary>
    private void SetCollider2DEnable(bool value)
    {
        if(m_collider2D != null)
            m_collider2D.enabled = value;
    }

    /// <summary>싱글 오브젝트의 쉐이더를 설정</summary>
    public override void SetMaterial(E_WorldObject_ShaderType shaderType)
    {
        if (m_renderer == null)
            return;

        if (shaderType.Equals(E_WorldObject_ShaderType.Default3D))
            m_renderer.material.SetFloat(m_shader_ChoiceString, 0f);
        else if (shaderType.Equals(E_WorldObject_ShaderType.Default2D))
            m_renderer.material.SetFloat(m_shader_ChoiceString, 1f);
        else if (shaderType.Equals(E_WorldObject_ShaderType.CanChange))
            m_renderer.material.SetFloat(m_shader_ChoiceString, 2f);
        else if (shaderType.Equals(E_WorldObject_ShaderType.Block))
            m_renderer.material.SetFloat(m_shader_ChoiceString, 3f);
        else if (shaderType.Equals(E_WorldObject_ShaderType.BackGround))
            m_renderer.material.SetFloat(m_shader_ChoiceString, 4f);
    }
}
