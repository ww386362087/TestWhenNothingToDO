﻿using Assets.Scripts.UI;
using System;

internal class NewbieGuideIntroFireMatch : NewbieGuideBaseScript
{
    protected override void Initialize()
    {
        CUIEvent uiEventParam = new CUIEvent();
        uiEventParam.m_eventID = enUIEventID.MatchingExt_BeginEnterTrainMent;
        uiEventParam.m_eventParams.commonBool = true;
        uint.TryParse(Singleton<CTextManager>.instance.GetText("MapID_PVP_Fire"), out uiEventParam.m_eventParams.tagUInt);
        string[] imgPath = new string[] { string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Newbie_Dir, "huokeng1"), string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Newbie_Dir, "huokeng2"), string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Newbie_Dir, "huokeng3") };
        Singleton<CBattleGuideManager>.GetInstance().OpenBannerIntroDialog(0, imgPath, 3, uiEventParam, null, null, true, false);
        this.CompleteHandler();
    }

    protected override bool IsDelegateClickEvent()
    {
        return true;
    }
}

