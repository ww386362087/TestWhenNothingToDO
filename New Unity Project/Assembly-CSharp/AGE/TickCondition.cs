﻿namespace AGE
{
    using System;

    public abstract class TickCondition : TickEvent
    {
        protected TickCondition()
        {
        }

        public virtual bool Check(Action _action, Track _track)
        {
            return true;
        }

        public override void PostProcess(Action _action, Track _track, int _localTime)
        {
        }

        public override void Process(Action _action, Track _track)
        {
            _action.SetCondition(_track, this.Check(_action, _track));
        }

        public override void ProcessBlend(Action _action, Track _track, TickEvent _prevEvent, float _blendWeight)
        {
        }
    }
}

