using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ItemManager : MonoBehaviour
{
    private List<GameObject> m_items2D;
    private List<GameObject> m_items3D;
    private List<ColliderCheck2D> m_colChecks2D;

    private void Awake()
    {
        InitItems();
    }

    private void Start()
    {
        ChangeItems();
    }
    
    // 아이템 오브젝트 초기화
    private void InitItems()
    {
        m_items2D = new List<GameObject>();
        m_items3D = new List<GameObject>();
        m_colChecks2D = new List<ColliderCheck2D>();

        for(int i = 0; i < transform.childCount; i++)
        {
            m_items2D.Add(transform.GetChild(i).Find("2D").gameObject);
            m_items3D.Add(transform.GetChild(i).Find("3D").gameObject);
        }

        m_colChecks2D.AddRange(GetComponentsInChildren<ColliderCheck2D>());
    }

    // 아이템 변경
    public void ChangeItems()
    {
        if(GameManager.Instance.ViewType == E_ViewType.View2D)
        {
            for(int i = 0; i < m_colChecks2D.Count; i++)
            {
                m_colChecks2D[i].ColliderCheck();
            }

            for(int i = 0; i < m_items2D.Count; i++)
            {
                m_items2D[i].transform.parent = m_items3D[i].transform.parent;
                m_items3D[i].transform.parent = m_items2D[i].transform;
                m_items3D[i].SetActive(false);
                m_items2D[i].SetActive(true);
            }
        }
        else if(GameManager.Instance.ViewType == E_ViewType.View3D)
        {
            for(int i = 0; i < m_items3D.Count; i++)
            {
                m_items3D[i].transform.parent = m_items2D[i].transform.parent;
                m_items2D[i].transform.parent = m_items3D[i].transform;
                m_items2D[i].SetActive(false);
                m_items3D[i].SetActive(true);
            }
        }
    }
}
