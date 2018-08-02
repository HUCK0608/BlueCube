using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointManager : MonoBehaviour
{
    private string m_savePath;

    private void Awake()
    {
        m_savePath = "CheckPoint_" + SceneManager.GetActiveScene().name;
    }
}
