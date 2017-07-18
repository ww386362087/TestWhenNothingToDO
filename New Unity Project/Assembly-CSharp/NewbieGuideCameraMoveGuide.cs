﻿using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using System;

internal class NewbieGuideCameraMoveGuide : NewbieGuideBaseScript
{
    protected override void Initialize()
    {
    }

    protected override bool IsDelegateClickEvent()
    {
        return true;
    }

    protected override bool IsShowGuideMask()
    {
        return false;
    }

    protected override void Update()
    {
        if (base.isInitialize)
        {
            base.Update();
        }
        else
        {
            GameSettings.TheCameraMoveType = (CameraMoveType) base.currentConf.Param[1];
            Singleton<CBattleGuideManager>.GetInstance().OpenFormShared((CBattleGuideManager.EBattleGuideFormType) base.currentConf.Param[0], 0, false);
            this.CompleteHandler();
        }
    }
}

