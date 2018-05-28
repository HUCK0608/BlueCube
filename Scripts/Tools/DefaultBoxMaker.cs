using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public sealed class DefaultBoxMaker : MonoBehaviour
{
    private static Material m_worldObject_Material;

    private static List<Mesh> m_snowBox;
    private static string m_snowBoxPath = "Model/BackGround/Snow/Box/BackGround_Snow_Box-";
    private static int m_snowBoxCount = 7;

    [Header("-1 랜덤, 0~6 모델")]
    [SerializeField]
    private int m_selectSnowBoxNumber = -1;

    private void Update()
    {
        InitMaterial();
        InitSnowBox();
    }

    private void InitMaterial()
    {
        if(m_worldObject_Material == null)
            m_worldObject_Material = Resources.Load("Materials/WorldObject/WorldObject_Material") as Material;

        GetComponentInChildren<MeshRenderer>().material = m_worldObject_Material;
    }

    private void InitSnowBox()
    {
        if(m_snowBox == null)
        {
            m_snowBox = new List<Mesh>();
            for (int i = 0; i < m_snowBoxCount; i++)
                m_snowBox.Add((Resources.Load(m_snowBoxPath + i.ToString()) as GameObject).GetComponentInChildren<MeshFilter>().sharedMesh);
        }

        if (m_selectSnowBoxNumber.Equals(-1))
            GetComponentInChildren<MeshFilter>().mesh = m_snowBox[Random.Range(0, 7)];
        else
            GetComponentInChildren<MeshFilter>().mesh = m_snowBox[m_selectSnowBoxNumber];
    }
}
