﻿namespace Assets.Scripts.GameLogic
{
    using System;

    public class SkillAbort
    {
        private bool[] abortRuleArray = new bool[14];
        public const int MAX_ABORTTYPE_COUNT = 14;

        public bool Abort(SkillAbortType _type)
        {
            int index = (int) _type;
            return (((index >= 0) && (index <= 13)) && this.abortRuleArray[index]);
        }

        public bool AbortWithAI()
        {
            for (int i = 0; i < 14; i++)
            {
                if (!this.abortRuleArray[i])
                {
                    return false;
                }
            }
            return true;
        }

        public void InitAbort(bool _bAbort)
        {
            for (int i = 0; i < 14; i++)
            {
                this.abortRuleArray[i] = _bAbort;
            }
            this.abortRuleArray[13] = false;
        }

        public void SetAbort(SkillAbortType _type)
        {
            int index = (int) _type;
            if ((index >= 0) && (index <= 13))
            {
                this.abortRuleArray[index] = true;
            }
        }
    }
}

