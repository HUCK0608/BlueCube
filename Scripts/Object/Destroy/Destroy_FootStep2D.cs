using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Destroy_FootStep2D : MonoBehaviour
{
    Destroy_FootStep m_footSteb;

    private void Awake()
    {
        m_footSteb = GetComponentInParent<Destroy_FootStep>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            m_footSteb.TimerOn();
        }
    }
}
