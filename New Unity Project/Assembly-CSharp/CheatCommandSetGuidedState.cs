﻿using Assets.Scripts.GameSystem;
using CSProtocol;
using System;

[CheatCommand("关卡/CheatCommandSetGuidedState", "设置服务器位", 0x1c), ArgumentDescription(1, typeof(byte), "开启或关闭(1置位0不置)", new object[] {  }), ArgumentDescription(0, typeof(uint), "服务器位ID", new object[] {  })]
internal class CheatCommandSetGuidedState : CheatCommandNetworking
{
    protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
    {
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        if ((masterRoleInfo != null) && Singleton<LobbyLogic>.GetInstance().isLogin)
        {
            CheatCmdRef.stDyeNewbieBit = new CSDT_CHEAT_DYE_NEWBIE_BIT();
            CheatCmdRef.stDyeNewbieBit.bOpenOrClose = CheatCommandBase.SmartConvert<byte>(InArguments[1]);
            CheatCmdRef.stDyeNewbieBit.bIsAll = 0;
            CheatCmdRef.stDyeNewbieBit.dwApntBit = CheatCommandBase.SmartConvert<uint>(InArguments[0]);
            masterRoleInfo.SetGuidedStateSet(CheatCommandBase.SmartConvert<uint>(InArguments[0]), CheatCommandBase.SmartConvert<byte>(InArguments[1]) != 0);
            return CheatCommandBase.Done;
        }
        return "undone";
    }
}

