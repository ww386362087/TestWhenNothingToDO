﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;

    internal class Action_bt_WrapperAI_Soldier_BTSoldierSiege_node112 : Action
    {
        private SkillSlotType method_p0 = SkillSlotType.SLOT_SKILL_0;

        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            return ((ObjAgent) pAgent).SetSkill(this.method_p0);
        }
    }
}

