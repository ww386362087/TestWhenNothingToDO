﻿using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using System;
using UnityEngine;

internal class NewbieGuideClickHeroSymbolPage : NewbieGuideBaseScript
{
    protected override void Initialize()
    {
    }

    protected override bool IsDelegateClickEvent()
    {
        return true;
    }

    protected override void Update()
    {
        if (base.isInitialize)
        {
            base.Update();
        }
        else
        {
            CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
            if (form != null)
            {
                Transform transform = form.get_transform().FindChild("Other/Panel_SymbolChange/DropList/Button_Down");
                if (transform != null)
                {
                    GameObject baseGo = transform.get_gameObject();
                    if (baseGo.get_activeInHierarchy())
                    {
                        base.AddHighLightGameObject(baseGo, true, form, true);
                        base.Initialize();
                    }
                }
            }
        }
    }
}

