﻿using Assets.Scripts.GameLogic.GameKernal;
using System;
using UnityEngine;

[AddComponentMenu("Wwise/AkAudioListener")]
public class AkAudioListener : MonoBehaviour
{
    private Plane GroundPlane;
    public int listenerId;
    [FriendlyName("死亡Offset")]
    public Vector3 m_DeadOffset;
    private Vector3 m_Front;
    private Vector3 m_FrontCache;
    private Vector3 m_Position;
    private Vector3 m_PositionCache;
    [FriendlyName("全局Offset")]
    public Vector3 m_StaticOffset;
    private Vector3 m_Top;
    private Vector3 m_TopCache;

    private void Update()
    {
        this.UpdateCache();
        if (((this.m_Position != this.m_PositionCache) || (this.m_Front != this.m_FrontCache)) || (this.m_Top != this.m_TopCache))
        {
            this.m_Position = this.m_PositionCache;
            this.m_Front = this.m_FrontCache;
            this.m_Top = this.m_TopCache;
            AkSoundEngine.SetListenerPosition(this.m_FrontCache.x, this.m_FrontCache.y, this.m_FrontCache.z, this.m_TopCache.x, this.m_TopCache.y, this.m_TopCache.z, this.m_PositionCache.x + this.m_StaticOffset.x, this.m_PositionCache.y + this.m_StaticOffset.y, this.m_PositionCache.z + this.m_StaticOffset.z, (uint) this.listenerId);
        }
    }

    private void UpdateCache()
    {
        this.m_FrontCache = base.get_transform().get_forward();
        this.m_TopCache = base.get_transform().get_up();
        this.m_PositionCache = base.get_transform().get_position();
        if (Singleton<BattleLogic>.instance.isRuning)
        {
            Player hostPlayer = Singleton<GamePlayerCenter>.instance.GetHostPlayer();
            if ((hostPlayer != null) && (hostPlayer.Captain != 0))
            {
                if (Singleton<WatchController>.GetInstance().IsWatching || hostPlayer.Captain.handle.ActorControl.IsDeadState)
                {
                    if (Camera.get_main() != null)
                    {
                        Vector3 location = (Vector3) hostPlayer.Captain.handle.location;
                        this.GroundPlane.SetNormalAndPosition(new Vector3(0f, 1f, 0f), new Vector3(0f, location.y, 0f));
                        Ray ray = Camera.get_main().ScreenPointToRay(new Vector3((float) (Screen.get_width() / 2), (float) (Screen.get_height() / 2), 0f));
                        float num = 0f;
                        if (this.GroundPlane.Raycast(ray, ref num))
                        {
                            this.m_PositionCache = ray.GetPoint(num) + (!hostPlayer.Captain.handle.ActorControl.IsDeadState ? Vector3.get_zero() : this.m_DeadOffset);
                        }
                    }
                }
                else
                {
                    this.m_PositionCache = (Vector3) hostPlayer.Captain.handle.location;
                }
            }
        }
    }
}

