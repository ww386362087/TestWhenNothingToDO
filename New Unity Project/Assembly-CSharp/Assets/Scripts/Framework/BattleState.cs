﻿namespace Assets.Scripts.Framework
{
    using Assets.Scripts.GameLogic;
    using Assets.Scripts.GameSystem;
    using Assets.Scripts.Sound;
    using System;
    using UnityEngine;

    [GameState]
    public class BattleState : BaseState
    {
        private BlendWeights m_originalBlendWeight;

        public override void OnStateEnter()
        {
            this.m_originalBlendWeight = QualitySettings.get_blendWeights();
            if (GameSettings.RenderQuality == SGameRenderQuality.Low)
            {
                QualitySettings.set_blendWeights(1);
            }
            else
            {
                QualitySettings.set_blendWeights(2);
            }
            SLevelContext curLvelContext = Singleton<BattleLogic>.instance.GetCurLvelContext();
            string eventName = ((curLvelContext == null) || string.IsNullOrEmpty(curLvelContext.m_musicStartEvent)) ? "PVP01_Play" : curLvelContext.m_musicStartEvent;
            Singleton<CSoundManager>.GetInstance().PostEvent(eventName, null);
            string str2 = (curLvelContext == null) ? string.Empty : curLvelContext.m_ambientSoundEvent;
            if (!string.IsNullOrEmpty(str2))
            {
                Singleton<CSoundManager>.instance.PostEvent(str2, null);
            }
            CUICommonSystem.OpenFps();
            Singleton<CUIParticleSystem>.GetInstance().Open();
            CResourceManager.isBattleState = true;
            switch (Singleton<CNewbieAchieveSys>.GetInstance().trackFlag)
            {
                case CNewbieAchieveSys.TrackFlag.SINGLE_COMBAT_3V3_ENTER:
                    MonoSingleton<NewbieGuideManager>.GetInstance().SetNewbieBit(10, true, false);
                    break;

                case CNewbieAchieveSys.TrackFlag.SINGLE_MATCH_3V3_ENTER:
                    MonoSingleton<NewbieGuideManager>.GetInstance().SetNewbieBit(11, true, false);
                    break;

                case CNewbieAchieveSys.TrackFlag.PVE_1_1_1_Enter:
                    MonoSingleton<NewbieGuideManager>.GetInstance().SetNewbieBit(13, true, false);
                    break;
            }
            if (curLvelContext.IsMobaModeWithOutGuide())
            {
                Singleton<CPlayerPvpHistoryController>.instance.StartBattle();
            }
            MonoSingleton<PandroaSys>.GetInstance().PausePandoraSys(true);
        }

        public override void OnStateLeave()
        {
            Singleton<CRecordUseSDK>.instance.OnBadGameEnd();
            MonoSingleton<PandroaSys>.GetInstance().PausePandoraSys(false);
            QualitySettings.set_blendWeights(this.m_originalBlendWeight);
            CResourceManager.isBattleState = false;
            SLevelContext curLvelContext = Singleton<BattleLogic>.instance.GetCurLvelContext();
            string eventName = ((curLvelContext == null) || string.IsNullOrEmpty(curLvelContext.m_musicEndEvent)) ? "PVP01_Stop" : curLvelContext.m_musicEndEvent;
            Singleton<CSoundManager>.GetInstance().PostEvent(eventName, null);
            string[] exceptFormNames = new string[] { CSettleSystem.PATH_PVP_SETTLE_PVP, SettlementSystem.SettlementFormName, PVESettleSys.PATH_LOSE };
            Singleton<CUIManager>.GetInstance().CloseAllForm(exceptFormNames, true, true);
            MonoSingleton<ShareSys>.instance.m_bShowTimeline = false;
            VCollisionShape.ClearCache();
            Singleton<CGameObjectPool>.GetInstance().ClearPooledObjects();
            enResourceType[] resourceTypes = new enResourceType[5];
            resourceTypes[1] = enResourceType.UI3DImage;
            resourceTypes[2] = enResourceType.UIForm;
            resourceTypes[3] = enResourceType.UIPrefab;
            resourceTypes[4] = enResourceType.UISprite;
            Singleton<CResourceManager>.GetInstance().RemoveCachedResources(resourceTypes);
            Singleton<CResourceManager>.GetInstance().UnloadUnusedAssets();
            Singleton<CSoundManager>.GetInstance().UnloadBanks(CSoundManager.BankType.Battle);
            Singleton<GameDataMgr>.GetInstance().UnloadReducedDatabin();
        }
    }
}

