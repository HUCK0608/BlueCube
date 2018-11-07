using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DownOutWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(GameLibrary.String_Player) && !PlayerManager.Instance.IsViewChange)
        {
            PlayerManager.Instance.Skill.ChangeView();
        }
    }
}
