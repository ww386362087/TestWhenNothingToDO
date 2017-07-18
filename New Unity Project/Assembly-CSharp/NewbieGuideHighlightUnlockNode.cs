﻿using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using System;
using UnityEngine;

public class NewbieGuideHighlightUnlockNode : NewbieGuideBaseScript
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
            CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CTaskSys.TASK_FORM_PATH);
            if (form != null)
            {
                CTaskView taskView = Singleton<CTaskSys>.GetInstance().GetTaskView();
                if (taskView != null)
                {
                    taskView.On_Tab_Change(0);
                    form.GetWidget(2).GetComponent<CUIListScript>().SelectElement(0, true);
                    GameObject widget = form.GetWidget(12);
                    if ((widget != null) && (widget != null))
                    {
                        base.AddHighLightAreaClickAnyWhere(widget, form);
                        base.Initialize();
                    }
                }
            }
        }
    }
}

