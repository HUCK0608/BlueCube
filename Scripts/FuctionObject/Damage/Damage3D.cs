using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage3D : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {
        if(other.transform.tag == "Player")
        {
            PlayerManager.Instance.Hit(1);
        }
    }
}
