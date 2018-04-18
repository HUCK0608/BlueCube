using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject_Multi : WorldObject
{
    private List<MeshRenderer> m_renderers;
    private List<Collider2D> m_collider2D;
    private List<Material> m_defaultMaterials;

    private int m_rendererCount;
    private int m_colliderCount;
    private int m_materialCount;

    protected override void Awake()
    {
        base.Awake();

        m_renderers = new List<MeshRenderer>();
        m_collider2D = new List<Collider2D>();
        m_defaultMaterials = new List<Material>();

        m_renderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        m_collider2D.AddRange(GetComponentsInChildren<Collider2D>());

        m_rendererCount = m_renderers.Count;
        m_colliderCount = m_collider2D.Count;

        for(int i = 0; i < m_rendererCount; i++)
            m_defaultMaterials.Add(m_renderers[i].material);

        m_materialCount = m_defaultMaterials.Count;
    }

    public override void SetRendererEnable(bool value)
    {
        base.SetRendererEnable(value);

        for (int i = 0; i < m_rendererCount; i++)
            m_renderers[i].enabled = value;
    }

    public override void SetCollider2DEnable(bool value)
    {
        for (int i = 0; i < m_colliderCount; i++)
            m_collider2D[i].enabled = value;
    }

    public override void SetMaterial(E_MaterialType materialType)
    {
        if(materialType.Equals(E_MaterialType.Default))
        {
            for (int i = 0; i < m_materialCount; i++)
                m_renderers[i].material = m_defaultMaterials[i];
        }
        else if(materialType.Equals(E_MaterialType.Change))
        {
            for (int i = 0; i < m_materialCount; i++)
                m_renderers[i].material = GameLibrary.Material_CanChange;
        }
        else if(materialType.Equals(E_MaterialType.Block))
        {
            if (!m_isShowBlock)
                StartCoroutine(ShowBlock());
        }
    }

    private IEnumerator ShowBlock()
    {
        m_isShowBlock = true;

        // 반복 횟수
        int cycleCount = 0;
        // 누적 시간
        float addTime = 0f;

        bool isChangeMaterial = false;

        while (PlayerManager.Instance.IsViewChangeReady && isIncludeChangeViewRect)
        {
            addTime += Time.deltaTime;

            // 누적시간이 반복 시간을 넘겼다면 실행
            if (addTime >= WorldManager.Instance.ShowBlockCycleTime)
            {
                // 순서에 따른 머테리얼 변경
                if (isChangeMaterial)
                {
                    SetMaterial(E_MaterialType.Change);
                    isChangeMaterial = false;

                    // 한 사이클이 돌았다고 설정
                    cycleCount++;
                }
                else
                {
                    for (int i = 0; i < m_rendererCount; i++)
                        m_renderers[i].material = GameLibrary.Material_Red;

                    isChangeMaterial = true;
                }


                if (cycleCount.Equals(WorldManager.Instance.MaxShowBlockCycle))
                    break;

                // 누적시간 초기화
                addTime = 0f;
            }

            yield return null;
        }

        // 현재 상태에 따라 메테리얼 변경
        if (isIncludeChangeViewRect)
            SetMaterial(E_MaterialType.Change);
        else
            SetMaterial(E_MaterialType.Default);

        m_isShowBlock = false;
    }
}
