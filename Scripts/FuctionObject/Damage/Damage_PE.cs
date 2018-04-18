using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Damage_PE : Damage
{
    public override void DoHit(GameObject collision)
    {
        // 플레이어일 경우
        if(collision.tag.Equals(GameLibrary.String_Player))
        {
            // 이미 데미지를 입힌 경우 리턴
            if (!m_hitList.IndexOf(PlayerManager.Instance.gameObject).Equals(-1))
                return;

            // 데미지를 입힘
            PlayerManager.Instance.Hit(1);

            // 히트 리스트에 추가
            m_hitList.Add(PlayerManager.Instance.gameObject);
        }
        // 적일 경우
        else if(collision.tag.Equals(GameLibrary.String_Enemy))
        {
            // 이미 데미지를 입힌 경우 리턴
            if (!m_hitList.IndexOf(collision).Equals(-1))
                return;

            // 데미지를 입힘
            collision.GetComponent<EnemyStat>().Hit(1);

            // 히트 리스트에 추가
            m_hitList.Add(collision);
        }
    }
}
