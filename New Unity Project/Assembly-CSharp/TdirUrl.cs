﻿using Apollo;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct TdirUrl
{
    public int nodeID;
    public string name;
    public TdirSvrStatu statu;
    public SvrFlag flag;
    public uint mask;
    public uint roleCount;
    public string attr;
    public List<IPAddrInfo> addrs;
    public int logicWorldID;
    public uint lastLoginTime;
    public bool useNetAcc;
    public bool isPreSvr;
}

