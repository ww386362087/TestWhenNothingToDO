﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Assignment_bt_WrapperAI_Soldier_BTSoldierNormal_node107 : Assignment
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            EBTStatus status = EBTStatus.BT_SUCCESS;
            int variable = (int) pAgent.GetVariable((uint) 0x921d0d6a);
            uint withOutActor = (uint) pAgent.GetVariable((uint) 0xb81a7cc);
            uint nearestEnemyWithoutJungleMonsterWithoutActor = ((ObjAgent) pAgent).GetNearestEnemyWithoutJungleMonsterWithoutActor(variable, withOutActor);
            pAgent.SetVariable<uint>("p_targetID", nearestEnemyWithoutJungleMonsterWithoutActor, 0x4349179f);
            return status;
        }
    }
}

