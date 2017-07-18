﻿using System;

public class AkInitSettings : IDisposable
{
    protected bool swigCMemOwn;
    private IntPtr swigCPtr;

    public AkInitSettings() : this(AkSoundEnginePINVOKE.CSharp_new_AkInitSettings(), true)
    {
    }

    internal AkInitSettings(IntPtr cPtr, bool cMemoryOwn)
    {
        this.swigCMemOwn = cMemoryOwn;
        this.swigCPtr = cPtr;
    }

    public virtual void Dispose()
    {
        AkInitSettings settings = this;
        lock (settings)
        {
            if (this.swigCPtr != IntPtr.Zero)
            {
                if (this.swigCMemOwn)
                {
                    this.swigCMemOwn = false;
                    AkSoundEnginePINVOKE.CSharp_delete_AkInitSettings(this.swigCPtr);
                }
                this.swigCPtr = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }
    }

    ~AkInitSettings()
    {
        this.Dispose();
    }

    internal static IntPtr getCPtr(AkInitSettings obj)
    {
        return ((obj != null) ? obj.swigCPtr : IntPtr.Zero);
    }

    public bool bEnableGameSyncPreparation
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_bEnableGameSyncPreparation_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_bEnableGameSyncPreparation_set(this.swigCPtr, value);
        }
    }

    public bool bUseSoundBankMgrThread
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_bUseSoundBankMgrThread_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_bUseSoundBankMgrThread_set(this.swigCPtr, value);
        }
    }

    public AkAudioAPI eMainOutputType
    {
        get
        {
            return (AkAudioAPI) AkSoundEnginePINVOKE.CSharp_AkInitSettings_eMainOutputType_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_eMainOutputType_set(this.swigCPtr, (int) value);
        }
    }

    public float fDefaultPoolRatioThreshold
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_fDefaultPoolRatioThreshold_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_fDefaultPoolRatioThreshold_set(this.swigCPtr, value);
        }
    }

    public int pfnAssertHook
    {
        get
        {
            return 0;
        }
        set
        {
        }
    }

    public AkOutputSettings settingsMainOutput
    {
        get
        {
            IntPtr cPtr = AkSoundEnginePINVOKE.CSharp_AkInitSettings_settingsMainOutput_get(this.swigCPtr);
            return (!(cPtr == IntPtr.Zero) ? new AkOutputSettings(cPtr, false) : null);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_settingsMainOutput_set(this.swigCPtr, AkOutputSettings.getCPtr(value));
        }
    }

    public uint uCommandQueueSize
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uCommandQueueSize_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_uCommandQueueSize_set(this.swigCPtr, value);
        }
    }

    public uint uContinuousPlaybackLookAhead
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uContinuousPlaybackLookAhead_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_uContinuousPlaybackLookAhead_set(this.swigCPtr, value);
        }
    }

    public uint uDefaultPoolSize
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uDefaultPoolSize_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_uDefaultPoolSize_set(this.swigCPtr, value);
        }
    }

    public uint uMaxHardwareTimeoutMs
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxHardwareTimeoutMs_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxHardwareTimeoutMs_set(this.swigCPtr, value);
        }
    }

    public uint uMaxNumPaths
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxNumPaths_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxNumPaths_set(this.swigCPtr, value);
        }
    }

    public uint uMaxNumTransitions
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxNumTransitions_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMaxNumTransitions_set(this.swigCPtr, value);
        }
    }

    public uint uMonitorPoolSize
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorPoolSize_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorPoolSize_set(this.swigCPtr, value);
        }
    }

    public uint uMonitorQueuePoolSize
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorQueuePoolSize_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_uMonitorQueuePoolSize_set(this.swigCPtr, value);
        }
    }

    public uint uNumSamplesPerFrame
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uNumSamplesPerFrame_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_uNumSamplesPerFrame_set(this.swigCPtr, value);
        }
    }

    public int uPrepareEventMemoryPoolID
    {
        get
        {
            return AkSoundEnginePINVOKE.CSharp_AkInitSettings_uPrepareEventMemoryPoolID_get(this.swigCPtr);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_AkInitSettings_uPrepareEventMemoryPoolID_set(this.swigCPtr, value);
        }
    }
}

