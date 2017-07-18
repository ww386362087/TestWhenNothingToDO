﻿namespace AGE
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class ActionHelperStorage
    {
        public string actionName = string.Empty;
        public bool autoPlay = true;
        public string detectStatePath = string.Empty;
        private int detectStatePathHash;
        public string helperName = string.Empty;
        private Action lastAction;
        private int lastActionFrame = -1;
        public bool playOnStart;
        public bool stopConflictActions;
        public GameObject[] targets = new GameObject[0];
        public bool waitForEvents;

        public int GetDetectStatePathHash()
        {
            if (this.detectStatePathHash == 0)
            {
                this.detectStatePathHash = Animator.StringToHash(this.detectStatePath);
            }
            return this.detectStatePathHash;
        }

        public bool IsLastActionActive()
        {
            if (!ActionManager.Instance.IsActionValid(this.lastAction))
            {
                this.lastAction = null;
            }
            return (this.lastAction != null);
        }

        public Action PlayAction()
        {
            if (Time.get_frameCount() <= (this.lastActionFrame + 1))
            {
                return null;
            }
            this.lastAction = ActionManager.Instance.PlayAction(this.actionName, this.autoPlay, this.stopConflictActions, this.targets);
            this.lastActionFrame = Time.get_frameCount();
            return this.lastAction;
        }

        public Action PlayAction(DictionaryView<string, GameObject> dictionary)
        {
            Action action = ActionManager.Instance.LoadActionResource(this.actionName);
            if (action == null)
            {
                return null;
            }
            GameObject[] objArray = (GameObject[]) this.targets.Clone();
            foreach (KeyValuePair<string, GameObject> pair in dictionary)
            {
                int num = -1;
                if (action.TemplateObjectIds.TryGetValue(pair.Key, out num))
                {
                    objArray[num] = pair.Value;
                }
            }
            if (Time.get_frameCount() <= (this.lastActionFrame + 1))
            {
                return null;
            }
            this.lastAction = ActionManager.Instance.PlayAction(this.actionName, this.autoPlay, this.stopConflictActions, objArray);
            this.lastActionFrame = Time.get_frameCount();
            return this.lastAction;
        }

        public Action PlayActionEx(params GameObject[] _gameObjects)
        {
            if (Time.get_frameCount() <= (this.lastActionFrame + 1))
            {
                return null;
            }
            this.lastAction = ActionManager.Instance.PlayAction(this.actionName, this.autoPlay, this.stopConflictActions, _gameObjects);
            this.lastActionFrame = Time.get_frameCount();
            return this.lastAction;
        }

        public void StopLastAction()
        {
            if (this.lastAction != null)
            {
                if (ActionManager.Instance.IsActionValid(this.lastAction))
                {
                    this.lastAction.Stop(false);
                }
                this.lastAction = null;
            }
        }
    }
}

