﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;

    internal class Action_bt_WrapperAI_Organ_OrganTurret_node88 : Action
    {
        private SkillSlotType method_p0 = SkillSlotType.SLOT_SKILL_1;

        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            ((ObjAgent) pAgent).SetSkillSpecial(this.method_p0);
            return EBTStatus.BT_SUCCESS;
        }
    }
}

