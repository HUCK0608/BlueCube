using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileMakerWindow : EditorWindow
{
    private static Vector3[] m_cullingCheckDirection = new Vector3[6] { Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back, Vector3.down };
    private static int m_cullingCheckDirectionCount = 6;
    private static float m_cullingCheckDistance = 1.05f;
    private static string m_cullingCheckTag = "Tile";

    private TileMaker[] m_tileMakers;
    private int m_tileMakersCount;

    private void Awake()
    {
        // Tile Maker 스크립트를 모두 가져옴
        m_tileMakers = (TileMaker[])FindObjectsOfType(typeof(TileMaker));
        m_tileMakersCount = m_tileMakers.Length;
    }

    [MenuItem("BlueCube/Tile Maker")]
    public static void ShowWindow()
    {
        GetWindow<TileMakerWindow>("Tile Maker");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("모든 오브젝트 동기화"))
        {
            for(int i = 0; i < m_tileMakersCount; i++)
            {
                m_tileMakers[i].InitMaterial();
                m_tileMakers[i].InitSnowGrass();
            }
        }

        if(GUILayout.Button("모든 오브젝트 렌더러 켜기"))
        {
            for (int i = 0; i < m_tileMakersCount; i++)
                m_tileMakers[i].GetComponentInChildren<MeshRenderer>().enabled = true;
        }

        if(GUILayout.Button("모든 오브젝트 컬링"))
        {
            RaycastHit hit;

            int count = 0;

            for(int i = 0; i < m_tileMakersCount; i++)
            {
                count = 0;

                for(int j = 0; j < m_cullingCheckDirectionCount; j++)
                {
                    if(GameLibrary.Raycast3D(m_tileMakers[i].transform.position, m_cullingCheckDirection[j], out hit, m_cullingCheckDistance, GameLibrary.LayerMask_Tile))
                    {
                        count++;
                    }
                }

                if (count.Equals(m_cullingCheckDirectionCount))
                    m_tileMakers[i].GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }
    }
}
