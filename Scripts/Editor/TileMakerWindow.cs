using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileMakerWindow : EditorWindow
{
    // 타일 컬링 관련 변수
    /// <summary>컬링 체크 6방향</summary>
    private static Vector3[] m_cullingCheckDirection = new Vector3[6] { Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back, Vector3.down };
    /// <summary>컬링 체크 방향 개수</summary>
    private static int m_cullingCheckDirectionCount = 6;
    /// <summary>컬링 체크 거리</summary>
    private static float m_cullingCheckDistance = 1.05f;


    // 모든 타일 관련 변수
    /// <summary>모든 타일 메이커</summary>
    private TileMaker[] m_tileMakers;
    /// <summary>모든 타일 메이커 개수</summary>
    private int m_tileMakersCount;


    // 모든 타일 이외 관련 변수
    /// <summary>타일이 아닌 모든 렌더러 모음</summary>
    private List<MeshRenderer> m_noTileMeshRenderers;
    /// <summary>타일이 아닌 모든 렌더러 개수</summary>
    private int m_noTileMeshRendereCount;


    // 선택된 타일 메이커 관련 변수
    /// <summary>선택된 타일 메이커 모음</summary>
    private List<TileMaker> m_selectTileMakers;
    /// <summary>선택된 타일 메이커 개수</summary>
    private int m_selectTileMakerCount;
    /// <summary>선택된 타일 메이커 정적 오브젝트 모음</summary>
    private List<SerializedObject> m_selectTileMakerSerializedObj;
    /// <summary>선택된 정적 타일 번호 프로퍼티</summary>
    private List<SerializedProperty> m_selectTileNumberProp;
    /// <summary>이전에 저장된 타일 넘버</summary>
    private int m_oldTilesNumber;
    /// <summary>타일 번호 변수 패스</summary>
    private static string m_tileNumberPropPath = "m_selectSnowGrassNumber";


    // 타일 생성 관련 변수
    private GameObject m_tilePrefab;

    [MenuItem("BlueCube/Tile Maker")]
    public static void ShowWindow()
    {
        GetWindow<TileMakerWindow>("Tile Maker");
    }

    private void OnEnable()
    {
        InitTileMakers();
        InitNoTileRenderers();
        GetSelectTilesInfo();

        m_tilePrefab = Resources.Load("Prefabs/Terrain/Snow/Tile/Terrain_Snow_Tile") as GameObject;
    }

    private void OnSelectionChange()
    {
        GetSelectTilesInfo();
    }

    /// <summary>타일 메이커 초기화</summary>
    private void InitTileMakers()
    {
        m_tileMakers = (TileMaker[])FindObjectsOfType(typeof(TileMaker));
        m_tileMakersCount = m_tileMakers.Length;
    }

    /// <summary>타일이 아닌 랜더러 초기화</summary>
    private void InitNoTileRenderers()
    {
        // 초기화
        m_noTileMeshRenderers = new List<MeshRenderer>();

        // 모든 메쉬 랜더러를 가져옴
        MeshRenderer[] allMeshRenderes = (MeshRenderer[])FindObjectsOfType(typeof(MeshRenderer));
        int allMeshRendererCount = allMeshRenderes.Length;

        for(int i = 0; i < allMeshRendererCount; i++)
        {
            // 자식으로 타일인지 찾음
            TileMaker tileMaker = allMeshRenderes[i].GetComponentInChildren<TileMaker>();

            // 부모로 타일인지 찾음
            if(tileMaker == null)
            {
                tileMaker = allMeshRenderes[i].GetComponentInParent<TileMaker>();

                // 자식 부모 모두 타일이 아니면 저장
                if(tileMaker == null)
                {
                    m_noTileMeshRenderers.Add(allMeshRenderes[i]);
                }
            }
        }

        // 개수 저장
        m_noTileMeshRendereCount = m_noTileMeshRenderers.Count;
    }

    /// <summary>선택한 타일들 정보를 가져옴</summary>
    private void GetSelectTilesInfo()
    {
        Transform[] selectObjects = Selection.transforms;
        int selectObjectCount = selectObjects.Length;

        // 선택된 오브젝트가 1개 이상일 경우에만 실행
        if(!selectObjectCount.Equals(0))
        {
            // 각 리스트 초기화
            m_selectTileMakers = new List<TileMaker>();
            m_selectTileMakerSerializedObj = new List<SerializedObject>();
            m_selectTileNumberProp = new List<SerializedProperty>();

            // 선택된 모든 오브젝트를 돌면서 체크
            for(int i = 0; i < selectObjectCount; i++)
            {
                TileMaker selectTileMaker = selectObjects[i].GetComponent<TileMaker>();

                // 선택된 오브젝트에 타일 메이커 컴포넌트가 있을 경우에만 실행
                if(selectTileMaker != null)
                {
                    // 정적 오브젝트 및 변수를 생성 및 가져옴
                    SerializedObject selectTileMakerSerializedObj = new SerializedObject(selectTileMaker);
                    SerializedProperty selectTileNumberProp = selectTileMakerSerializedObj.FindProperty(m_tileNumberPropPath);

                    // 각 정보를 저장
                    m_selectTileMakers.Add(selectTileMaker);
                    m_selectTileMakerSerializedObj.Add(selectTileMakerSerializedObj);
                    m_selectTileNumberProp.Add(selectTileNumberProp);
                }
            }

            // 개수 저장
            m_selectTileMakerCount = m_selectTileMakers.Count;

            // 하나라도 선택된 타일 메이커가 있을 경우 실행
            if(!m_selectTileMakerCount.Equals(0))
            {
                // 첫 번째에 있는 타일 넘버를 기존 타일 넘버라고 저장
                m_oldTilesNumber = m_selectTileNumberProp[0].intValue;

                // 선택된 타일이 한 개 이상일 경우에만 실행
                if (m_selectTileMakerCount > 1)
                {
                    // 첫 번째를 제외한 타일 메이커 개수만큼 실행
                    for (int i = 1; i < m_selectTileMakerCount; i++)
                    {
                        // 첫 번째에 있는 타일 넘버랑 다를 경우 실행
                        if (!m_oldTilesNumber.Equals(m_selectTileNumberProp[i].intValue))
                        {
                            // 선택된 모든 타일 메이커들의 타일 넘버가 같지 않다고 설정
                            m_oldTilesNumber = -1;
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            m_selectTileMakerCount = 0;
        }
    }

    /// <summary>실제로 그려주는 부분</summary>
    private void OnGUI()
    {
        GUILayout.Label("모든 타일 설정", EditorStyles.boldLabel);

        AllTileInitModel();
        AllTileRendererEnableOn();
        AllTileRendererEnableOff();
        AllTileRendererCulling();

        GUILayout.Space(10f);
        GUILayout.Label("기타 설정", EditorStyles.boldLabel);
        AllNoTileRendererEnableOn();
        AllNoTileRendererEnableOff();

        CreateTileLayout();

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
            for (int i = 0; i < m_tileMakersCount; i++)
                m_tileMakers[i].GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }

    /// <summary>모든 타일 오브젝트 렌더러 끄기</summary>
    private void AllTileRendererEnableOff()
    {
        if (GUILayout.Button("모든 타일 오브젝트 렌더러 끄기"))
        {
            for (int i = 0; i < m_tileMakersCount; i++)
                m_tileMakers[i].GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    /// <summary>모든 타일 오브젝트 컬링</summary>
    private void AllTileRendererCulling()
    {
        if (GUILayout.Button("모든 타일 오브젝트 컬링"))
        {
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

    /// <summary>타일이 아닌 모든 오브젝트 렌더러 켜기</summary>
    private void AllNoTileRendererEnableOn()
    {
        if (GUILayout.Button("타일이 아닌 모든 오브젝트 렌더러 켜기"))
        {
            for (int i = 0; i < m_noTileMeshRendereCount; i++)
            {
                m_noTileMeshRenderers[i].enabled = true;
            }
        }
    }

    /// <summary>타일이 아닌 모든 오브젝트 렌더러 끄기</summary>
    private void AllNoTileRendererEnableOff()
    {
        if (GUILayout.Button("타일이 아닌 모든 오브젝트 렌더러 끄기"))
        {
            for (int i = 0; i < m_noTileMeshRendereCount; i++)
            {
                m_noTileMeshRenderers[i].enabled = false;
            }
        }
    }

    /// <summary>선택 타일 메쉬 변경</summary>
    private void SelectTileMeshChange()
    {
        // 하나라도 선택된 타일 메이커가 있을 경우에만 실행
        if(!m_selectTileMakerCount.Equals(0))
        {
            GUILayout.Space(10f);
            EditorGUILayout.LabelField("선택한 타일 설정 (0 ~ " + (TileMaker.SnowGrassCount - 1).ToString() + ")", EditorStyles.boldLabel);

            int newTileNumber = EditorGUILayout.IntField("Tiles Number", m_oldTilesNumber);

            // 기존 타일 넘버랑 새로 입력된 타일 넘버가 다르고, 타일 넘버가 존재하는 타일일 경우에만 실행
            if (!newTileNumber.Equals(m_oldTilesNumber) && (newTileNumber >= 0 && newTileNumber < TileMaker.SnowGrassCount))
            {
                for (int i = 0; i < m_selectTileMakerCount; i++)
                {
                    // 업데이트
                    m_selectTileMakerSerializedObj[i].Update();

                    // 변수 저장
                    m_selectTileNumberProp[i].intValue = newTileNumber;
                    m_selectTileMakers[i].SetMesh(newTileNumber);

                    // 저장
                    m_selectTileMakerSerializedObj[i].ApplyModifiedProperties();
                }

                m_oldTilesNumber = newTileNumber;
            }
        }
    }

    /// <summary>타일 생성 레이아웃</summary>
    private void CreateTileLayout()
    {
        // 선택된 타일 메이커가 하나라도 있을 경우에만 실행
        if(!m_selectTileMakerCount.Equals(0))
        {
            GUILayout.Space(10f);
            GUILayout.Label("타일 생성");

            int screenWidth = (int)position.width;

            GUIStyle windowCenterButtonStyle = new GUIStyle(GUI.skin.button);
            windowCenterButtonStyle.margin = new RectOffset(screenWidth / 4, screenWidth / 4, 10, 10);
            
            if(GUILayout.Button("Forward", windowCenterButtonStyle))
                CreateTile(Vector3.forward);

            if (GUILayout.Button("Up", windowCenterButtonStyle))
                CreateTile(Vector3.up);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Left"))
                CreateTile(Vector3.left);


            if (GUILayout.Button("Right"))
                CreateTile(Vector3.right);

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Down", windowCenterButtonStyle))
                CreateTile(Vector3.down);

            if (GUILayout.Button("Back", windowCenterButtonStyle))
                CreateTile(Vector3.back);
        }
    }

    /// <summary>direction방향에 타일 생성</summary>
    private void CreateTile(Vector3 direction)
    {
        Transform world = GameObject.Find("World").transform;
        float moveDistance = 2f;

        List<GameObject> newTiles = new List<GameObject>();

        for(int i = 0; i < m_selectTileMakerCount; i++)
        {
            GameObject newTile = Instantiate(m_tilePrefab);
            newTile.transform.parent = world;
            newTile.transform.position = m_selectTileMakers[i].transform.position + direction * moveDistance;
            newTile.GetComponent<TileMaker>().SetMesh(0);
            newTiles.Add(newTile);
            Undo.RegisterCreatedObjectUndo(newTile, "Create Object");
        }

        Selection.objects = (Object[])newTiles.ToArray();
    }
}