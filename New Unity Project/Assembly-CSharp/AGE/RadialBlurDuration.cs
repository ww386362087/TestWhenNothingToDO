﻿namespace AGE
{
    using Assets.Scripts.Common;
    using Assets.Scripts.Framework;
    using System;
    using UnityEngine;

    public class RadialBlurDuration : DurationEvent
    {
        public float blurScale = 50f;
        private Camera[] cameras;
        public float falloffExp = 1.5f;

        public override BaseEvent Clone()
        {
            RadialBlurDuration duration = ClassObjPool<RadialBlurDuration>.Get();
            duration.CopyData(this);
            return duration;
        }

        protected override void CopyData(BaseEvent src)
        {
            base.CopyData(src);
            RadialBlurDuration duration = src as RadialBlurDuration;
            this.falloffExp = duration.falloffExp;
            this.blurScale = duration.blurScale;
        }

        public override void Enter(Action _action, Track _track)
        {
            if (GameSettings.AllowRadialBlur)
            {
                this.cameras = Object.FindObjectsOfType<Camera>();
                if (this.cameras != null)
                {
                    string[] textArray1 = new string[] { "Scene" };
                    int mask = LayerMask.GetMask(textArray1);
                    for (int i = 0; i < this.cameras.Length; i++)
                    {
                        Camera camera = this.cameras[i];
                        if ((camera.get_cullingMask() & mask) != 0)
                        {
                            RadialBlur component = camera.GetComponent<RadialBlur>();
                            if (component == null)
                            {
                                component = camera.get_gameObject().AddComponent<RadialBlur>();
                            }
                            component.blurScale = this.blurScale;
                            component.falloffExp = this.falloffExp;
                            component.UpdateParameters();
                        }
                    }
                }
            }
        }

        public override void Leave(Action _action, Track _track)
        {
            if (this.cameras != null)
            {
                string[] textArray1 = new string[] { "Scene" };
                int mask = LayerMask.GetMask(textArray1);
                for (int i = 0; i < this.cameras.Length; i++)
                {
                    Camera camera = this.cameras[i];
                    if ((camera != null) && ((camera.get_cullingMask() & mask) != 0))
                    {
                        RadialBlur component = camera.GetComponent<RadialBlur>();
                        if (component != null)
                        {
                            Object.Destroy(component);
                        }
                    }
                }
                this.cameras = null;
            }
        }

        public override void OnUse()
        {
            base.OnUse();
            this.falloffExp = 1.5f;
            this.blurScale = 50f;
            this.cameras = null;
        }

        public override bool SupportEditMode()
        {
            return false;
        }
    }
}

