using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Interaction_Bomb : MonoBehaviour
{
    private bool m_isPick;
    /// <summary>아이템 들어올리기를 완료했을경우 true를 반환</summary>
    public bool IsPick { get { return m_isPick; } }
}
