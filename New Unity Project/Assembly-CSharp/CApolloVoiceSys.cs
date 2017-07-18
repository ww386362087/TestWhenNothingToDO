﻿using System;
using UnityEngine;

internal class CApolloVoiceSys
{
    private IApolloVoice m_CallApolloVoiceSDK;

    public void SysInitial()
    {
        this.m_CallApolloVoiceSDK = new ApolloVoice_lib();
        if (this.m_CallApolloVoiceSDK == null)
        {
            Debug.Log("apollo voice sdk init error!");
        }
        else
        {
            Debug.Log("apollo voice sdk init!");
            this.m_CallApolloVoiceSDK.Init();
        }
    }

    public IApolloVoice CallApolloVoiceSDK
    {
        get
        {
            return this.m_CallApolloVoiceSDK;
        }
    }
}

