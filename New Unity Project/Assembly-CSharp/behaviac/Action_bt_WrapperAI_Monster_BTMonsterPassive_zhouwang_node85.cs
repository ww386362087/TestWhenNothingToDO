﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using System;

    internal class Action_bt_WrapperAI_Monster_BTMonsterPassive_zhouwang_node85 : Action
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            SkillSlotType variable = (SkillSlotType) ((int) pAgent.GetVariable((uint) 0x6c745b));
            return ((ObjAgent) pAgent).SetSkill(variable);
        }
    }
}

