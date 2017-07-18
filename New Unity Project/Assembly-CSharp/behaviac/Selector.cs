﻿namespace behaviac
{
    using System;
    using System.Collections.Generic;

    public class Selector : BehaviorNode
    {
        protected override BehaviorTask createTask()
        {
            return new SelectorTask();
        }

        ~Selector()
        {
        }

        public override bool IsValid(Agent pAgent, BehaviorTask pTask)
        {
            return ((pTask.GetNode() is Selector) && base.IsValid(pAgent, pTask));
        }

        protected override void load(int version, string agentType, List<property_t> properties)
        {
            base.load(version, agentType, properties);
        }

        public class SelectorTask : CompositeTask
        {
            public override void copyto(BehaviorTask target)
            {
                base.copyto(target);
            }

            ~SelectorTask()
            {
            }

            public override void load(ISerializableNode node)
            {
                base.load(node);
            }

            protected override bool onenter(Agent pAgent)
            {
                base.m_activeChildIndex = 0;
                return true;
            }

            protected override void onexit(Agent pAgent, EBTStatus s)
            {
            }

            public override void save(ISerializableNode node)
            {
                base.save(node);
            }

            protected override EBTStatus update(Agent pAgent, EBTStatus childStatus)
            {
                bool flag = true;
                do
                {
                    EBTStatus status = childStatus;
                    if (!flag || (status == EBTStatus.BT_RUNNING))
                    {
                        status = base.m_children[base.m_activeChildIndex].exec(pAgent);
                    }
                    flag = false;
                    if (status != EBTStatus.BT_FAILURE)
                    {
                        return status;
                    }
                    base.m_activeChildIndex++;
                }
                while ((base.m_activeChildIndex < base.m_children.Count) && this.CheckPredicates(pAgent));
                return EBTStatus.BT_FAILURE;
            }
        }
    }
}

