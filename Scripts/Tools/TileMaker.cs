using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileMaker : MonoBehaviour
{
    // 월드 오브젝트 메테리얼
    private static Material m_worldObject_Material;

    // 눈 지형 모음 및 필요변수
    private static List<Mesh> m_snowGrass;
    private static string m_snowGrassPath = "Model/Terrains/Snow/Tile/Terrain_Snow_Tile-";
    private static int m_snowGrassCount = 45;

    [Header("Mesh Number (0 ~ 44)")]
    // 선택한 눈 지형 번호
    [SerializeField]
    private int m_selectSnowGrassNumber;

    private void Update()
    {
        InitMaterial();
        InitSnowGrass();
    }

    public void InitMaterial()
    {
        if (m_worldObject_Material == null)
            m_worldObject_Material = Resources.Load("Materials/WorldObject/WorldObject_Material") as Material;

        // 메테리얼 설정
        GetComponentInChildren<MeshRenderer>().material = m_worldObject_Material;
    }

    public void InitSnowGrass()
    {
        if (m_snowGrass == null)
        {
            m_snowGrass = new List<Mesh>();
            for (int i = 0; i < m_snowGrassCount; i++)
                m_snowGrass.Add((Resources.Load(m_snowGrassPath + i.ToString()) as GameObject).GetComponentInChildren<MeshFilter>().sharedMesh);
        }

        // 메쉬 설정
        GetComponentInChildren<MeshFilter>().mesh = m_snowGrass[m_selectSnowGrassNumber];
    }
}