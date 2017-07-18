﻿namespace Assets.Scripts.GameLogic
{
    using Assets.Scripts.GameLogic.GameKernal;
    using ResData;
    using System;

    [StarConditionAttrContext(10)]
    internal class ClearanceTeamCountCondition : StarCondition
    {
        private int MemberCount;

        public override void Initialize(ResDT_ConditionInfo InConditionInfo)
        {
            base.Initialize(InConditionInfo);
        }

        public override void Start()
        {
            base.Start();
            Player hostPlayer = Singleton<GamePlayerCenter>.instance.GetHostPlayer();
            DebugHelper.Assert(hostPlayer != null, "Hostplayer should not be null");
            this.MemberCount = hostPlayer.heroCount;
        }

        public override StarEvaluationStatus status
        {
            get
            {
                return (!SmartCompare.Compare<int>(this.MemberCount, this.targetCount, this.operation) ? StarEvaluationStatus.Failure : StarEvaluationStatus.Success);
            }
        }

        private int targetCount
        {
            get
            {
                return base.ConditionInfo.ValueDetail[0];
            }
        }

        public override int[] values
        {
            get
            {
                return new int[] { this.MemberCount };
            }
        }
    }
}

