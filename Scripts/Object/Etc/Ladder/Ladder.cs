using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Ladder : MonoBehaviour
{
    [SerializeField]
    private Transform m_hangPosition;

    /// <summary>사다리의 매달릴 좌표를 반환함</summary>
    public Vector3 HangPosition { get { return m_hangPosition.position; } }
    /// <summary>사다리의 정면 방향을 반환함</summary>
    public Vector3 Forward { get { return transform.forward; } }
}
