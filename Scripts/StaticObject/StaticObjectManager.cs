using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StaticObjectManager : MonoBehaviour
{
    // 오브젝트들
    private List<GameObject> m_objects2D;
    private List<GameObject> m_objects3D;

    // 오브젝트 개수
    private int m_objectsAmount;

    private void Awake()
    {
        InitObjects();
    }

    // 오브젝트 초기화
    private void InitObjects()
    {
        m_objectsAmount = transform.childCount;

        m_objects2D = new List<GameObject>();
        m_objects3D = new List<GameObject>();
        
        for(int i = 0; i < m_objectsAmount; i++)
        {
            m_objects2D.Add(transform.GetChild(i).Find("2D").gameObject);
            m_objects3D.Add(transform.GetChild(i).Find("3D").gameObject);
        }
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
            for (int i = 0; i < m_objectsAmount; i++)
            {
                m_objects3D[i].SetActive(false);
                m_objects2D[i].SetActive(true);
            }
        }
        else if(GameManager.Instance.ViewType == E_ViewType.View3D)
        {
            for (int i = 0; i < m_objectsAmount; i++)
            {
                m_objects2D[i].SetActive(false);
                m_objects3D[i].SetActive(true);
            }
        }
    }
}
