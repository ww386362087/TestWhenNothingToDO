﻿namespace Assets.Scripts.GameLogic
{
    using System;

    public class SkillTimerInfo
    {
        public int leftTime;
        public ulong starTime;
        public int totalTime;

        public SkillTimerInfo(int _totalTime, int _leftTime, ulong _starTime)
        {
            this.totalTime = _totalTime;
            this.leftTime = _leftTime;
            this.starTime = _starTime;
        }

        public void setSkillTimerParam(int _totalTime, int _leftTime, ulong _starTime)
        {
            this.totalTime = _totalTime;
            this.leftTime = _leftTime;
            this.starTime = _starTime;
        }
    }
}

