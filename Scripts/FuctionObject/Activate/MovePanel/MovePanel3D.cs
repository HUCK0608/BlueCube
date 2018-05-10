using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovePanel3D : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer.Shift().Equals(GameLibrary.LayerMask_InteractionPickPut))
        {
            other.transform.parent.parent = transform.parent;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer.Shift().Equals(GameLibrary.LayerMask_InteractionPickPut))
        {
            other.transform.parent.SetParent(WorldManager.Instance.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameLibrary.String_Player))
        {
            // 플레이어 고정
            PlayerManager.Instance.transform.parent = transform.parent;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            // 플레이어 고정 해제
            PlayerManager.Instance.transform.parent = null;
        }
    }
}
