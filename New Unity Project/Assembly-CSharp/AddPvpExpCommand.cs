﻿using CSProtocol;
using System;

[ArgumentDescription(typeof(int), "数量", new object[] {  }), CheatCommand("英雄/属性修改/经验等级/AddPvpExp", "加PVP经验", 0x25)]
internal class AddPvpExpCommand : CommonValueChangeCommand
{
    protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
    {
        CheatCmdRef.stAddPvpExp = new CSDT_CHEAT_COMVAL();
        CheatCmdRef.stAddPvpExp.iValue = InValue;
    }
}

