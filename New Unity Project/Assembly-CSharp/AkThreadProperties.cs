﻿using System;

public class AkThreadProperties : IDisposable
{
    protected bool swigCMemOwn;
    private IntPtr swigCPtr;

    public AkThreadProperties() : this(AkSoundEnginePINVOKE.CSharp_new_AkThreadProperties(), true)
    {
    }

    internal AkThreadProperties(IntPtr cPtr, bool cMemoryOwn)
    {
        this.swigCMemOwn = cMemoryOwn;
        this.swigCPtr = cPtr;
    }

    public virtual void Dispose()
    {
        AkThreadProperties properties = this;
        lock (properties)
        {
            if (this.swigCPtr != IntPtr.Zero)
            {
                if (this.swigCMemOwn)
                {
                    this.swigCMemOwn = false;
                    AkSoundEnginePINVOKE.CSharp_delete_AkThreadProperties(this.swigCPtr);
                }
                this.swigCPtr = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }
    }

    ~AkThreadProperties()
    {
        this.Dispose();
    }

    internal static IntPtr getCPtr(AkThreadProperties obj)
    {
        return ((obj != null) ? obj.swigCPtr : IntPtr.Zero);
    }

    public int nPriority
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkThreadProperties_nPriority_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkThreadProperties_nPriority_set(this.swigCPtr, value);
        }
    }

    public int uSchedPolicy
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkThreadProperties_uSchedPolicy_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkThreadProperties_uSchedPolicy_set(this.swigCPtr, value);
        }
    }

    public uint uStackSize
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkThreadProperties_uStackSize_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkThreadProperties_uStackSize_set(this.swigCPtr, value);
        }
    }
}

