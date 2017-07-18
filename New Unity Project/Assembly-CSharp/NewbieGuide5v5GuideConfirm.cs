﻿using Assets.Scripts.GameLogic;
using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using System;
using UnityEngine;

public class NewbieGuide5v5GuideConfirm : NewbieGuideBaseScript
{
    private CUIEventScript ConfirmBtnScript;
    private CUIFormScript Guide5v5ConfirmForm;

    protected override void ClickHandler(CUIEvent uiEvt)
    {
        if (this.ConfirmBtnScript != null)
        {
            this.ConfirmBtnScript.onClick = (CUIEventScript.OnUIEventHandler) Delegate.Remove(this.ConfirmBtnScript.onClick, new CUIEventScript.OnUIEventHandler(this.ClickHandler));
        }
        if (uiEvt.m_eventParams.tag == 1)
        {
            CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
            this.CompleteHandler();
            LobbyLogic.ReqStartGuideLevel55(false, (uint) masterRoleInfo.acntMobaInfo.iSelectedHeroType);
            masterRoleInfo.SetNewbieAchieve(0x3d, true, true);
        }
    }

    protected override void Initialize()
    {
        this.Guide5v5ConfirmForm = Singleton<CUIManager>.GetInstance().OpenForm(NewbieGuideManager.FORM_5v5GUIDE_CONFIRM, false, true);
        GameObject obj2 = this.Guide5v5ConfirmForm.get_transform().FindChild("Bg/btnConfirm").get_gameObject();
        DebugHelper.Assert(obj2 != null, string.Format("{0}can't find cancel and confirm button in {1}", base.logTitle, NewbieGuideManager.FORM_5v5GUIDE_CONFIRM));
        this.ConfirmBtnScript = obj2.GetComponent<CUIEventScript>();
        if (this.ConfirmBtnScript != null)
        {
            this.ConfirmBtnScript.m_onClickEventParams.tag = 1;
            this.ConfirmBtnScript.onClick = (CUIEventScript.OnUIEventHandler) Delegate.Combine(this.ConfirmBtnScript.onClick, new CUIEventScript.OnUIEventHandler(this.ClickHandler));
        }
        base.Initialize();
    }

    protected override bool IsShowGuideMask()
    {
        return false;
    }
}

