﻿namespace Assets.Scripts.GameSystem
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Calc9SlotHeroData
    {
        public uint ConfigId;
        public int RecommendPos;
        public uint Ability;
        public uint Level;
        public int Quality;
        public int BornIndex;
        public bool selected;
    }
}

