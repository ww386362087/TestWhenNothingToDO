﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Action_bt_WrapperAI_Monster_BTMonsterBossInitiative_node120 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            uint variable = (uint) pAgent.GetVariable((uint) 0x4349179f);
            ((ObjAgent) pAgent).SelectTarget(variable);
            return EBTStatus.BT_SUCCESS;
        }
    }
}

