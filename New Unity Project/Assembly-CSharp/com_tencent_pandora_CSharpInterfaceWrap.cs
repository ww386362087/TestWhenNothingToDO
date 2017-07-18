﻿using com.tencent.pandora;
using System;
using UnityEngine;
using UnityEngine.UI;

public class com_tencent_pandora_CSharpInterfaceWrap
{
    private static Type classType = typeof(CSharpInterface);

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int _Createcom_tencent_pandora_CSharpInterface(IntPtr L)
    {
        if (LuaDLL.lua_gettop(L) == 0)
        {
            CSharpInterface o = new CSharpInterface();
            LuaScriptMgr.PushObject(L, o);
            return 1;
        }
        LuaDLL.luaL_error(L, "invalid arguments to method: com.tencent.pandora.CSharpInterface.New");
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int AddClick(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 2);
        GameObject go = LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
        LuaFunction luaFunction = LuaScriptMgr.GetLuaFunction(L, 2);
        CSharpInterface.AddClick(go, luaFunction);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int AddUGUIOnClickDown(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 2);
        GameObject go = LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
        LuaFunction luaFunction = LuaScriptMgr.GetLuaFunction(L, 2);
        CSharpInterface.AddUGUIOnClickDown(go, luaFunction);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int AddUGUIOnClickUp(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 2);
        GameObject go = LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
        LuaFunction luaFunction = LuaScriptMgr.GetLuaFunction(L, 2);
        CSharpInterface.AddUGUIOnClickUp(go, luaFunction);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int AndroidPay(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        bool b = CSharpInterface.AndroidPay(LuaScriptMgr.GetLuaString(L, 1));
        LuaScriptMgr.Push(L, b);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int AssembleFont(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 2);
        string luaString = LuaScriptMgr.GetLuaString(L, 1);
        string jsonFontTable = LuaScriptMgr.GetLuaString(L, 2);
        int d = CSharpInterface.AssembleFont(luaString, jsonFontTable);
        LuaScriptMgr.Push(L, d);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int AsyncDownloadImage(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        CSharpInterface.AsyncDownloadImage(LuaScriptMgr.GetLuaString(L, 1));
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int AsyncSetImage(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 4);
        string luaString = LuaScriptMgr.GetLuaString(L, 1);
        string url = LuaScriptMgr.GetLuaString(L, 2);
        Image image = LuaScriptMgr.GetUnityObject(L, 3, typeof(Image));
        uint number = (uint) LuaScriptMgr.GetNumber(L, 4);
        CSharpInterface.AsyncSetImage(luaString, url, image, number);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int CallBroker(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 3);
        uint number = (uint) LuaScriptMgr.GetNumber(L, 1);
        string luaString = LuaScriptMgr.GetLuaString(L, 2);
        int cmdId = (int) LuaScriptMgr.GetNumber(L, 3);
        CSharpInterface.CallBroker(number, luaString, cmdId);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int CallGame(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        CSharpInterface.CallGame(LuaScriptMgr.GetLuaString(L, 1));
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int CreatePanel(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 2);
        uint number = (uint) LuaScriptMgr.GetNumber(L, 1);
        string luaString = LuaScriptMgr.GetLuaString(L, 2);
        CSharpInterface.CreatePanel(number, luaString);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int DestroyPanel(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        CSharpInterface.DestroyPanel(LuaScriptMgr.GetLuaString(L, 1));
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int DoCmdFromGame(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        CSharpInterface.DoCmdFromGame(LuaScriptMgr.GetLuaString(L, 1));
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int ExecCallback(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 2);
        uint number = (uint) LuaScriptMgr.GetNumber(L, 1);
        string luaString = LuaScriptMgr.GetLuaString(L, 2);
        CSharpInterface.ExecCallback(number, luaString);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_isApplePlatform(IntPtr L)
    {
        LuaScriptMgr.Push(L, CSharpInterface.isApplePlatform);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int GetClassType(IntPtr L)
    {
        LuaScriptMgr.Push(L, classType);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int GetFunctionSwitch(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        bool functionSwitch = CSharpInterface.GetFunctionSwitch(LuaScriptMgr.GetLuaString(L, 1));
        LuaScriptMgr.Push(L, functionSwitch);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int GetLogger(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 0);
        Logger logger = CSharpInterface.GetLogger();
        LuaScriptMgr.Push(L, (Object) logger);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int GetPanel(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        GameObject panel = CSharpInterface.GetPanel(LuaScriptMgr.GetLuaString(L, 1));
        LuaScriptMgr.Push(L, (Object) panel);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int GetPlatformDesc(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 0);
        string platformDesc = CSharpInterface.GetPlatformDesc();
        LuaScriptMgr.Push(L, platformDesc);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int GetSDKVersion(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 0);
        string sDKVersion = CSharpInterface.GetSDKVersion();
        LuaScriptMgr.Push(L, sDKVersion);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int GetTotalSwitch(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 0);
        bool totalSwitch = CSharpInterface.GetTotalSwitch();
        LuaScriptMgr.Push(L, totalSwitch);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int GetUserData(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 0);
        UserData userData = CSharpInterface.GetUserData();
        LuaScriptMgr.PushObject(L, userData);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int IOSPay(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        bool b = CSharpInterface.IOSPay(LuaScriptMgr.GetLuaString(L, 1));
        LuaScriptMgr.Push(L, b);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int IsImageDownloaded(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        bool b = CSharpInterface.IsImageDownloaded(LuaScriptMgr.GetLuaString(L, 1));
        LuaScriptMgr.Push(L, b);
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int NotifyAndroidPayFinish(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        CSharpInterface.NotifyAndroidPayFinish(LuaScriptMgr.GetLuaString(L, 1));
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int NotifyCloseAllPanel(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 0);
        CSharpInterface.NotifyCloseAllPanel();
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int NotifyIOSPayFinish(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        CSharpInterface.NotifyIOSPayFinish(LuaScriptMgr.GetLuaString(L, 1));
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int NotifyPushData(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 2);
        string luaString = LuaScriptMgr.GetLuaString(L, 1);
        string jsonData = LuaScriptMgr.GetLuaString(L, 2);
        CSharpInterface.NotifyPushData(luaString, jsonData);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int ReadCookie(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 1);
        string str = CSharpInterface.ReadCookie(LuaScriptMgr.GetLuaString(L, 1));
        LuaScriptMgr.Push(L, str);
        return 1;
    }

    public static void Register(IntPtr L)
    {
        LuaMethod[] regs = new LuaMethod[] { 
            new LuaMethod("GetLogger", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.GetLogger)), new LuaMethod("GetPlatformDesc", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.GetPlatformDesc)), new LuaMethod("GetSDKVersion", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.GetSDKVersion)), new LuaMethod("WriteCookie", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.WriteCookie)), new LuaMethod("ReadCookie", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.ReadCookie)), new LuaMethod("IOSPay", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.IOSPay)), new LuaMethod("AndroidPay", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.AndroidPay)), new LuaMethod("GetUserData", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.GetUserData)), new LuaMethod("AsyncSetImage", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.AsyncSetImage)), new LuaMethod("ShowGameImg", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.ShowGameImg)), new LuaMethod("AsyncDownloadImage", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.AsyncDownloadImage)), new LuaMethod("IsImageDownloaded", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.IsImageDownloaded)), new LuaMethod("GetTotalSwitch", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.GetTotalSwitch)), new LuaMethod("GetFunctionSwitch", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.GetFunctionSwitch)), new LuaMethod("CallGame", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.CallGame)), new LuaMethod("StreamReport", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.StreamReport)), 
            new LuaMethod("CallBroker", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.CallBroker)), new LuaMethod("AssembleFont", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.AssembleFont)), new LuaMethod("CreatePanel", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.CreatePanel)), new LuaMethod("DestroyPanel", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.DestroyPanel)), new LuaMethod("GetPanel", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.GetPanel)), new LuaMethod("AddClick", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.AddClick)), new LuaMethod("AddUGUIOnClickDown", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.AddUGUIOnClickDown)), new LuaMethod("AddUGUIOnClickUp", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.AddUGUIOnClickUp)), new LuaMethod("ExecCallback", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.ExecCallback)), new LuaMethod("DoCmdFromGame", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.DoCmdFromGame)), new LuaMethod("NotifyIOSPayFinish", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.NotifyIOSPayFinish)), new LuaMethod("NotifyAndroidPayFinish", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.NotifyAndroidPayFinish)), new LuaMethod("NotifyPushData", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.NotifyPushData)), new LuaMethod("NotifyCloseAllPanel", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.NotifyCloseAllPanel)), new LuaMethod("SetPosition", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.SetPosition)), new LuaMethod("SetScale", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.SetScale)), 
            new LuaMethod("SetPosZ", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.SetPosZ)), new LuaMethod("New", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap._Createcom_tencent_pandora_CSharpInterface)), new LuaMethod("GetClassType", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.GetClassType))
         };
        LuaField[] fields = new LuaField[] { new LuaField("isApplePlatform", new LuaCSFunction(com_tencent_pandora_CSharpInterfaceWrap.get_isApplePlatform), null) };
        LuaScriptMgr.RegisterLib(L, "com.tencent.pandora.CSharpInterface", typeof(CSharpInterface), regs, fields, typeof(object));
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int SetPosition(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 4);
        GameObject go = LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
        float number = (float) LuaScriptMgr.GetNumber(L, 2);
        float y = (float) LuaScriptMgr.GetNumber(L, 3);
        float z = (float) LuaScriptMgr.GetNumber(L, 4);
        CSharpInterface.SetPosition(go, number, y, z);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int SetPosZ(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 2);
        GameObject go = LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
        float number = (float) LuaScriptMgr.GetNumber(L, 2);
        CSharpInterface.SetPosZ(go, number);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int SetScale(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 4);
        GameObject go = LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
        float number = (float) LuaScriptMgr.GetNumber(L, 2);
        float y = (float) LuaScriptMgr.GetNumber(L, 3);
        float z = (float) LuaScriptMgr.GetNumber(L, 4);
        CSharpInterface.SetScale(go, number, y, z);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int ShowGameImg(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 4);
        int number = (int) LuaScriptMgr.GetNumber(L, 1);
        int djID = (int) LuaScriptMgr.GetNumber(L, 2);
        GameObject go = LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
        uint callId = (uint) LuaScriptMgr.GetNumber(L, 4);
        CSharpInterface.ShowGameImg(number, djID, go, callId);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int StreamReport(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 3);
        string luaString = LuaScriptMgr.GetLuaString(L, 1);
        int number = (int) LuaScriptMgr.GetNumber(L, 2);
        int returnCode = (int) LuaScriptMgr.GetNumber(L, 3);
        CSharpInterface.StreamReport(luaString, number, returnCode);
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int WriteCookie(IntPtr L)
    {
        LuaScriptMgr.CheckArgsCount(L, 2);
        string luaString = LuaScriptMgr.GetLuaString(L, 1);
        string content = LuaScriptMgr.GetLuaString(L, 2);
        bool b = CSharpInterface.WriteCookie(luaString, content);
        LuaScriptMgr.Push(L, b);
        return 1;
    }
}

