﻿namespace Assets.Scripts.GameSystem
{
    using ResData;
    using System;

    public class FixTimePhase : ActivityPhase
    {
        private ResDT_WealFixedTimeReward _config;
        private uint _id;

        public FixTimePhase(Activity owner, uint id, ResDT_WealFixedTimeReward config) : base(owner)
        {
            this._id = id;
            this._config = config;
        }

        public override int CloseTime
        {
            get
            {
                return (int) this._config.dwEndTime;
            }
        }

        public override uint ID
        {
            get
            {
                return this._id;
            }
        }

        public override bool InMultipleTime
        {
            get
            {
                return (this._config.bIsMultiple == 1);
            }
        }

        public override uint RewardID
        {
            get
            {
                return this._config.dwRewardID;
            }
        }

        public override int StartTime
        {
            get
            {
                return (int) this._config.dwStartTime;
            }
        }
    }
}

