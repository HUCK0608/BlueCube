﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TutorialStarter3D : MonoBehaviour
{
    [SerializeField]
    private Tutorial m_tutorial;

    [SerializeField]
    private TutorialStarter2D m_starter2D;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            m_tutorial.StartTutorial();

            if (m_starter2D != null)
                m_starter2D.gameObject.SetActive(false);

            gameObject.SetActive(false);
        }
    }
}
