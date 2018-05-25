using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileMakerWindow : EditorWindow
{
    private TileMaker[] m_tileMakers;
    private int m_tileMakersCount;

    private static Vector3[] m_cullingCheckDirection = new Vector3[6] { Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back, Vector3.down };
    private static int m_cullingCheckDirectionCount = 6;
    private static float m_cullingCheckDistance = 1.05f;
    private static string m_cullingCheckTag = "Tile";

    /// <summary>선택한 타일 메이커</summary>
    private TileMaker m_selectTileMaker;
    /// <summary>선택한 타일 시리얼라이즈 오브젝트</summary>
    private SerializedObject m_selectTileSerializedObject;
    /// <summary>선택한 타일메쉬 넘버 프로퍼티</summary>
    private SerializedProperty m_selectTileMeshNumberProp;
    /// <summary>Tile Maker를 선택했을 경우 true를 반환</summary>
    private bool m_isSelectTileMaker;
    /// <summary>기존 메쉬 넘버</summary>
    private int m_oldMeshNumber;

    /// <summary>타일 메이커 초기화</summary>
    private void InitTileMakers()
    {
        m_tileMakers = (TileMaker[])FindObjectsOfType(typeof(TileMaker));
        m_tileMakersCount = m_tileMakers.Length;
    }

    [MenuItem("BlueCube/Tile Maker")]
    public static void ShowWindow()
    {
        GetWindow<TileMakerWindow>("Tile Maker");
    }

    private void OnEnable()
    {
        GetSelectTileInfo();
    }

    private void OnSelectionChange()
    {
        GetSelectTileInfo();
    }

    /// <summary>선택한 타일 정보를 가져옴</summary>
    private void GetSelectTileInfo()
    {
        GameObject selectObject = Selection.activeGameObject;

        if (selectObject != null)
        {
            m_selectTileMaker = selectObject.GetComponent<TileMaker>();

            if (m_selectTileMaker != null)
            {
                m_selectTileSerializedObject = new SerializedObject(m_selectTileMaker);
                m_selectTileMeshNumberProp = m_selectTileSerializedObject.FindProperty("m_selectSnowGrassNumber");
                m_isSelectTileMaker = true;

                return;
            }
        }

        m_isSelectTileMaker = false;
    }

    /// <summary>실제로 그려주는 부분</summary>
    private void OnGUI()
    {
        GUILayout.Label("모든 타일 설정", EditorStyles.boldLabel);

        AllTileInitModel();
        AllTileRendererEnableOn();
        AllTileRendererEnableOff();
        AllTileRendererCulling();
        SelectTileMeshChange();
    }

    /// <summary>매 주기마다 OnGUI를 그려줌</summary>
    private void OnInspectorUpdate()
    {
        Repaint();
    }

    /// <summary>모든 타일 오브젝트 모델 동기화</summary>
    private void AllTileInitModel()
    {
        if (GUILayout.Button("모든 타일 오브젝트 모델 동기화"))
        {
            InitTileMakers();

            for (int i = 0; i < m_tileMakersCount; i++)
            {
                m_tileMakers[i].InitMesh();
            }
        }
    }

    /// <summary>모든 타일 오브젝트 렌더러 켜기</summary>
    private void AllTileRendererEnableOn()
    {
        if (GUILayout.Button("모든 타일 오브젝트 렌더러 켜기"))
        {
            InitTileMakers();

            for (int i = 0; i < m_tileMakersCount; i++)
                m_tileMakers[i].GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }

    /// <summary>모든 타일 오브젝트 렌더러 끄기</summary>
    private void AllTileRendererEnableOff()
    {
        if (GUILayout.Button("모든 타일 오브젝트 렌더러 끄기"))
        {
            InitTileMakers();

            for (int i = 0; i < m_tileMakersCount; i++)
                m_tileMakers[i].GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    /// <summary>모든 타일 오브젝트 컬링</summary>
    private void AllTileRendererCulling()
    {
        if (GUILayout.Button("모든 타일 오브젝트 컬링"))
        {
            InitTileMakers();

            RaycastHit hit;

            int count = 0;

            for (int i = 0; i < m_tileMakersCount; i++)
            {
                count = 0;

                for (int j = 0; j < m_cullingCheckDirectionCount; j++)
                {
                    if (GameLibrary.Raycast3D(m_tileMakers[i].transform.position, m_cullingCheckDirection[j], out hit, m_cullingCheckDistance, GameLibrary.LayerMask_Tile))
                    {
                        count++;
                    }
                }

                if (count.Equals(m_cullingCheckDirectionCount))
                    m_tileMakers[i].GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }
    }

    /// <summary>선택 타일 메쉬 변경</summary>
    private void SelectTileMeshChange()
    {
        if(m_isSelectTileMaker)
        {
            GUILayout.Space(10f);
            EditorGUILayout.LabelField("선택한 타일 설정 (0 ~ " + (TileMaker.SnowGrassCount - 1).ToString() + ")", EditorStyles.boldLabel);

            m_selectTileSerializedObject.Update();

            m_oldMeshNumber = m_selectTileMeshNumberProp.intValue;
            m_selectTileMeshNumberProp.intValue = EditorGUILayout.IntField("Mesh Number", m_selectTileMeshNumberProp.intValue);

            // 기존 번호랑 다를 경우 메쉬 변경 후 새로운 메쉬 번호를 저장
            if(!m_oldMeshNumber.Equals(m_selectTileMeshNumberProp.intValue))
            {
                m_selectTileMaker.SetMesh(m_selectTileMeshNumberProp.intValue);
                m_oldMeshNumber = m_selectTileMeshNumberProp.intValue;
            }

            m_selectTileSerializedObject.ApplyModifiedProperties();
        }
    }

}