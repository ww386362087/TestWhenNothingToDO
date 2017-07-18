﻿using CSProtocol;
using System;

[CheatCommand("英雄/属性修改/数值/AddAcntMaxAP", "设置最大体力值", 5), ArgumentDescription(typeof(int), "体力值", new object[] {  })]
internal class AddAcntMaxAPCommand : CommonValueChangeCommand
{
    protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
    {
        CheatCmdRef.stAddAcntMaxAP = new CSDT_CHEAT_COMVAL();
        CheatCmdRef.stAddAcntMaxAP.iValue = InValue;
    }
}

