using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapMakerWindow : EditorWindow
{
    /// <summary>MapMaker 스크립트</summary>
    private MapMaker m_mapMaker;

    /// <summary>MapMaker 직렬 오브젝트</summary>
    private SerializedObject m_mapMakerSO;

    [MenuItem("BlueCube/Map Maker")]
    public static void ShowWindow()
    {
        GetWindow<MapMakerWindow>("Map Maker");
    }

    /// <summary>새로 빌드가 되어도 한 번 호출되는 부분</summary>
    private void OnEnable()
    {
        LoadMapMaker();
    }

    /// <summary>인터페이스가 그려지는 부분</summary>
    private void OnGUI()
    {
        m_mapMakerSO.Update();
    }

    ////////////////// On Eanble에 사용하는 함수 //////////////////

    /// <summary>MapMaker 로드</summary>
    private void LoadMapMaker()
    {
        m_mapMaker = Resources.Load("MapMaker/MapMaker") as MapMaker;

        m_mapMakerSO = new SerializedObject(m_mapMaker);
    }

}
