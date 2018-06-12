using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SceneLoader2D : MonoBehaviour
{
    private SceneLoader m_sceneLoader;

    private void Awake()
    {
        m_sceneLoader = GetComponentInParent<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals(GameLibrary.String_Player))
        {
            m_sceneLoader.LoadScene();
        }
    }
}
