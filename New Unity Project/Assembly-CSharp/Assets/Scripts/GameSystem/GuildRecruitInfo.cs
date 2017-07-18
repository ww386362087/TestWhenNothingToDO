﻿namespace Assets.Scripts.GameSystem
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct GuildRecruitInfo
    {
        public ulong senderUid;
        public byte senderLevel;
        public ulong guildId;
        public uint sendTime;
        public string guildName;
        public string senderName;
        public string senderHeadUrl;
        public byte limitGrade;
        public byte limitLevel;
    }
}

