﻿namespace behaviac
{
    using Assets.Scripts.GameLogic;
    using ResData;

    internal class Condition_bt_WrapperAI_Hero_HeroSimpleAI_node98 : Condition
    {
        private SkillSlotType opl_p0 = SkillSlotType.SLOT_SKILL_6;

        protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
        {
            RES_SUMMONERSKILL_TYPE summonerSkillType = ((ObjAgent) pAgent).GetSummonerSkillType(this.opl_p0);
            RES_SUMMONERSKILL_TYPE res_summonerskill_type2 = RES_SUMMONERSKILL_TYPE.RES_SUMMONERSKILL_RETURNCITY;
            return ((summonerSkillType != res_summonerskill_type2) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS);
        }
    }
}

