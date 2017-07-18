﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AkGameObj)), AddComponentMenu("Wwise/AkAmbient")]
public class AkAmbient : AkEvent
{
    [CompilerGenerated]
    private AkAmbient <ParentAkAmbience>k__BackingField;
    public static DictionaryView<int, AkMultiPosEvent> multiPosEventTree = new DictionaryView<int, AkMultiPosEvent>();
    public List<Vector3> multiPositionArray = new List<Vector3>();
    public MultiPositionTypeLabel multiPositionTypeLabel;

    private AkPositionArray BuildAkPositionArray()
    {
        AkPositionArray array = new AkPositionArray((uint) this.multiPositionArray.Count);
        for (int i = 0; i < this.multiPositionArray.Count; i++)
        {
            array.Add(base.get_transform().get_position() + this.multiPositionArray[i], base.get_transform().get_forward());
        }
        return array;
    }

    public AkPositionArray BuildMultiDirectionArray(ref AkMultiPosEvent eventPosList)
    {
        AkPositionArray array = new AkPositionArray((uint) eventPosList.list.Count);
        for (int i = 0; i < eventPosList.list.Count; i++)
        {
            array.Add(eventPosList.list[i].get_transform().get_position(), eventPosList.list[i].get_transform().get_forward());
        }
        return array;
    }

    public override void HandleEvent(GameObject in_gameObject)
    {
        if (this.multiPositionTypeLabel != MultiPositionTypeLabel.MultiPosition_Mode)
        {
            base.HandleEvent(in_gameObject);
        }
        else
        {
            AkMultiPosEvent event2 = multiPosEventTree[base.eventID];
            if (!event2.eventIsPlaying)
            {
                event2.eventIsPlaying = true;
                base.soundEmitterObject = event2.list[0].get_gameObject();
                if (base.enableActionOnEvent)
                {
                    AkSoundEngine.ExecuteActionOnEvent((uint) base.eventID, base.actionOnEventType, event2.list[0].get_gameObject(), ((int) base.transitionDuration) * 0x3e8, base.curveInterpolation);
                }
                else
                {
                    AkSoundEngine.PostEvent((uint) base.eventID, event2.list[0].get_gameObject(), 1, new AkCallbackManager.EventCallback(event2.FinishedPlaying), null, 0, null, 0);
                }
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (AkSoundEngine.IsInitialized())
        {
            AkSoundEngine.UnregisterGameObj(base.get_gameObject());
        }
    }

    private void OnDisable()
    {
        if (this.multiPositionTypeLabel == MultiPositionTypeLabel.MultiPosition_Mode)
        {
            AkMultiPosEvent eventPosList = multiPosEventTree[base.eventID];
            if (eventPosList.list.Count == 1)
            {
                multiPosEventTree.Remove(base.eventID);
            }
            else
            {
                eventPosList.list.Remove(this);
                AkPositionArray array = this.BuildMultiDirectionArray(ref eventPosList);
                AkSoundEngine.SetMultiplePositions(eventPosList.list[0].get_gameObject(), array, (ushort) array.Count, MultiPositionType.MultiPositionType_MultiSources);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawIcon(base.get_transform().get_position(), "WwiseAudioSpeaker.png", false);
    }

    private void OnEnable()
    {
        if (this.multiPositionTypeLabel == MultiPositionTypeLabel.Simple_Mode)
        {
            AkGameObj[] components = base.get_gameObject().GetComponents<AkGameObj>();
            for (int i = 0; i < components.Length; i++)
            {
                components[i].set_enabled(true);
            }
        }
        else if (this.multiPositionTypeLabel == MultiPositionTypeLabel.Large_Mode)
        {
            AkGameObj[] objArray2 = base.get_gameObject().GetComponents<AkGameObj>();
            for (int j = 0; j < objArray2.Length; j++)
            {
                objArray2[j].set_enabled(false);
            }
            AkPositionArray array = this.BuildAkPositionArray();
            AkSoundEngine.SetMultiplePositions(base.get_gameObject(), array, (ushort) array.Count, MultiPositionType.MultiPositionType_MultiSources);
        }
        else if (this.multiPositionTypeLabel == MultiPositionTypeLabel.MultiPosition_Mode)
        {
            AkMultiPosEvent event2;
            AkGameObj[] objArray3 = base.get_gameObject().GetComponents<AkGameObj>();
            for (int k = 0; k < objArray3.Length; k++)
            {
                objArray3[k].set_enabled(false);
            }
            if (multiPosEventTree.TryGetValue(base.eventID, out event2))
            {
                if (!event2.list.Contains(this))
                {
                    event2.list.Add(this);
                }
            }
            else
            {
                event2 = new AkMultiPosEvent();
                event2.list.Add(this);
                multiPosEventTree.Add(base.eventID, event2);
            }
            AkPositionArray array2 = this.BuildMultiDirectionArray(ref event2);
            AkSoundEngine.SetMultiplePositions(event2.list[0].get_gameObject(), array2, (ushort) array2.Count, MultiPositionType.MultiPositionType_MultiSources);
        }
    }

    public AkAmbient ParentAkAmbience
    {
        [CompilerGenerated]
        get
        {
            return this.<ParentAkAmbience>k__BackingField;
        }
        [CompilerGenerated]
        set
        {
            this.<ParentAkAmbience>k__BackingField = value;
        }
    }
}

