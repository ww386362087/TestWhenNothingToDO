﻿using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using System;
using UnityEngine;

public class NewbieGuideClickDefense : NewbieGuideBaseScript
{
    private CUIFormScript battleForm;
    private GameObject buttonObj;
    private CSignalButton signalButton;
    private float timer;
    private float timeToWait = 2f;

    protected override void Initialize()
    {
        this.battleForm = Singleton<CUIManager>.GetInstance().GetForm(FightForm.s_battleUIForm);
        if (this.battleForm != null)
        {
            this.buttonObj = this.battleForm.GetWidget(14);
            CUIEventScript component = this.buttonObj.GetComponent<CUIEventScript>();
            this.signalButton = Singleton<CBattleSystem>.GetInstance().FightForm.GetSignalPanel().GetSingleButton(component.m_onClickEventParams.tag);
        }
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
            this.timer += Time.get_deltaTime();
            if ((this.timer >= this.timeToWait) && !this.signalButton.IsInCooldown())
            {
                base.AddHighLightGameObject(this.buttonObj, true, this.battleForm, true);
                base.Initialize();
            }
        }
    }
}

