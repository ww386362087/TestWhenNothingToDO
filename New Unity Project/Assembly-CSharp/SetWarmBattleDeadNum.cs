﻿using CSProtocol;
using System;

[ArgumentDescription(typeof(uint), "死亡数", new object[] {  }), CheatCommand("关卡/温暖局/SetWarmBattleDeadNum", "设置温暖局死亡数", 0x3e)]
internal class SetWarmBattleDeadNum : CommonValueChangeCommand
{
    protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
    {
        CheatCmdRef.stWarmBattleDeadNum = new CSDT_CHEAT_COMVAL();
        CheatCmdRef.stWarmBattleDeadNum.iValue = InValue;
    }
}

