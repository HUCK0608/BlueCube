using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStarter3D : MonoBehaviour
{
    [SerializeField]
    private Tutorial m_tutorial;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            m_tutorial.StartTutorial();
            gameObject.SetActive(false);
        }
    }
}
