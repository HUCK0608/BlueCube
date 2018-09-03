using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TutorialStarter2D : MonoBehaviour
{
    [SerializeField]
    private Tutorial m_tutorial;

    [SerializeField]
    private TutorialStarter3D m_starter3D;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            m_tutorial.StartTutorial();

            if (m_starter3D != null)
                m_starter3D.gameObject.SetActive(false);

            gameObject.SetActive(false);
        }
    }
}
