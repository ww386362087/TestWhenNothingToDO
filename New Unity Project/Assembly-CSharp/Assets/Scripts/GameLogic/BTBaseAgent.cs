﻿namespace Assets.Scripts.GameLogic
{
    using Assets.Scripts.Framework;
    using behaviac;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [TypeMetaInfo("BaseAgent", "游戏使用的一切行为树代理从该类继承")]
    public class BTBaseAgent : Agent, IUpdateLogic
    {
        public string m_AgentFileName = string.Empty;
        private List<string> m_globalEventFiredInThisFrame = new List<string>();
        protected bool m_isPaused;

        private void Awake()
        {
            base.Init();
        }

        private void ClearEventFiredLastFrame()
        {
            if (this.m_globalEventFiredInThisFrame.Count > 0)
            {
                for (int i = 0; i < this.m_globalEventFiredInThisFrame.Count; i++)
                {
                }
                this.m_globalEventFiredInThisFrame.Clear();
            }
        }

        [MethodMetaInfo]
        public EBTStatus FireAIEvent(string eventName)
        {
            this.m_globalEventFiredInThisFrame.Add(eventName);
            return EBTStatus.BT_SUCCESS;
        }

        [MethodMetaInfo("取模", "取模")]
        public int GetMod(uint numA, int numB)
        {
            return (int) (((ulong) numA) % ((long) numB));
        }

        [MethodMetaInfo("获取一个随机整数", "获取一个随机整数")]
        public int GetRandomInt(uint maxNum)
        {
            return FrameRandom.Random(maxNum);
        }

        [MethodMetaInfo("获取Vector3的y值", "获取Vector3的y值")]
        public float GetVector3Y(Vector3 par)
        {
            return par.y;
        }

        [MethodMetaInfo]
        public bool HasAIEventFired(string eventName)
        {
            return false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.ClearEventFiredLastFrame();
        }

        [MethodMetaInfo]
        public EBTStatus SelfDestroy(bool bDestroyGameObject)
        {
            if (bDestroyGameObject)
            {
                Object.Destroy(base.get_gameObject());
            }
            else
            {
                Object.Destroy(this);
            }
            return EBTStatus.BT_SUCCESS;
        }

        public void SetCurAgentActive()
        {
            if ((this.m_AgentFileName != null) && (this.m_AgentFileName.Length > 0))
            {
                base.SetIdFlag(1);
                Agent.SetIdMask(uint.MaxValue);
                base.btload(this.m_AgentFileName);
                base.btsetcurrent(this.m_AgentFileName);
            }
        }

        public virtual void SetPaused(bool isPaused)
        {
            this.m_isPaused = isPaused;
        }

        private void Start()
        {
        }

        private void Update()
        {
        }

        public virtual void UpdateLogic(int delta)
        {
            if (base.btgetcurrent() != null)
            {
                this.ClearEventFiredLastFrame();
            }
            if ((base.btgetcurrent() != null) && (this.btexec() != EBTStatus.BT_RUNNING))
            {
                base.btsetcurrent(this.m_AgentFileName);
            }
        }

        public bool bPaused
        {
            get
            {
                return this.m_isPaused;
            }
        }
    }
}

