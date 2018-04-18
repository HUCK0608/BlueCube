using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject_Single : WorldObject
{
    private MeshRenderer m_renderer;
    private Collider2D m_collider2D;
    private Material m_defaultMaterial;

    protected override void Awake()
    {
        base.Awake();

        m_renderer = GetComponentInChildren<MeshRenderer>();
        m_collider2D = GetComponentInChildren<Collider2D>();
        m_defaultMaterial = m_renderer.material;
    }

    public override void SetRendererEnable(bool value)
    {
        base.SetRendererEnable(value);

        m_renderer.enabled = value;
    }

    public override void SetCollider2DEnable(bool value)
    {
        if (m_collider2D == null)
            return;

        m_collider2D.enabled = value;
    }

    public override void SetMaterial(E_MaterialType materialType)
    {
        if (materialType.Equals(E_MaterialType.Default))
        {
            m_renderer.material = m_defaultMaterial;
        }
        else if(materialType.Equals(E_MaterialType.Change))
        {
            m_renderer.material = GameLibrary.Material_CanChange;
        }
        else if(materialType.Equals(E_MaterialType.Block))
        {
            if(!m_isShowBlock)
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

        while(PlayerManager.Instance.IsViewChangeReady && isIncludeChangeViewRect)
        {
            addTime += Time.deltaTime;

            // 누적시간이 반복 시간을 넘겼다면 실행
            if(addTime >= WorldManager.Instance.ShowBlockCycleTime)
            {
                // 순서에 따른 머테리얼 변경
                if (isChangeMaterial)
                {
                    m_renderer.material = GameLibrary.Material_CanChange;
                    isChangeMaterial = false;

                    // 한 사이클이 돌았다고 설정
                    cycleCount++;
                }
                else
                {
                    m_renderer.material = GameLibrary.Material_Red;
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
