﻿using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class GameObjectExtension
{
    public static void CustomSetActive(this GameObject obj, bool bActive)
    {
        if ((obj != null) && (obj.get_activeSelf() != bActive))
        {
            obj.SetActive(bActive);
        }
    }
}

