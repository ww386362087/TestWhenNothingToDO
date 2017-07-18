﻿using System;
using UnityEngine;

internal class GlobalConfig : MonoSingleton<GlobalConfig>
{
    [FriendlyName("右摇杆控制是否合并玩家移动")]
    public bool bComposePlayerMovement = true;
    [FriendlyName("是否开启游戏内控制台")]
    public bool bEnableInGameConsole;
    [FriendlyName("是否开启特效裁剪优化")]
    public bool bEnableParticleCullOptimize = true;
    [FriendlyName("PVE真实时间Tick")]
    public bool bEnableRealTimeTickInPVE;
    [HideInInspector]
    public bool bOnExternalSpeedPicker;
    [FriendlyName("右摇杆遇零重置速度")]
    public bool bResetCameraSpeedWhenZero;
    [FriendlyName("人为操作模拟")]
    public bool bSimulateHumanOperation;
    [FriendlyName("模拟丢包")]
    public bool bSimulateLosePackage;
    [FriendlyName("右摇杆移动加速度")]
    public float CameraMoveAcceleration = 10000f;
    [FriendlyName("右摇杆移动速度")]
    public float CameraMoveSpeed = 10000f;
    [FriendlyName("右摇杆最大速度")]
    public float CameraMoveSpeedMax = 30000f;
    [FriendlyName("宝箱怪掉落概率")]
    public int ChestMonsterDropItemProbability = 0x4b;
    [FriendlyName("机关反隐分帧数")]
    public int DefenseAntiHiddenFrameInterval = 4;
    [FriendlyName("机关反隐伤害ID")]
    public int DefenseAntiHiddenHurtId = 0xf4244;
    [FriendlyName("机关反隐效果间隔")]
    public int DefenseAntiHiddenInterval = 400;
    [FriendlyName("掉落物飞起高度")]
    public int DropItemFlyHeight = 0x2710;
    [FriendlyName("掉落物飞翔时间")]
    public int DropItemFlyTime = 0x4b0;
    [FriendlyName("迷雾渲染插值帧数")]
    public int GPUInterpolateFrameInterval = 6;
    [FriendlyName("摇杆最大移动距离")]
    public int JoysticMaxExtendDist = 200;
    [FriendlyName("摇杆初始位置xy偏移")]
    public Vector2 JoysticRootPos = new Vector2(240f, 240f);
    [FriendlyName("普通怪掉落概率")]
    public int NormalMonsterDropItemProbability = 10;
    [FriendlyName("机关掉落概率")]
    public int OrganDropItemProbability = 50;
    [FriendlyName("右Panel移动速度")]
    public float PanelCameraMoveSpeed = 50f;
    [FriendlyName("拾取范围")]
    public int PickupRange = 0xbb8;
    [FriendlyName("刷单个怪之间的间隔")]
    public int SoldierWaveInterval = 0x3e8;
    [HideInInspector]
    public int UnityMainThreadID;
    public int WaypointIgnoreDist = 0x1869f;
}

