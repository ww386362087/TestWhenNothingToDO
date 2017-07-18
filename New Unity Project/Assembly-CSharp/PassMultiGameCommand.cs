﻿using CSProtocol;
using System;

[CheatCommand("关卡/PassMultiGame", "通过多人pvp", 30), ArgumentDescription(0, typeof(EWinOrLose), "胜利或失败", new object[] {  })]
internal class PassMultiGameCommand : CheatCommandNetworking
{
    protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
    {
        CheatCmdRef.stPassMultiGame = new CSDT_CHEAT_PASS_MULTI_GAME();
        EWinOrLose lose = (EWinOrLose) CheatCommandBase.StringToEnum(InArguments[0], typeof(EWinOrLose));
        CheatCmdRef.stPassMultiGame.bGameResult = (lose != EWinOrLose.胜利) ? ((byte) 2) : ((byte) 1);
        return CheatCommandBase.Done;
    }
}

