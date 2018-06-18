using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerAutoMoveTrigger : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;

    public void ActivateAutoMove()
    {
        PlayerManager.Instance.SubController3D.AutoMoveAndRotate(direction);
        CameraManager.Instance.PlayGanziCam();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(GameLibrary.String_Player))
            ActivateAutoMove();
    }
}
