using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyManager : MonoBehaviour
{
    // 적 모음
    private List<GameObject> m_enemy2D;
    private List<GameObject> m_enemy3D;

    // 적 개수
    int m_enemyCount;

    private void Awake()
    {
        InitEnemies();
    }

    // 적 초기화
    private void InitEnemies()
    {
        m_enemyCount = transform.childCount;

        m_enemy2D = new List<GameObject>();
        m_enemy3D = new List<GameObject>();

        // 리스트에 담기
        for (int i = 0; i < m_enemyCount; i++)
        {
            m_enemy2D.Add(transform.GetChild(i).Find("2D").gameObject);
            m_enemy3D.Add(transform.GetChild(i).Find("3D").gameObject);
        }
    }

    private void Start()
    {
        ChangeEnemies();
    }

    // 적 변경
    public void ChangeEnemies()
    {
        // 2D로 변경
        if(GameManager.Instance.ViewType == E_ViewType.View2D)
        {
            for(int i = 0; i < m_enemyCount; i++)
            {
                m_enemy3D[i].SetActive(false);
                m_enemy2D[i].SetActive(true);
            }
        }
        // 3D로 변경
        else if(GameManager.Instance.ViewType == E_ViewType.View3D)
        {
            for(int i = 0; i <m_enemyCount; i++)
            {
                m_enemy2D[i].SetActive(false);
                m_enemy3D[i].SetActive(true);
            }
        }
    }
}
