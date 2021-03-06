﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RotationYPanel3D : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            GameManager.Instance.PlayerManager.transform.parent = transform;
            Debug.Log("Enter Player");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            GameManager.Instance.PlayerManager.transform.parent = null;
            Debug.Log("Enter Player");
        }
    }
}
