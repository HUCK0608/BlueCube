using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckBlock : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_checkPoints;

    private int m_blockCount;
    /// <summary>끼는 오브젝트 개수</summary>
    public int BlockCount { get { return m_blockCount; } }
}
