using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RotationYPanel3D : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            PlayerManager.Instance.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            PlayerManager.Instance.transform.parent = null;
        }
    }
}
