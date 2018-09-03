using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string m_loadScenePath;

    private bool m_isOnLoad;

    public void LoadScene()
    {
        if (!m_isOnLoad)
        {
            m_isOnLoad = true;
            StartCoroutine(LoadSceneLogic());
        }
    }

    private IEnumerator LoadSceneLogic()
    {
        LoadingUI.Instance.PlayFadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(m_loadScenePath);
    }
}
