﻿using System;

public class PlaylistItem : IDisposable
{
    protected bool swigCMemOwn;
    private IntPtr swigCPtr;

    public PlaylistItem() : this(AkSoundEnginePINVOKE.CSharp_new_PlaylistItem__SWIG_0(), true)
    {
    }

    public PlaylistItem(PlaylistItem in_rCopy) : this(AkSoundEnginePINVOKE.CSharp_new_PlaylistItem__SWIG_1(getCPtr(in_rCopy)), true)
    {
    }

    internal PlaylistItem(IntPtr cPtr, bool cMemoryOwn)
    {
        this.swigCMemOwn = cMemoryOwn;
        this.swigCPtr = cPtr;
    }

    public PlaylistItem Assign(PlaylistItem in_rCopy)
    {
        return new PlaylistItem(AkSoundEnginePINVOKE.CSharp_PlaylistItem_Assign(this.swigCPtr, getCPtr(in_rCopy)), false);
    }

    public virtual void Dispose()
    {
        PlaylistItem item = this;
        lock (item)
        {
            if (this.swigCPtr != IntPtr.Zero)
            {
                if (this.swigCMemOwn)
                {
                    this.swigCMemOwn = false;
                    AkSoundEnginePINVOKE.CSharp_delete_PlaylistItem(this.swigCPtr);
                }
                this.swigCPtr = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }
    }

    ~PlaylistItem()
    {
        this.Dispose();
    }

    internal static IntPtr getCPtr(PlaylistItem obj)
    {
        return ((obj != null) ? obj.swigCPtr : IntPtr.Zero);
    }

    public bool IsEqualTo(PlaylistItem in_rCopy)
    {
        return AkSoundEnginePINVOKE.CSharp_PlaylistItem_IsEqualTo(this.swigCPtr, getCPtr(in_rCopy));
    }

    public AKRESULT SetExternalSources(uint in_nExternalSrc, AkExternalSourceInfo in_pExternalSrc)
    {
        return (AKRESULT) AkSoundEnginePINVOKE.CSharp_PlaylistItem_SetExternalSources(this.swigCPtr, in_nExternalSrc, AkExternalSourceInfo.getCPtr(in_pExternalSrc));
    }

    public uint audioNodeID
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_PlaylistItem_audioNodeID_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_PlaylistItem_audioNodeID_set(this.swigCPtr, value);
        }
    }

    public int msDelay
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_PlaylistItem_msDelay_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_PlaylistItem_msDelay_set(this.swigCPtr, value);
        }
    }

    public IntPtr pCustomInfo
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_PlaylistItem_pCustomInfo_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_PlaylistItem_pCustomInfo_set(this.swigCPtr, value);
        }
    }
}

