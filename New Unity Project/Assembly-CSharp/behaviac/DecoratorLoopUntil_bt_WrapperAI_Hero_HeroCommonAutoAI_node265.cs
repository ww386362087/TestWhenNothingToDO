﻿namespace behaviac
{
    using System;

    internal class DecoratorLoopUntil_bt_WrapperAI_Hero_HeroCommonAutoAI_node265 : DecoratorLoopUntil
    {
        public DecoratorLoopUntil_bt_WrapperAI_Hero_HeroCommonAutoAI_node265()
        {
            base.m_bDecorateWhenChildEnds = true;
            base.m_until = true;
        }

        protected override int GetCount(Agent pAgent)
        {
            return -1;
        }
    }
}

