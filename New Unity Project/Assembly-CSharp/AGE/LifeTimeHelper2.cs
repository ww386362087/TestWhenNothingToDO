﻿namespace AGE
{
    using System;
    using UnityEngine;

    internal class LifeTimeHelper2 : MonoBehaviour
    {
        public ParticleSystem[] particleSys;

        private void Update()
        {
            if (this.particleSys == null)
            {
                Object.Destroy(this);
            }
            else
            {
                for (int i = 0; i < this.particleSys.Length; i++)
                {
                    ParticleSystem system = this.particleSys[i];
                    if (((system != null) && !system.get_isStopped()) && system.IsAlive())
                    {
                        return;
                    }
                }
                ActionManager.DestroyGameObject(base.get_gameObject());
            }
        }
    }
}

