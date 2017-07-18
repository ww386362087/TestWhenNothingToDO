﻿using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using System;
using UnityEngine;

public class NewbieGuideClickRewardToHall : NewbieGuideBaseScript
{
    protected override void Initialize()
    {
        CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CTaskSys.TASK_FORM_PATH);
        Transform transform = (form == null) ? null : form.get_transform().FindChild("TopCommon/Button_Close");
        DebugHelper.Assert((form != null) && (transform != null), "taskForm !=null && trans != null");
        GameObject baseGo = (transform == null) ? null : transform.get_gameObject();
        base.AddHighLightGameObject(baseGo, true, form, true);
        base.Initialize();
    }

    protected override bool IsDelegateClickEvent()
    {
        return true;
    }
}

