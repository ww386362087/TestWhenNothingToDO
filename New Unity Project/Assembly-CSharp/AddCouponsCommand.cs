﻿using CSProtocol;
using System;

[CheatCommand("英雄/属性修改/钱币/AddDiamond", "加钻石", 50), ArgumentDescription(typeof(int), "数量", new object[] {  })]
internal class AddCouponsCommand : CommonValueChangeCommand
{
    protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
    {
        CheatCmdRef.stAddDiamond = new CSDT_CHEAT_COMVAL();
        CheatCmdRef.stAddDiamond.iValue = InValue;
    }
}

