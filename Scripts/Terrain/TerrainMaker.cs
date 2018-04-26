using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainMaker : MonoBehaviour
{
    // 눈 지형 모음 및 필요변수
    [SerializeField]
    private static List<Mesh> m_snowGrass;
    private static string m_snowGrassPath = "Model/Terrains/Snow/Terrain_SnowGrass-";
    [SerializeField]
    private static int m_snowGrassCount = 26;

    // 선택한 눈 지형 번호
    [SerializeField]
    private int m_selectSnowGrassNumber;

    [SerializeField]
    private Material m_material;

    private void Update()
    {
        transform.GetComponentInChildren<Renderer>().material = m_material;
        ChangeTerrain();
    }

    private void InitSnowGrass()
    {
        if (m_snowGrass == null)
        {
            m_snowGrass = new List<Mesh>();
            for (int i = 0; i < m_snowGrassCount; i++)
            {
                m_snowGrass.Add((Resources.Load(m_snowGrassPath + i.ToString()) as GameObject).GetComponentInChildren<MeshFilter>().sharedMesh);
            }
        }
    }

    private void ChangeTerrain()
    {
        InitSnowGrass();
        GetComponentInChildren<MeshFilter>().mesh = m_snowGrass[m_selectSnowGrassNumber];
    }
}