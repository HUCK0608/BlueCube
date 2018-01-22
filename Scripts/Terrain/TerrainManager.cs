using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TerrainManager : MonoBehaviour
{
    // 변경해야되는 지형
    private List<ChangeTerrian> m_changeTerrains;

    // 변경해야되는 지형 개수
    private int m_changeTerrainAmount;

    private void Awake()
    {
        InitTerrian();
    }

    private void Start()
    {
        ChangeTerrain();
    }

    // 지형 초기화
    private void InitTerrian()
    {
        m_changeTerrains = new List<ChangeTerrian>();

        // 리스트에 추가
        m_changeTerrains.AddRange(GetComponentsInChildren<ChangeTerrian>());

        // 개수 설정
        m_changeTerrainAmount = m_changeTerrains.Count;
    }

    // 지형 변경
    public void ChangeTerrain()
    {
        for (int i = 0; i < m_changeTerrainAmount; i++)
            m_changeTerrains[i].Change();
    }
}
