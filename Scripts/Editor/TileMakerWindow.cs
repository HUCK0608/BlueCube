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
    private List<TileMaker> m_tileMakers;
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
        GetSelectTilesInfo();

        m_tilePrefab = Resources.Load("Prefabs/Terrain/Snow/Tile/Terrain_Snow_Tile") as GameObject;

        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;

        SetEditorMode();
    }

    private void OnSelectionChange()
    {
        GetSelectTilesInfo();
    }

    /// <summary>타일 메이커 초기화</summary>
    private void InitTileMakers()
    {
        m_tileMakers = new List<TileMaker>();
        m_tileMakers.AddRange((TileMaker[])FindObjectsOfType(typeof(TileMaker)));
        m_tileMakersCount = m_tileMakers.Count;
    }

    /// <summary>타일이 아닌 랜더러 초기화</summary>
    private void InitNoTileRenderers()
    {
        // 초기화
        m_noTileMeshRenderers = new List<MeshRenderer>();

        // 모든 메쉬 랜더러를 가져옴
        MeshRenderer[] allMeshRenderes = (MeshRenderer[])FindObjectsOfType(typeof(MeshRenderer));
        int allMeshRendererCount = allMeshRenderes.Length;

        for (int i = 0; i < allMeshRendererCount; i++)
        {
            // 자식으로 타일인지 찾음
            TileMaker tileMaker = allMeshRenderes[i].GetComponentInChildren<TileMaker>();

            // 부모로 타일인지 찾음
            if (tileMaker == null)
            {
                tileMaker = allMeshRenderes[i].GetComponentInParent<TileMaker>();

                // 자식 부모 모두 타일이 아니면 저장
                if (tileMaker == null)
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
        if (!selectObjectCount.Equals(0))
        {
            // 각 리스트 초기화
            m_selectTileMakers = new List<TileMaker>();
            m_selectTileMakerSerializedObj = new List<SerializedObject>();
            m_selectTileNumberProp = new List<SerializedProperty>();

            // 선택된 모든 오브젝트를 돌면서 체크
            for (int i = 0; i < selectObjectCount; i++)
            {
                TileMaker selectTileMaker = selectObjects[i].GetComponent<TileMaker>();

                // 선택된 오브젝트에 타일 메이커 컴포넌트가 있을 경우에만 실행
                if (selectTileMaker != null)
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
            if (!m_selectTileMakerCount.Equals(0))
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

    /// <summary>타일이 아닌 모든 오브젝트 렌더러 켜기</summary>
    private void AllNoTileRendererEnableOn()
    {
        if (GUILayout.Button("타일이 아닌 모든 오브젝트 렌더러 켜기"))
        {
            InitNoTileRenderers();

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
            InitNoTileRenderers();

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
        if (!m_selectTileMakerCount.Equals(0))
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
        if (!m_selectTileMakerCount.Equals(0))
        {
            GUILayout.Space(10f);
            GUILayout.Label("타일 생성");

            int screenWidth = (int)position.width;

            GUIStyle windowCenterButtonStyle = new GUIStyle(GUI.skin.button);
            windowCenterButtonStyle.margin = new RectOffset(screenWidth / 4, screenWidth / 4, 10, 10);

            if (GUILayout.Button("Forward", windowCenterButtonStyle))
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

        for (int i = 0; i < m_selectTileMakerCount; i++)
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

    ///////////////////////////////////////////// SceneView GUI /////////////////////////////////////////////

    /// <summary>생성할 수 없는 위치</summary>
    private static Vector3 m_notCreatePosition = new Vector3(9999f, 9999f, 9999f);

    /// <summary>생성될 위치가 그려지는 타일</summary>
    private GameObject m_drawTile;
    /// <summary>생성 모드 활성화 여부</summary>
    private bool m_isOnCreateMode;
    /// <summary>선택 모드 활성화 여부</summary>
    private bool m_isOnSelectMode;

    /// <summary>임시 생성 노말</summary>
    private Vector3 m_tempCreateNormal;
    /// <summary>임시 생성 타일 모음</summary>
    private List<GameObject> m_tempCreateTiles;

    /// <summary>임시 선택 타일 모음</summary>
    private List<GameObject> m_tempSelectTiles;

    /// <summary>처음 위치</summary>
    private Vector3 m_firstPosition;
    /// <summary>처음 노말</summary>
    private Vector3 m_firstNormal;
    /// <summary>처음 플레인</summary>
    private Plane m_firstPlane;

    /// <summary>LeftMouse 드래그 중일경우 true를 반환</summary>
    private bool m_isOnLeftMouseDrag;

    /// <summary>씬 뷰에 그려질 GUI</summary>
    private void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();
        DrawCreateModeButton();
        DrawDeleteModeButton();
        Handles.EndGUI();

        CreateModeLogic();

        RunEvent();
    }

    /// <summary>생성 모드 버튼 그리기</summary>
    private void DrawCreateModeButton()
    {
        if (!m_isOnCreateMode)
        {
            if (GUILayout.Button("타일 생성 모드 시작 (F1)", GUILayout.Width(140f), GUILayout.Height(30f)))
                OnCreateMode();
        }
        else
        {
            Color oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;

            if (GUILayout.Button("타일 생성 모드 종료 (F1)", GUILayout.Width(140f), GUILayout.Height(30f)))
                OffCreateMode();

            GUI.backgroundColor = oldColor;
        }
    }

    /// <summary>선택 모드 버튼 그리기</summary>
    private void DrawDeleteModeButton()
    {
        if (!m_isOnSelectMode)
        {
            if (GUILayout.Button("타일 선택 모드 시작 (F2)", GUILayout.Width(140f), GUILayout.Height(30f)))
                OnSelectMode();
        }
        else
        {
            Color oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;

            if (GUILayout.Button("타일 선택 모드 종료 (F2)", GUILayout.Width(140f), GUILayout.Height(30f)))
                OffSelectMode();

            GUI.backgroundColor = oldColor;
        }
    }

    /// <summary>생성모드 켜기</summary>
    private void OnCreateMode()
    {
        if (m_isOnSelectMode)
            OffSelectMode();

        m_isOnCreateMode = true;

        m_drawTile = Instantiate(m_tilePrefab) as GameObject;
        m_drawTile.name = "(TileMaker)TempTile";
        m_drawTile.GetComponentInChildren<Collider>().enabled = false;
    }

    /// <summary>생성모드 끄기</summary>
    private void OffCreateMode()
    {
        m_isOnCreateMode = false;

        DestroyImmediate(m_drawTile);

        GameObject drawTile = GameObject.Find("(TileMaker)TempTile");
        if (drawTile != null)
            DestroyImmediate(drawTile);

        if (m_isOnLeftMouseDrag)
        {
            int tempCreateTileCount = m_tempCreateTiles.Count;

            for (int i = 0; i < tempCreateTileCount; i++)
                DestroyImmediate(m_tempCreateTiles[i]);

            m_tempCreateTiles.Clear();

            m_isOnLeftMouseDrag = false;
        }
    }

    /// <summary>선택모드 켜기</summary>
    private void OnSelectMode()
    {
        if (m_isOnCreateMode)
            OffCreateMode();

        m_isOnSelectMode = true;
    }

    /// <summary>선택모드 끄기</summary>
    private void OffSelectMode()
    {
        m_isOnSelectMode = false;
    }

    /// <summary>에디터 모드로 설정</summary>
    private void SetEditorMode()
    {
        if (m_isOnCreateMode)
            OffCreateMode();
        if (m_isOnSelectMode)
            OffSelectMode();
    }

    /// <summary>생성모드 로직</summary>
    private void CreateModeLogic()
    {
        RemoveSelection();
        DrawCreatePointVisual();
    }

    /// <summary>아무런 선택도 안 되게 설정</summary>
    private void RemoveSelection()
    {
        if (m_isOnCreateMode)
            Selection.activeTransform = null;
    }

    /// <summary>생성될 위치를 시각화하여 그림</summary>
    private void DrawCreatePointVisual()
    {
        // 생성 모드가 아닐경우 리턴
        if (!m_isOnCreateMode)
            return;

        // 마우스 드래그가 활성화 되었을 때 시각화를 끄고 리턴
        if (m_isOnLeftMouseDrag)
        {
            if (m_drawTile.activeSelf)
                m_drawTile.SetActive(false);
            return;
        }

        // 시각화가 꺼져있다면 킴
        if (!m_drawTile.activeSelf)
            m_drawTile.SetActive(true);

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        int layermask = GameLibrary.LayerMask_Tile;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            m_drawTile.transform.position = hit.transform.position + hit.normal * 2f;
            m_tempCreateNormal = hit.normal;
        }
        else
            m_drawTile.transform.position = m_notCreatePosition;
    }

    /// <summary>이벤트 실행</summary>
    private void RunEvent()
    {
        Event currentEvent = Event.current;

        if (currentEvent.isKey)
        {
            if (currentEvent.type == EventType.KeyUp)
            {
                switch (currentEvent.keyCode)
                {
                    case KeyCode.F1:
                        Event_F1KeyDown();
                        break;
                    case KeyCode.F2:
                        Event_F2KeyDown();
                        break;
                    case KeyCode.Escape:
                        Event_EscapeKeyDown();
                        break;
                }
            }
        }
        else if (currentEvent.isMouse)
        {
            // 생성모드 또는 선택모드에서만 실행
            if (m_isOnCreateMode || m_isOnSelectMode)
            {
                // 마우스 왼쪽 버튼일 경우에만 실행
                if (currentEvent.button.Equals(0))
                {
                    int controlID = GUIUtility.GetControlID(FocusType.Passive);

                    switch (currentEvent.GetTypeForControl(controlID))
                    {
                        case EventType.MouseDown:
                            Event_LeftMouseDown();

                            GUIUtility.hotControl = controlID;
                            currentEvent.Use();
                            break;
                        case EventType.MouseDrag:
                            Event_LeftMouseDrag();

                            GUIUtility.hotControl = controlID;
                            currentEvent.Use();
                            break;
                        case EventType.MouseUp:
                            Event_LeftMouseUp();

                            GUIUtility.hotControl = 0;
                            currentEvent.Use();
                            break;
                    }
                }
            }
        }
    }

    /// <summary>C KeyDown 이벤트</summary>
    private void Event_F1KeyDown()
    {
        if (m_isOnCreateMode)
            OffCreateMode();
        else
            OnCreateMode();
    }

    /// <summary>S KeyDown 이벤트</summary>
    private void Event_F2KeyDown()
    {
        if (m_isOnSelectMode)
            OffSelectMode();
        else
            OnSelectMode();
    }

    /// <summary>Escape KeyDown 이벤트</summary>
    private void Event_EscapeKeyDown()
    {
        SetEditorMode();
    }

    /// <summary>LeftMouseDown 이벤트</summary>
    private void Event_LeftMouseDown()
    {
        Event_CreateMode_LeftMouseDown();
        Event_SelectMode_LeftMouseDown();
    }

    /// <summary>LeftMouseDrag 이벤트</summary>
    private void Event_LeftMouseDrag()
    {
        m_isOnLeftMouseDrag = true;

        Event_CreateMode_LeftMouseDrag();
        Event_SelectMode_LeftMouseDrag();
    }

    /// <summary>LeftMouseUp 이벤트</summary>
    private void Event_LeftMouseUp()
    {
        if (m_isOnLeftMouseDrag)
            m_isOnLeftMouseDrag = false;

        Event_CreateMode_LeftMouseUp();
    }

    #region CreateMode Event 목록

    /// <summary>생성모드 LeftMouseDown 이벤트</summary>
    private void Event_CreateMode_LeftMouseDown()
    {
        if (!m_isOnCreateMode)
            return;

        // 놓을 수 없는 위치일 경우 리턴
        if (m_drawTile.transform.position.Equals(m_notCreatePosition))
            return;

        // 타일 생성
        GameObject newTile = PrefabUtility.InstantiatePrefab(m_tilePrefab) as GameObject;
        newTile.transform.position = m_drawTile.transform.position;
        newTile.transform.parent = GameObject.Find("World").transform;
        Undo.RegisterCreatedObjectUndo(newTile, "Object Create");

        // 처음 생성 위치와 노말 저장
        m_firstPosition = m_drawTile.transform.position;
        m_firstNormal = m_tempCreateNormal;
        m_firstPlane = new Plane(m_firstNormal, m_firstPosition);

        m_drawTile.SetActive(false);
    }

    /// <summary>생성모드 LeftMouseDrag 이벤트</summary>
    private void Event_CreateMode_LeftMouseDrag()
    {
        if (!m_isOnCreateMode)
            return;

        if (m_tempCreateTiles == null)
            m_tempCreateTiles = new List<GameObject>();
        else
        {
            int tempCreateTileCount = m_tempCreateTiles.Count;

            for (int i = 0; i < tempCreateTileCount; i++)
                DestroyImmediate(m_tempCreateTiles[i]);

            m_tempCreateTiles.Clear();
        }

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float enter;
        m_firstPlane.Raycast(ray, out enter);

        Vector3 mouseRayHitPivot = ray.GetPoint(enter).GetGamePivot();

        float roundFirstNormalX = Mathf.Round(m_firstNormal.x);
        float roundFirstNormalY = Mathf.Round(m_firstNormal.y);
        float roundFirstNormalZ = Mathf.Round(m_firstNormal.z);

        float zero = 0f;
        float one = 1f;
        float two = 2f;
        int intZero = 0;
        int intOne = 1;
        int intTwo = 2;

        int pivotCount1 = 0;
        int pivotCount2 = 0;

        if (roundFirstNormalX.Equals(zero) && roundFirstNormalY.Equals(zero))
        {
            pivotCount1 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.x) - Mathf.RoundToInt(mouseRayHitPivot.x)) / intTwo;
            pivotCount2 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.y) - Mathf.RoundToInt(mouseRayHitPivot.y)) / intTwo;
        }
        else if (roundFirstNormalX.Equals(zero) && roundFirstNormalZ.Equals(zero))
        {
            pivotCount1 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.x) - Mathf.RoundToInt(mouseRayHitPivot.x)) / intTwo;
            pivotCount2 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.z) - Mathf.RoundToInt(mouseRayHitPivot.z)) / intTwo;
        }
        else
        {
            pivotCount1 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.y) - Mathf.RoundToInt(mouseRayHitPivot.y)) / intTwo;
            pivotCount2 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.z) - Mathf.RoundToInt(mouseRayHitPivot.z)) / intTwo;
        }

        // 각 피벗의 개수가 0개 이상일 경우에만 실행
        if (!(pivotCount1.Equals(zero) && pivotCount2.Equals(zero)))
        {
            Vector3 directionToMousePosition = mouseRayHitPivot - m_firstPosition;
            Vector3 direction1 = Vector3.zero;
            Vector3 direction2 = Vector3.zero;

            if (roundFirstNormalX.Equals(zero) && roundFirstNormalY.Equals(zero))
            {
                direction1.x = directionToMousePosition.x;
                direction2.y = directionToMousePosition.y;
            }
            else if (roundFirstNormalX.Equals(zero) && roundFirstNormalZ.Equals(zero))
            {
                direction1.x = directionToMousePosition.x;
                direction2.z = directionToMousePosition.z;
            }
            else
            {
                direction1.y = directionToMousePosition.y;
                direction2.z = directionToMousePosition.z;
            }

            direction1 = direction1.normalized;
            direction2 = direction2.normalized;

            Transform world = GameObject.Find("World").transform;
            RaycastHit hit;
            int layerMask = GameLibrary.LayerMask_Tile;

            for (int i = 0; i <= pivotCount1; i++)
            {
                for (int j = 0; j <= pivotCount2; j++)
                {
                    // 처음 생성 위치를 제외하고 실행
                    if (!(i.Equals(0) && j.Equals(0)))
                    {
                        Vector3 pivot = m_firstPosition + direction1 * two * i + direction2 * two * j;
                        if (Physics.Raycast(pivot, -m_firstNormal, out hit, Mathf.Infinity, layerMask))
                        {
                            Vector3 hitPivot = hit.transform.position + hit.normal * two;

                            int pivotCount3 = intZero;

                            if (roundFirstNormalX.Equals(one))
                                pivotCount3 = Mathf.Abs(Mathf.RoundToInt(hitPivot.x) - Mathf.RoundToInt(pivot.x)) / 2;
                            else if (roundFirstNormalY.Equals(one))
                                pivotCount3 = Mathf.Abs(Mathf.RoundToInt(hitPivot.y) - Mathf.RoundToInt(pivot.y)) / 2;
                            else
                                pivotCount3 = Mathf.Abs(Mathf.RoundToInt(hitPivot.z) - Mathf.RoundToInt(pivot.z)) / 2;

                            for (int k = 0; k <= pivotCount3; k++)
                            {
                                Vector3 createPivot = hitPivot - (-m_firstNormal) * k * two;
                                Vector3 rayCheckOrigin = createPivot + m_firstNormal * (k + intOne) * two;

                                // 해당 위치에 충돌하는 타일이 없을 경우에만 타일 생성
                                if (!Physics.Raycast(rayCheckOrigin, -m_firstNormal, two, layerMask))
                                {
                                    GameObject newTile = PrefabUtility.InstantiatePrefab(m_tilePrefab) as GameObject;
                                    newTile.transform.position = createPivot;
                                    newTile.transform.parent = world;

                                    m_tempCreateTiles.Add(newTile);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>생성모드 LeftMouseUp 이벤트</summary>
    private void Event_CreateMode_LeftMouseUp()
    {
        if (!m_isOnCreateMode)
            return;

        int tempCreateTileCount = m_tempCreateTiles.Count;
        for (int i = 0; i < tempCreateTileCount; i++)
            Undo.RegisterCreatedObjectUndo(m_tempCreateTiles[i], "Create Tile");

        m_tempCreateTiles.Clear();

        m_drawTile.SetActive(true);
    }
    #endregion

    #region SelectMode Event 목록
    /// <summary>선택모드 LeftMouseDown 이벤트</summary>
    private void Event_SelectMode_LeftMouseDown()
    {
        if (!m_isOnSelectMode)
            return;

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        int layermask = GameLibrary.LayerMask_Tile;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            if (m_tempSelectTiles == null)
                m_tempSelectTiles = new List<GameObject>();
            else
                m_tempSelectTiles.Clear();

            m_tempSelectTiles.Add(hit.transform.parent.gameObject);
            m_firstPosition = hit.transform.position;
            m_firstNormal = hit.normal;
            m_firstPlane = new Plane(m_firstNormal, m_firstPosition);
        }
        else
        {
            if(m_tempSelectTiles != null)
                m_tempSelectTiles.Clear();

            Selection.activeTransform = null;
        }
    }

    /// <summary>선택모드 LeftMouseDrag 이벤트</summary>
    private void Event_SelectMode_LeftMouseDrag()
    {
        if (!m_isOnSelectMode)
            return;

        if (m_tempSelectTiles == null || m_tempSelectTiles.Count.Equals(0))
            return;

        m_tempSelectTiles.Clear();

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float enter;
        m_firstPlane.Raycast(ray, out enter);

        Vector3 mouseRayHitPivot = ray.GetPoint(enter).GetGamePivot();

        float roundFirstNormalX = Mathf.Round(m_firstNormal.x);
        float roundFirstNormalY = Mathf.Round(m_firstNormal.y);
        float roundFirstNormalZ = Mathf.Round(m_firstNormal.z);

        float zero = 0f;
        float two = 2f;
        int intTwo = 2;

        int pivotCount1 = 0;
        int pivotCount2 = 0;
        
        if (roundFirstNormalX.Equals(zero) && roundFirstNormalY.Equals(zero))
        {
            pivotCount1 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.x) - Mathf.RoundToInt(mouseRayHitPivot.x)) / intTwo;
            pivotCount2 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.y) - Mathf.RoundToInt(mouseRayHitPivot.y)) / intTwo;
        }
        else if (roundFirstNormalX.Equals(zero) && roundFirstNormalZ.Equals(zero))
        {
            pivotCount1 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.x) - Mathf.RoundToInt(mouseRayHitPivot.x)) / intTwo;
            pivotCount2 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.z) - Mathf.RoundToInt(mouseRayHitPivot.z)) / intTwo;
        }
        else
        {
            pivotCount1 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.y) - Mathf.RoundToInt(mouseRayHitPivot.y)) / intTwo;
            pivotCount2 = Mathf.Abs(Mathf.RoundToInt(m_firstPosition.z) - Mathf.RoundToInt(mouseRayHitPivot.z)) / intTwo;
        }

        Vector3 directionToMousePosition = mouseRayHitPivot - m_firstPosition;
        Vector3 direction1 = Vector3.zero;
        Vector3 direction2 = Vector3.zero;

        if (roundFirstNormalX.Equals(zero) && roundFirstNormalY.Equals(zero))
        {
            direction1.x = directionToMousePosition.x;
            direction2.y = directionToMousePosition.y;
        }
        else if (roundFirstNormalX.Equals(zero) && roundFirstNormalZ.Equals(zero))
        {
            direction1.x = directionToMousePosition.x;
            direction2.z = directionToMousePosition.z;
        }
        else
        {
            direction1.y = directionToMousePosition.y;
            direction2.z = directionToMousePosition.z;
        }

        direction1 = direction1.normalized;
        direction2 = direction2.normalized;

        RaycastHit hit;
        int layerMask = GameLibrary.LayerMask_Tile;

        for (int i = 0; i <= pivotCount1; i++)
        {
            for (int j = 0; j <= pivotCount2; j++)
            {
                Vector3 checkOrigin = m_firstPosition + direction1 * two * i + direction2 * two * j + m_firstNormal * two;
                if (Physics.Raycast(checkOrigin, -m_firstNormal, out hit, Mathf.Infinity, layerMask))
                {
                    GameObject oldTile = hit.transform.parent.gameObject;
                    checkOrigin = hit.transform.position;

                    while(true)
                    {
                        if (Physics.Raycast(checkOrigin, m_firstNormal, out hit, Mathf.Infinity, layerMask))
                        {
                            oldTile = hit.transform.parent.gameObject;
                            checkOrigin = hit.transform.position;
                        }
                        else
                        {
                            m_tempSelectTiles.Add(oldTile);
                            break;
                        }
                    }
                }
            }
        }

        Selection.objects = m_tempSelectTiles.ToArray();
    }
    #endregion
}