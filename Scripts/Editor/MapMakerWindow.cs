using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapMakerWindow : EditorWindow
{
    ///// <summary>MapMaker</summary>
    //private MapMaker m_mapMaker;
    ///// <summary>MapMaker SerializedObject</summary>
    //private SerializedObject m_mapMakerSerializedObject;
    ///// <summary>Prefabs SerializedProperty</summary>
    //private SerializedProperty m_prefabsSerializedProperty;

    ///// <summary>프리팹 프로퍼티 목록</summary>
    //private List<SerializedProperty> m_prefabProperties;
    ///// <summary>프리팹 개수</summary>
    //private int m_prefabCount;

    ///// <summary>선택한 프리팹</summary>
    //private GameObject m_selectPrefab;
    ///// <summary>그려진 선택된 프리팹</summary>
    //private GameObject m_drawSelectPrefab;

    ///// <summary>놓을 수 없는 위치</summary>
    //private static Vector3 m_notCreatePosition = new Vector3(9999f, 9999f, 9999f);
    //private static float m_thumbnailSize_Window = 70f;
    //private static float m_thumbnailSize_SceneView = 50f;

    ///// <summary>메뉴를 클릭할 때 실행</summary>
    ////[MenuItem("BlueCube/MapMaker")]
    //public static void ShowWindow()
    //{
    //    GetWindow<MapMakerWindow>("MapMaker");
    //}

    //////////////////////////////////////////////////////// Window //////////////////////////////////////////////////////

    ///// <summary>활성화 될 때 한번 실행</summary>
    //private void OnEnable()
    //{
    //    LoadMapMaker();

    //    // 씬 뷰 OnGUI 델리게이트에 여기에 있는 함수를 추가
    //    SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    //}

    ///// <summary>MapMaker 로드</summary>
    //private void LoadMapMaker()
    //{
    //    m_mapMaker = Resources.Load("MapMaker/MapMaker") as MapMaker;
    //    m_mapMakerSerializedObject = new SerializedObject(m_mapMaker);
    //    m_prefabsSerializedProperty = m_mapMakerSerializedObject.FindProperty("m_prefabs");
    //}

    ///// <summary>윈도우 인터페이스가 그려지는 함수</summary>
    //private void OnGUI()
    //{
    //    DrawGUI_Window();
    //}

    ///// <summary>윈도우 업데이트</summary>
    //private void OnInspectorUpdate()
    //{
    //    LoadPrefabs();
    //    Repaint();
    //}

    ///// <summary>프리팹 로드</summary>
    //private void LoadPrefabs()
    //{
    //    m_prefabCount = m_prefabsSerializedProperty.arraySize;

    //    if (m_prefabProperties == null)
    //        m_prefabProperties = new List<SerializedProperty>();
    //    else
    //        m_prefabProperties.Clear();

    //    for (int i = 0; i < m_prefabCount; i++)
    //        m_prefabProperties.Add(m_prefabsSerializedProperty.GetArrayElementAtIndex(i));
    //}

    ///// <summary>윈도우에 GUI를 그림</summary>
    //private void DrawGUI_Window()
    //{
    //    DrawPrefabThumbnail_Window();
    //}

    ///// <summary>윈도우에 프리팹 미리보기를 그림</summary>
    //private void DrawPrefabThumbnail_Window()
    //{
    //    for(int i = 0; i < m_prefabCount; i++)
    //    {
    //        GameObject prefab = m_prefabProperties[i].objectReferenceValue as GameObject;
    //        Texture2D thumbnail = prefab.gameObject.GetThumbnail();
    //        GUILayout.Box(thumbnail, GUILayout.Width(m_thumbnailSize_Window), GUILayout.Height(m_thumbnailSize_Window));
    //    }
    //}

    ///// <summary>비활성화 될 때 한번 실행</summary>
    //private void OnDisable()
    //{
    //    ClearSelectPrefab();

    //    // 씬 뷰 OnGUI 델리게이트에 여기에 있는 함수를 제거
    //    SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    //}

    /////////////////////////////////////////////////////// SceneView /////////////////////////////////////////////////////

    ///// <summary>SceneView에 GUI가 그려짐</summary>
    //private void OnSceneGUI(SceneView sceneView)
    //{
    //    Handles.BeginGUI();
    //    DrawGUI_SceneView();
    //    Handles.EndGUI();

    //    RunEvent();
    //    SetCreateMode();
    //}

    ///// <summary>씬 뷰에 GUI를 그림</summary>
    //private void DrawGUI_SceneView()
    //{
    //    DrawPrefabButton_SceneView();
    //    DrawCreatePosition();
    //}

    ///// <summary>씬 뷰에 프리팹 미리보기를 그림</summary>
    //private void DrawPrefabButton_SceneView()
    //{
    //    for(int i = 0; i < m_prefabCount; i++)
    //    {
    //        GameObject prefab = m_prefabProperties[i].objectReferenceValue as GameObject;
    //        Texture2D thumbnail = prefab.gameObject.GetThumbnail();

    //        if (GUILayout.Button(thumbnail, GUILayout.Width(m_thumbnailSize_SceneView), GUILayout.Height(m_thumbnailSize_SceneView)))
    //        {
    //            m_selectPrefab = prefab;
    //            InitSelectPrefab();
    //        }
    //    }
    //}

    ///// <summary>선택된 프리팹을 씬 뷰에 동기화</summary>
    //private void InitSelectPrefab()
    //{
    //    if (m_drawSelectPrefab != null)
    //        DestroyImmediate(m_drawSelectPrefab);

    //    m_drawSelectPrefab = Instantiate(m_selectPrefab) as GameObject;

    //    Collider[] colliders = m_drawSelectPrefab.GetComponentsInChildren<Collider>();

    //    for (int i = 0; i < colliders.Length; i++)
    //        colliders[i].enabled = false;
    //}

    ///// <summary>생성 위치를 그려줌</summary>
    //private void DrawCreatePosition()
    //{
    //    // 선택된 프리팹이 없다면 리턴
    //    if (m_selectPrefab == null)
    //        return;

    //    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
    //    RaycastHit hit;

    //    if(Physics.Raycast(ray, out hit, Mathf.Infinity))
    //        m_drawSelectPrefab.transform.position = hit.point.GetGamePivot();
    //    else
    //        m_drawSelectPrefab.transform.position = m_notCreatePosition;
    //}

    ///// <summary>선택된 프리팹 초기화</summary>
    //private void ClearSelectPrefab()
    //{
    //    if(m_selectPrefab != null)
    //    {
    //        m_selectPrefab = null;
    //        DestroyImmediate(m_drawSelectPrefab);
    //    }
    //}

    ///// <summary>이벤트 실행</summary>
    //private void RunEvent()
    //{
    //    Event currentEvent = Event.current;

    //    if(currentEvent.isKey)
    //    {
    //        if (currentEvent.keyCode == KeyCode.Escape)
    //            Event_Escape();
    //    }
    //    else if(currentEvent.isMouse)
    //    {
    //        if(currentEvent.type.Equals(EventType.MouseDown))
    //        {
    //            // Left
    //            if(currentEvent.button.Equals(0))
    //            {
    //                Event_MouseLeftClick();
    //            }
    //        }
    //    }
    //}

    ///// <summary>Escape 이벤트</summary>
    //private void Event_Escape()
    //{
    //    ClearSelectPrefab();
    //}

    ///// <summary>MouseLeftClick 이벤트</summary>
    //private void Event_MouseLeftClick()
    //{
    //    if (m_selectPrefab == null)
    //        return;
    //    if (m_drawSelectPrefab.transform.position.Equals(m_notCreatePosition))
    //        return;

    //    GameObject newObject = Instantiate(m_selectPrefab) as GameObject;
    //    newObject.transform.position = m_drawSelectPrefab.transform.position;
    //    newObject.transform.parent = GameObject.Find("World").transform;

    //    Undo.RegisterCreatedObjectUndo(newObject, "Create Object");
    //}

    ///// <summary>생성 모드</summary>
    //private void SetCreateMode()
    //{
    //    if (m_selectPrefab == null)
    //        return;

    //    Selection.activeTransform = null;
    //}
}
