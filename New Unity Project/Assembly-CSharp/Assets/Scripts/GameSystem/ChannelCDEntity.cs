﻿namespace Assets.Scripts.GameSystem
{
    using System;

    public class ChannelCDEntity : CDEntity
    {
        public int channelID;

        public ChannelCDEntity(int channelID, int cd_time) : base(cd_time, 0)
        {
            this.channelID = channelID;
        }
    }
}

