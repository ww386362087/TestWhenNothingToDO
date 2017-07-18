﻿using Assets.Scripts.GameSystem;
using CSProtocol;
using System;

[CheatCommand("工具/SetIntimacy", "设置亲密度", 0x53), ArgumentDescription(1, typeof(ulong), "ullUid", new object[] {  }), ArgumentDescription(2, typeof(uint), "worldID", new object[] {  }), ArgumentDescription(3, typeof(ushort), "亲密度值", new object[] {  })]
internal class SetIntimacy : CheatCommandNetworking
{
    protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
    {
        ulong num = CheatCommandBase.SmartConvert<ulong>(InArguments[0]);
        uint num2 = CheatCommandBase.SmartConvert<uint>(InArguments[1]);
        CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
        CheatCmdRef.stChgIntimacy = new CSDT_CHEAT_CHG_INTIMACY();
        CheatCmdRef.stChgIntimacy.stUin.ullUid = (num != 0) ? num : masterRoleInfo.playerUllUID;
        CheatCmdRef.stChgIntimacy.stUin.dwLogicWorldId = (num2 != 0) ? num2 : ((uint) masterRoleInfo.logicWorldID);
        CheatCmdRef.stChgIntimacy.wIntimacyValue = CheatCommandBase.SmartConvert<ushort>(InArguments[2]);
        return CheatCommandBase.Done;
    }
}

