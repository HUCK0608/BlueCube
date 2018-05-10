using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovePanel2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            // 플레이어 고정
            PlayerManager.Instance.transform.parent = transform.parent;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            // 플레이어 고정 해제
            PlayerManager.Instance.transform.parent = null;
        }
    }
}
