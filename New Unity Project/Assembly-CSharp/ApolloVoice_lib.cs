﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class ApolloVoice_lib : IApolloVoice
{
    private static bool bInit;
    private static AndroidJavaClass mApolloVoice;

    public int _CloseMic()
    {
        if (!bInit)
        {
            return 0;
        }
        int num = ApolloVoiceCloseMic();
        if (num == 0)
        {
            return 0;
        }
        return num;
    }

    public int _CloseSpeaker()
    {
        if (!bInit)
        {
            return 0;
        }
        int num = ApolloVoiceCloseSpeaker();
        if (num == 0)
        {
            return 0;
        }
        return num;
    }

    public int _CreateApolloVoiceEngine(string appID)
    {
        int num = ApolloVoiceCreateEngine(appID);
        if (num == 0)
        {
            bInit = true;
        }
        return num;
    }

    public int _DestoryApolloVoiceEngine()
    {
        if (!bInit)
        {
            return 0;
        }
        bInit = false;
        int num = ApolloVoiceDestoryEngine();
        if (num == 0)
        {
            return 0;
        }
        return num;
    }

    public int _DownloadVoiceFile(string strFullPath, bool bAutoPlay)
    {
        return 0;
    }

    public int _EnableMemberVoice(int nMemberId, bool bEnable)
    {
        if (!bInit)
        {
            return 4;
        }
        return ApolloVoiceForbidMemberVoice(nMemberId, bEnable);
    }

    public int _GetFileKey(byte[] fileKey)
    {
        if (!bInit)
        {
            return 0;
        }
        int length = fileKey.Length;
        return ApolloVoiceGetFileKey(fileKey, length);
    }

    public int _GetJoinRoomResult()
    {
        if (!bInit)
        {
            return 4;
        }
        return ApolloVoiceGetJoinRoomResult();
    }

    public int _GetMemberState(int[] memberState)
    {
        if (!bInit)
        {
            return 0;
        }
        int length = memberState.Length;
        StringBuilder builder = new StringBuilder(length * 4);
        int num2 = ApolloVoiceGetMemberState(builder, length);
        if (num2 <= 0)
        {
            return 0;
        }
        string str2 = builder.ToString();
        int startIndex = 0;
        int num4 = 0;
        int index = 0;
        for (int i = 0; (i < str2.Length) && (index < length); i++)
        {
            char ch = str2[i];
            if (ch.ToString() == "#")
            {
                num4 = i;
                string s = str2.Substring(startIndex, num4 - startIndex);
                memberState[index] = int.Parse(s);
                startIndex = i + 1;
                index++;
            }
        }
        return num2;
    }

    public int _GetMicLevel()
    {
        if (!bInit)
        {
            return 0;
        }
        return ApolloGetMicLevel();
    }

    public int _GetSpeakerLevel()
    {
        if (!bInit)
        {
            return 0;
        }
        return ApolloVoiceGetSpeakerLevel();
    }

    public int _JoinRoom(string url1, string url2, string url3, long roomId, long roomKey, short memberId, string OpenId, int nTimeOut)
    {
        if (!bInit)
        {
            return 0;
        }
        int num = ApolloVoiceJoinRoom(url1, url2, url3, roomId, roomKey, memberId, OpenId, nTimeOut);
        if (num == 0)
        {
            return 0;
        }
        return num;
    }

    public int _OpenMic()
    {
        if (!bInit)
        {
            return 0;
        }
        int num = ApolloVoiceOpenMic();
        if (num == 0)
        {
            return 0;
        }
        return num;
    }

    public int _OpenSpeaker()
    {
        if (!bInit)
        {
            return 0;
        }
        int num = ApolloVoiceOpenSpeaker();
        if (num == 0)
        {
            return 0;
        }
        return num;
    }

    public int _Pause()
    {
        if (!bInit)
        {
            return 0;
        }
        int num = ApolloVoicePause();
        if (num == 0)
        {
            return 0;
        }
        return num;
    }

    public int _PlayFile(string strFullPath)
    {
        if (bInit)
        {
            ApolloVoicePlayFile(strFullPath);
        }
        return 0;
    }

    public int _QuitRoom(long roomId, short memberId, string OpenId)
    {
        if (!bInit)
        {
            return 0;
        }
        byte[] bytes = Encoding.ASCII.GetBytes(OpenId);
        int num = ApolloVoiceQuitRoom(roomId, memberId, bytes);
        if (num == 0)
        {
            return 0;
        }
        return num;
    }

    public int _Resume()
    {
        if (!bInit)
        {
            return 0;
        }
        int num = ApolloVoiceResume();
        if (num == 0)
        {
            return 0;
        }
        return num;
    }

    public int _SendRecFile(string fileKey)
    {
        return 0;
    }

    public int _SetMemberCount(int nCount)
    {
        if (!bInit)
        {
            return 0;
        }
        return ApolloVoiceSetMemberCount(nCount);
    }

    public int _SetMode(int nMode)
    {
        if (bInit)
        {
            ApolloVoiceSetMode(nMode);
        }
        return 0;
    }

    public int _SetSpeakerVolume(int nVol)
    {
        if (!bInit)
        {
            return 0;
        }
        return ApolloVoiceSetSpeakerVolume(nVol);
    }

    public int _StartRecord(string strFullPath)
    {
        if (!bInit)
        {
            return 0;
        }
        return ApolloVoiceStartRecord(strFullPath);
    }

    public int _StopRecord(bool bAutoSend)
    {
        if (!bInit)
        {
            return 0;
        }
        return ApolloVoiceStopRecord(bAutoSend);
    }

    public int _TestMic()
    {
        if (!bInit)
        {
            return 0;
        }
        return ApolloVoiceTestMic();
    }

    [DllImport("apollo_voice")]
    private static extern int ApolloGetMicLevel();
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceCloseMic();
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceCloseSpeaker();
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceCreateEngine([MarshalAs(UnmanagedType.LPArray)] string appID);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceDestoryEngine();
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceForbidMemberVoice(int nMemberId, bool bEnable);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceGetFileKey(byte[] filekey, int nSize);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceGetJoinRoomResult();
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceGetMemberState([MarshalAs(UnmanagedType.LPStr)] StringBuilder memberState, int nSize);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceGetPhoneMode();
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceGetSpeakerLevel();
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceJoinRoom([MarshalAs(UnmanagedType.LPArray)] string url1, [MarshalAs(UnmanagedType.LPArray)] string url2, [MarshalAs(UnmanagedType.LPArray)] string url3, long roomId, long roomKey, short memberId, [MarshalAs(UnmanagedType.LPArray)] string openId, int nTimeOut);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceOpenMic();
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceOpenSpeaker();
    [DllImport("apollo_voice")]
    private static extern int ApolloVoicePause();
    [DllImport("apollo_voice")]
    private static extern int ApolloVoicePlayFile([MarshalAs(UnmanagedType.LPArray)] string strFullPath);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceQuitRoom(long roomId, short memberId, byte[] OpenId);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceResume();
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceSendRecFile([MarshalAs(UnmanagedType.LPArray)] string strFullPath);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceSetMemberCount(int nCount);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceSetMode(int nMode);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceSetSpeakerVolume(int nvol);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceStartRecord([MarshalAs(UnmanagedType.LPArray)] string strFullPath);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceStopRecord(bool bAutoSend);
    [DllImport("apollo_voice")]
    private static extern int ApolloVoiceTestMic();
    public void Init()
    {
    }
}

