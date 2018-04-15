using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Interaction_Put_Defalut : Interaction_Put
{
    public override void Put()
    {
    }

    protected override Vector3 GetPutPosition()
    {
        return Vector3.one;
    }
}
