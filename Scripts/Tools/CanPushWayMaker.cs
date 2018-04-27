using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public sealed class CanPushWayMaker : MonoBehaviour
{
    // 월드 오브젝트 메테리얼
    private static Material m_worldObject_Material;

    // 눈 밀기가능 지형 모음 및 필요변수
    private static List<Mesh> m_snowCanPushWay;
    private static string m_snowCanPushWayPath = "Model/Terrains/Snow/Terrain_Snow_CanPushWay-";
    private static int m_snowCanPushWayCount = 4;

    // 선택한 눈 밀기가능 지형
    [SerializeField]
    private int m_selectSnowCanPushWayNumber;

    private void Update()
    {
        InitMaterial();
        InitSnowCanPushWay();
    }

    private void InitMaterial()
    {
        if (m_worldObject_Material == null)
            m_worldObject_Material = Resources.Load("Materials/WorldObject/WorldObject_Material") as Material;

        // 메테리얼 설정
        GetComponentInChildren<MeshRenderer>().material = m_worldObject_Material;
    }

    private void InitSnowCanPushWay()
    {
        if(m_snowCanPushWay == null)
        {
            m_snowCanPushWay = new List<Mesh>();
            for (int i = 0; i < m_snowCanPushWayCount; i++)
                m_snowCanPushWay.Add((Resources.Load(m_snowCanPushWayPath + i.ToString()) as GameObject).GetComponentInChildren<MeshFilter>().sharedMesh);
        }

        // 메쉬설정
        GetComponentInChildren<MeshFilter>().mesh = m_snowCanPushWay[m_selectSnowCanPushWayNumber];
    }
}
