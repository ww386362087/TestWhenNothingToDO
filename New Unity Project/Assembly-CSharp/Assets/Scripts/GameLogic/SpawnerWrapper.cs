﻿namespace Assets.Scripts.GameLogic
{
    using Assets.Scripts.Framework;
    using Assets.Scripts.GameLogic.DataCenter;
    using ResData;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class SpawnerWrapper
    {
        [FriendlyName("Meta信息非随机")]
        public bool bSequentialMeta;
        [FriendlyName("生成对象配置ID")]
        public int ConfigId;
        public int[] InitBuffDemand;
        [FriendlyName("随机被动技能规则")]
        public int InitRandPassSkillRule;
        private SpawnerBase m_internalSpawner;
        public GameObject m_rangeDeadPoint;
        public GeoPolygon m_rangePolygon;
        public ESpawnObjectType SpawnType;
        public STriggerCondActor[] SrcActorCond;
        public ActorMeta TheActorMeta;

        public SpawnerWrapper()
        {
            this.InitBuffDemand = new int[0];
        }

        public SpawnerWrapper(ESpawnObjectType inSpawnType)
        {
            this.InitBuffDemand = new int[0];
            this.SpawnType = inSpawnType;
        }

        public void Destroy()
        {
            if (this.m_internalSpawner != null)
            {
                this.m_internalSpawner.Destroy();
                this.m_internalSpawner = null;
            }
        }

        public object DoSpawn(VInt3 inWorldPos, VInt3 inDir, GameObject inSpawnPoint)
        {
            if (this.m_internalSpawner != null)
            {
                return this.m_internalSpawner.DoSpawn(inWorldPos, inDir, inSpawnPoint);
            }
            return null;
        }

        public SpawnerBase GetActionInternal()
        {
            return this.m_internalSpawner;
        }

        public void Init()
        {
            if (this.m_internalSpawner == null)
            {
                switch (this.SpawnType)
                {
                    case ESpawnObjectType.Tailsman:
                    {
                        SpawnerTailsman tailsman = new SpawnerTailsman(this);
                        tailsman.TailsmanId = this.ConfigId;
                        tailsman.SrcActorCond = this.SrcActorCond;
                        this.m_internalSpawner = tailsman;
                        break;
                    }
                    case ESpawnObjectType.Actor:
                    {
                        SpawnerActor actor = new SpawnerActor(this);
                        actor.TheActorMeta = this.TheActorMeta;
                        actor.bSequentialMeta = this.bSequentialMeta;
                        actor.InitRandPassSkillRule = this.InitRandPassSkillRule;
                        actor.InitBuffDemand = this.InitBuffDemand;
                        actor.m_rangePolygon = this.m_rangePolygon;
                        actor.m_rangeDeadPoint = this.m_rangeDeadPoint;
                        this.m_internalSpawner = actor;
                        return;
                    }
                }
            }
        }

        public void PreLoadResource(ref List<ActorPreloadTab> list, LoaderHelper loadHelper)
        {
            if ((this.SpawnType == ESpawnObjectType.Actor) && (this.TheActorMeta.ConfigId > 0))
            {
                loadHelper.AddPreloadActor(ref list, ref this.TheActorMeta, 1f, 0);
            }
        }

        public void PreLoadResource(ref ActorPreloadTab loadInfo, LoaderHelper loadHelper)
        {
            if (this.SpawnType == ESpawnObjectType.Tailsman)
            {
                CharmLib dataByKey = GameDataMgr.charmLib.GetDataByKey((long) this.ConfigId);
                if (dataByKey != null)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (dataByKey.astCharmId[i].iParam == 0)
                        {
                            break;
                        }
                        int iParam = dataByKey.astCharmId[i].iParam;
                        ShenFuInfo info = GameDataMgr.shenfuBin.GetDataByKey((long) iParam);
                        if (info != null)
                        {
                            AssetLoadBase base3 = new AssetLoadBase();
                            base3.assetPath = StringHelper.UTF8BytesToString(ref info.szShenFuResPath);
                            AssetLoadBase item = base3;
                            loadInfo.mesPrefabs.Add(item);
                            loadHelper.AnalyseSkillCombine(ref loadInfo, info.iBufId);
                        }
                    }
                }
            }
        }

        public enum ESpawnObjectType
        {
            Tailsman,
            Actor,
            Invalid
        }
    }
}

