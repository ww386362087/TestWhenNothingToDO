﻿namespace Assets.Scripts.GameSystem
{
    using Assets.Scripts.Framework;
    using ResData;
    using System;

    internal class CHeroCfgData : IHeroData
    {
        private ResHeroShop _heroShopInfo;
        private ResHeroCfgInfo m_cfgInfo;
        private string m_imgPath;
        private string m_name;
        private string m_tilte;

        public CHeroCfgData(uint id)
        {
            this.m_cfgInfo = GameDataMgr.heroDatabin.GetDataByKey(id);
            GameDataMgr.heroShopInfoDict.TryGetValue(id, out this._heroShopInfo);
            if (this.m_cfgInfo == null)
            {
                object[] inParameters = new object[] { id };
                DebugHelper.Assert(false, "ResHeroCfgInfo can not find id = {0}", inParameters);
            }
            else
            {
                this.m_name = StringHelper.UTF8BytesToString(ref this.m_cfgInfo.szName);
                this.m_imgPath = StringHelper.UTF8BytesToString(ref this.m_cfgInfo.szImagePath);
                this.m_tilte = StringHelper.UTF8BytesToString(ref this.m_cfgInfo.szHeroTitle);
            }
        }

        public bool IsValidExperienceHero()
        {
            return false;
        }

        public ResHeroPromotion promotion()
        {
            if (this._heroShopInfo != null)
            {
                for (int i = 0; i < this._heroShopInfo.bPromotionCnt; i++)
                {
                    uint key = this._heroShopInfo.PromotionID[i];
                    if ((key != 0) && GameDataMgr.heroPromotionDict.ContainsKey(key))
                    {
                        ResHeroPromotion promotion = new ResHeroPromotion();
                        if ((GameDataMgr.heroPromotionDict.TryGetValue(key, out promotion) && (promotion.dwOnTimeGen <= CRoleInfo.GetCurrentUTCTime())) && (promotion.dwOffTimeGen >= CRoleInfo.GetCurrentUTCTime()))
                        {
                            return promotion;
                        }
                    }
                }
            }
            return null;
        }

        public bool bIsPlayerUse
        {
            get
            {
                return GameDataMgr.IsHeroAvailable(this.m_cfgInfo.dwCfgID);
            }
        }

        public bool bPlayerOwn
        {
            get
            {
                return false;
            }
        }

        public uint cfgID
        {
            get
            {
                if (this.m_cfgInfo != null)
                {
                    return this.m_cfgInfo.dwCfgID;
                }
                return 0;
            }
        }

        public int combatEft
        {
            get
            {
                return CHeroInfo.GetInitCombatByHeroId(this.m_cfgInfo.dwCfgID);
            }
        }

        public int curExp
        {
            get
            {
                return 0;
            }
        }

        public ResHeroCfgInfo heroCfgInfo
        {
            get
            {
                return this.m_cfgInfo;
            }
        }

        public string heroName
        {
            get
            {
                return this.m_name;
            }
        }

        public string heroTitle
        {
            get
            {
                return this.m_tilte;
            }
        }

        public int heroType
        {
            get
            {
                return this.m_cfgInfo.bMainJob;
            }
        }

        public string imagePath
        {
            get
            {
                return this.m_imgPath;
            }
        }

        public int level
        {
            get
            {
                return 0;
            }
        }

        public int maxExp
        {
            get
            {
                return 0;
            }
        }

        public uint proficiency
        {
            get
            {
                return 0;
            }
        }

        public byte proficiencyLV
        {
            get
            {
                return 0;
            }
        }

        public int quality
        {
            get
            {
                return 1;
            }
        }

        public ResDT_SkillInfo[] skillArr
        {
            get
            {
                return this.m_cfgInfo.astSkill;
            }
        }

        public uint skinID
        {
            get
            {
                return 0;
            }
        }

        public uint sortId
        {
            get
            {
                return this.m_cfgInfo.dwShowSortId;
            }
        }

        public int star
        {
            get
            {
                return this.m_cfgInfo.iInitialStar;
            }
        }

        public int subQuality
        {
            get
            {
                return 0;
            }
        }
    }
}

