using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Jump : Tutorial
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public override void StartTutorial()
    {
        gameObject.SetActive(true);

        StartCoroutine(EndCheck());
    }

    private IEnumerator EndCheck()
    {
        yield return new WaitUntil(() => PlayerManager.Instance.MainController.CurrentState3D.Equals(E_PlayerState3D.JumpUp));

        EndTutorial();
    }

    protected override void EndTutorial()
    {
        gameObject.SetActive(false);
    }
}
