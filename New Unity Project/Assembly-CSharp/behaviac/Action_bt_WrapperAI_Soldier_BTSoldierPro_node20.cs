﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Action_bt_WrapperAI_Soldier_BTSoldierPro_node20 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            SkillSlotType variable = (SkillSlotType) ((int) pAgent.GetVariable((uint) 0x6c745b));
            return ((ObjAgent) pAgent).RealUseSkill(variable);
        }
    }
}

