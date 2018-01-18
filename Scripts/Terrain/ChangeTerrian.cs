using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ChangeTerrian : MonoBehaviour
{
    private SpriteRenderer m_terrian2D;
    private MeshRenderer m_terrian3D;

    private static PhysicsMaterial2D m_noFriction2D;

    private void Awake()
    {
        InitTerrian();
    }

    // 초기화
    private void InitTerrian()
    {
        m_terrian2D = transform.Find("2D").GetComponent<SpriteRenderer>();
        m_terrian3D = transform.Find("3D").GetComponent<MeshRenderer>();

        if(m_noFriction2D == null)
            m_noFriction2D = Resources.Load<PhysicsMaterial2D>("PhysicsMaterials/NoFriction2D");

        // 2D 콜라이더가 없다면
        if (m_terrian2D.GetComponent<Collider2D>() == null)
        {
            // 2D 콜라이더 추가
            BoxCollider2D collider2D = m_terrian2D.gameObject.AddComponent<BoxCollider2D>();
            collider2D.sharedMaterial = m_noFriction2D;
            
        }
    }

    // 지형 변경
    public void Change()
    {
        if(GameManager.Instance.ViewType == E_ViewType.View2D)
        {
            m_terrian3D.enabled = false;
            m_terrian2D.enabled = true;
        }
        else if(GameManager.Instance.ViewType == E_ViewType.View3D)
        {
            m_terrian2D.enabled = false;
            m_terrian3D.enabled = true;
        }
    }
}
