﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;

    internal class Assignment_bt_WrapperAI_Hero_HeroSimpleAI_node172 : Assignment
    {
        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            EBTStatus status = EBTStatus.BT_SUCCESS;
            SkillSlotType type = SkillSlotType.SLOT_SKILL_1;
            pAgent.SetVariable<SkillSlotType>("p_curSlotType", type, 0x6c745b);
            return status;
        }
    }
}

