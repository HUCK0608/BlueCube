using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectManager : MonoBehaviour
{
    // 오브젝트들
    private List<SpriteRenderer> m_objects2D;
    private List<MeshRenderer> m_objects3D;

    private void Awake()
    {
        InitObjects();
    }

    // 오브젝트 초기화
    private void InitObjects()
    {
        m_objects2D = new List<SpriteRenderer>();
        m_objects3D = new List<MeshRenderer>();

        m_objects2D.AddRange(GetComponentsInChildren<SpriteRenderer>());
        m_objects3D.AddRange(GetComponentsInChildren<MeshRenderer>());
    }

    private void Start()
    {
        ChangeObjects();
    }

    // 오브젝트 변경
    public void ChangeObjects()
    {
        if(GameManager.Instance.ViewType == E_ViewType.View2D)
        {
            for (int i = 0; i < m_objects3D.Count; i++)
                m_objects3D[i].enabled = false;

            for (int i = 0; i < m_objects2D.Count; i++)
                m_objects2D[i].enabled = true;
        }
        else if(GameManager.Instance.ViewType == E_ViewType.View3D)
        {
            for (int i = 0; i < m_objects2D.Count; i++)
                m_objects2D[i].enabled = false;

            for (int i = 0; i < m_objects3D.Count; i++)
                m_objects3D[i].enabled = true;
        }
    }
}
