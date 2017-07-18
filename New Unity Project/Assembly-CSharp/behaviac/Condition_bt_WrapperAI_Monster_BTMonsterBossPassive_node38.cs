﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Condition_bt_WrapperAI_Monster_BTMonsterBossPassive_node38 : Condition
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            uint variable = (uint) pAgent.GetVariable((uint) 0x4349179f);
            int range = (int) pAgent.GetVariable((uint) 0x73e592c4);
            bool flag = ((ObjAgent) pAgent).IsDistanceToActorLessThanRange(variable, range);
            bool flag2 = true;
            return ((flag != flag2) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS);
        }
    }
}

