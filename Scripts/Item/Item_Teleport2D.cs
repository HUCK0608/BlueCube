using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_Teleport2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            Vector3 teleportPosition = transform.parent.position;
            teleportPosition.x = other.transform.position.x;
            other.transform.position = teleportPosition;
            transform.parent.gameObject.SetActive(false);
        }
    }
}
