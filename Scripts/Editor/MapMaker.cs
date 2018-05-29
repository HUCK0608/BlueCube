using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MapMaker", menuName = "MapMaker")]
[System.Serializable]
public class MapMaker : ScriptableObject
{
    [SerializeField]
    private List<GameObject> m_prefabs;
}
