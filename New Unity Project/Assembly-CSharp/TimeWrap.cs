﻿using com.tencent.pandora;
using System;
using UnityEngine;

public class TimeWrap
{
    private static Type classType = typeof(Time);

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int _CreateTime(IntPtr L)
    {
        if (LuaDLL.lua_gettop(L) == 0)
        {
            Time o = new Time();
            LuaScriptMgr.PushObject(L, o);
            return 1;
        }
        LuaDLL.luaL_error(L, "invalid arguments to method: Time.New");
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_captureFramerate(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_captureFramerate());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_deltaTime(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_deltaTime());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_fixedDeltaTime(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_fixedDeltaTime());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_fixedTime(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_fixedTime());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_frameCount(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_frameCount());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_maximumDeltaTime(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_maximumDeltaTime());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_realtimeSinceStartup(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_realtimeSinceStartup());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_renderedFrameCount(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_renderedFrameCount());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_smoothDeltaTime(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_smoothDeltaTime());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_time(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_time());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_timeScale(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_timeScale());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_timeSinceLevelLoad(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_timeSinceLevelLoad());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_unscaledDeltaTime(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_unscaledDeltaTime());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int get_unscaledTime(IntPtr L)
    {
        LuaScriptMgr.Push(L, Time.get_unscaledTime());
        return 1;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int GetClassType(IntPtr L)
    {
        LuaScriptMgr.Push(L, classType);
        return 1;
    }

    public static void Register(IntPtr L)
    {
        LuaMethod[] regs = new LuaMethod[] { new LuaMethod("New", new LuaCSFunction(TimeWrap._CreateTime)), new LuaMethod("GetClassType", new LuaCSFunction(TimeWrap.GetClassType)) };
        LuaField[] fields = new LuaField[] { new LuaField("time", new LuaCSFunction(TimeWrap.get_time), null), new LuaField("timeSinceLevelLoad", new LuaCSFunction(TimeWrap.get_timeSinceLevelLoad), null), new LuaField("deltaTime", new LuaCSFunction(TimeWrap.get_deltaTime), null), new LuaField("fixedTime", new LuaCSFunction(TimeWrap.get_fixedTime), null), new LuaField("unscaledTime", new LuaCSFunction(TimeWrap.get_unscaledTime), null), new LuaField("unscaledDeltaTime", new LuaCSFunction(TimeWrap.get_unscaledDeltaTime), null), new LuaField("fixedDeltaTime", new LuaCSFunction(TimeWrap.get_fixedDeltaTime), new LuaCSFunction(TimeWrap.set_fixedDeltaTime)), new LuaField("maximumDeltaTime", new LuaCSFunction(TimeWrap.get_maximumDeltaTime), new LuaCSFunction(TimeWrap.set_maximumDeltaTime)), new LuaField("smoothDeltaTime", new LuaCSFunction(TimeWrap.get_smoothDeltaTime), null), new LuaField("timeScale", new LuaCSFunction(TimeWrap.get_timeScale), new LuaCSFunction(TimeWrap.set_timeScale)), new LuaField("frameCount", new LuaCSFunction(TimeWrap.get_frameCount), null), new LuaField("renderedFrameCount", new LuaCSFunction(TimeWrap.get_renderedFrameCount), null), new LuaField("realtimeSinceStartup", new LuaCSFunction(TimeWrap.get_realtimeSinceStartup), null), new LuaField("captureFramerate", new LuaCSFunction(TimeWrap.get_captureFramerate), new LuaCSFunction(TimeWrap.set_captureFramerate)) };
        LuaScriptMgr.RegisterLib(L, "UnityEngine.Time", typeof(Time), regs, fields, typeof(object));
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int set_captureFramerate(IntPtr L)
    {
        Time.set_captureFramerate((int) LuaScriptMgr.GetNumber(L, 3));
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int set_fixedDeltaTime(IntPtr L)
    {
        Time.set_fixedDeltaTime((float) LuaScriptMgr.GetNumber(L, 3));
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int set_maximumDeltaTime(IntPtr L)
    {
        Time.set_maximumDeltaTime((float) LuaScriptMgr.GetNumber(L, 3));
        return 0;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int set_timeScale(IntPtr L)
    {
        Time.set_timeScale((float) LuaScriptMgr.GetNumber(L, 3));
        return 0;
    }
}

