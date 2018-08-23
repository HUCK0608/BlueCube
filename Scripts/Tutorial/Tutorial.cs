using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    /// <summary>튜토리얼을 시작</summary>
    public abstract void StartTutorial();

    /// <summary>튜토리얼 종료</summary>
    protected abstract void EndTutorial();
}
