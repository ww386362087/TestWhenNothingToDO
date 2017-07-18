﻿using System;

public class Iterator : IDisposable
{
    protected bool swigCMemOwn;
    private IntPtr swigCPtr;

    public Iterator() : this(AkSoundEnginePINVOKE.CSharp_new_Iterator(), true)
    {
    }

    internal Iterator(IntPtr cPtr, bool cMemoryOwn)
    {
        this.swigCMemOwn = cMemoryOwn;
        this.swigCPtr = cPtr;
    }

    public virtual void Dispose()
    {
        Iterator iterator = this;
        lock (iterator)
        {
            if (this.swigCPtr != IntPtr.Zero)
            {
                if (this.swigCMemOwn)
                {
                    this.swigCMemOwn = false;
                    AkSoundEnginePINVOKE.CSharp_delete_Iterator(this.swigCPtr);
                }
                this.swigCPtr = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }
    }

    ~Iterator()
    {
        this.Dispose();
    }

    internal static IntPtr getCPtr(Iterator obj)
    {
        return ((obj != null) ? obj.swigCPtr : IntPtr.Zero);
    }

    public PlaylistItem GetItem()
    {
        return new PlaylistItem(AkSoundEnginePINVOKE.CSharp_Iterator_GetItem(this.swigCPtr), false);
    }

    public bool IsDifferentFrom(Iterator in_rOp)
    {
        return AkSoundEnginePINVOKE.CSharp_Iterator_IsDifferentFrom(this.swigCPtr, getCPtr(in_rOp));
    }

    public bool IsEqualTo(Iterator in_rOp)
    {
        return AkSoundEnginePINVOKE.CSharp_Iterator_IsEqualTo(this.swigCPtr, getCPtr(in_rOp));
    }

    public Iterator NextIter()
    {
        return new Iterator(AkSoundEnginePINVOKE.CSharp_Iterator_NextIter(this.swigCPtr), false);
    }

    public Iterator PrevIter()
    {
        return new Iterator(AkSoundEnginePINVOKE.CSharp_Iterator_PrevIter(this.swigCPtr), false);
    }

    public PlaylistItem pItem
    {
        get
        {
            IntPtr cPtr = AkSoundEnginePINVOKE.CSharp_Iterator_pItem_get(this.swigCPtr);
            return (!(cPtr == IntPtr.Zero) ? new PlaylistItem(cPtr, false) : null);
        }
        set
        {
            AkSoundEnginePINVOKE.CSharp_Iterator_pItem_set(this.swigCPtr, PlaylistItem.getCPtr(value));
        }
    }
}

