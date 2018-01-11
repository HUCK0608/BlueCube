using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Item_Push3D : MonoBehaviour
{
    private void OnCollisionExit(Collision other)
    {
        if(other.transform.tag == "Player")
        {
            float roundX = Mathf.Round(transform.position.x);
            float roundZ = Mathf.Round(transform.position.z);

            transform.position = new Vector3(roundX, transform.position.y, roundZ);
        }
    }

}
