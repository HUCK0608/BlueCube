using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileMaker : MonoBehaviour
{
    // 눈 지형 모음 및 필요변수
    private static string m_snowGrassPath = "Model/Terrains/Snow/Tile/Terrain_Snow_Tile-";
    private static int m_snowGrassCount = 45;
    public static int SnowGrassCount { get { return m_snowGrassCount; } }

    // 선택한 눈 지형 번호
    [SerializeField]
    private int m_selectSnowGrassNumber;

    /// <summary>저장된 meshNumber 메쉬로 변경</summary>
    public void InitMesh()
    {
        GetComponentInChildren<MeshFilter>().mesh = ((Resources.Load(m_snowGrassPath + m_selectSnowGrassNumber.ToString())) as GameObject).GetComponentInChildren<MeshFilter>().sharedMesh;
    }

    /// <summary>meshNumber 메쉬로 변경</summary>
    public void SetMesh(int meshNumber)
    {
        GetComponentInChildren<MeshFilter>().mesh = ((Resources.Load(m_snowGrassPath + meshNumber.ToString())) as GameObject).GetComponentInChildren<MeshFilter>().sharedMesh;
    }
}