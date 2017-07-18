﻿namespace Assets.Scripts.GameSystem
{
    using Assets.Scripts.Framework;
    using Assets.Scripts.GameLogic;
    using Assets.Scripts.UI;
    using ResData;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class HeroInfoHud
    {
        private Text _adTxt;
        private Text _apTxt;
        private Text _assistTxt;
        private Text _deadTxt;
        private Image _epImg;
        private Image[] _equipIcons;
        private Image _headImg;
        private Image _hpImg;
        private Text _hpTxt;
        private Text _killTxt;
        private Text _levelTxt;
        private Text _mgcDefTxt;
        private Text _moneyTxt;
        private Text _phyDefTxt;
        private HeroKDA _pickedHero;
        private Text _reviveTxt;
        private GameObject _root;
        private CBattleShowBuffDesc _showBuffDesc;
        private SkillInfoHud _skillInfoHud;

        public HeroInfoHud(GameObject root)
        {
            this._root = root;
            this._pickedHero = null;
            this._headImg = Utility.GetComponetInChild<Image>(root, "HeadIcon");
            this._reviveTxt = Utility.GetComponetInChild<Text>(root, "HeadRevive");
            this._levelTxt = Utility.GetComponetInChild<Text>(root, "HeadLevel");
            this._hpTxt = Utility.GetComponetInChild<Text>(root, "HpTxt");
            this._adTxt = Utility.GetComponetInChild<Text>(root, "AdTxt");
            this._apTxt = Utility.GetComponetInChild<Text>(root, "ApTxt");
            this._phyDefTxt = Utility.GetComponetInChild<Text>(root, "PhyDefTxt");
            this._mgcDefTxt = Utility.GetComponetInChild<Text>(root, "MgcDefTxt");
            this._hpImg = Utility.GetComponetInChild<Image>(root, "HpImg");
            this._epImg = Utility.GetComponetInChild<Image>(root, "EpImg");
            this._killTxt = Utility.GetComponetInChild<Text>(root, "StatInfo/Kill");
            this._deadTxt = Utility.GetComponetInChild<Text>(root, "StatInfo/Dead");
            this._assistTxt = Utility.GetComponetInChild<Text>(root, "StatInfo/Assist");
            this._moneyTxt = Utility.GetComponetInChild<Text>(root, "Money");
            this._equipIcons = new Image[6];
            for (int i = 0; i < 6; i++)
            {
                this._equipIcons[i] = Utility.GetComponetInChild<Image>(root, "EquipInfo/Grid_" + i);
            }
            this._showBuffDesc = new CBattleShowBuffDesc();
            this._showBuffDesc.Init(Utility.FindChild(root, "BuffInfo"));
            this._skillInfoHud = new SkillInfoHud(Utility.FindChild(root, "SkillInfo"));
        }

        public void Clear()
        {
            if (this._showBuffDesc != null)
            {
                this._showBuffDesc.UnInit();
                this._showBuffDesc = null;
            }
            this._skillInfoHud = null;
        }

        public void LateUpdate()
        {
            this.ValidateReviceCD();
        }

        public void SetPickHero(HeroKDA hero)
        {
            if ((hero != null) && (this._pickedHero != hero))
            {
                this._pickedHero = hero;
                this._headImg.SetSprite(string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_BustCircle_Dir, CSkinInfo.GetHeroSkinPic((uint) this._pickedHero.actorHero.handle.TheActorMeta.ConfigId, 0)), Singleton<CBattleSystem>.GetInstance().WatchFormScript, true, false, false, false);
                if (this._showBuffDesc != null)
                {
                    this._showBuffDesc.SwitchActor(ref this._pickedHero.actorHero);
                }
                if (this._skillInfoHud != null)
                {
                    this._skillInfoHud.SwitchActor(ref this._pickedHero.actorHero);
                }
                this.ValidateLevel();
                this.ValidateHp();
                this.ValidateEp();
                this.ValidateAd();
                this.ValidateAp();
                this.ValidatePhyDef();
                this.ValidateMgcDef();
                this.ValidateMoney();
                this.ValidateKDA();
                this.ValidateEquip();
                this.ValidateReviceCD();
            }
        }

        public void UpdateLogic(int delta)
        {
            if (this._showBuffDesc != null)
            {
                this._showBuffDesc.UpdateBuffCD(delta);
            }
        }

        public void ValidateAd()
        {
            if (this._pickedHero != null)
            {
                this._adTxt.set_text(this._pickedHero.actorHero.handle.ValueComponent.mActorValue[RES_FUNCEFT_TYPE.RES_FUNCEFT_PHYATKPT].totalValue.ToString());
            }
        }

        public void ValidateAp()
        {
            if (this._pickedHero != null)
            {
                this._apTxt.set_text(this._pickedHero.actorHero.handle.ValueComponent.mActorValue[RES_FUNCEFT_TYPE.RES_FUNCEFT_MGCATKPT].totalValue.ToString());
            }
        }

        public void ValidateEp()
        {
            if (this._pickedHero != null)
            {
                int actorEp = this._pickedHero.actorHero.handle.ValueComponent.actorEp;
                int totalValue = this._pickedHero.actorHero.handle.ValueComponent.mActorValue[RES_FUNCEFT_TYPE.RES_PROPERTY_MAXEP].totalValue;
                this._epImg.CustomFillAmount(((float) actorEp) / ((float) totalValue));
            }
        }

        public void ValidateEquip()
        {
            stEquipInfo[] equips = this._pickedHero.actorHero.handle.EquipComponent.GetEquips();
            for (int i = 0; i < 6; i++)
            {
                Image image = this._equipIcons[i];
                ushort equipID = equips[i].m_equipID;
                bool flag = false;
                if (equipID > 0)
                {
                    ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint) equipID);
                    if (dataByKey != null)
                    {
                        string prefabPath = string.Format("{0}{1}", CUIUtility.s_Sprite_System_BattleEquip_Dir, dataByKey.szIcon);
                        CUIUtility.SetImageSprite(image, prefabPath, Singleton<CBattleSystem>.GetInstance().WatchFormScript, true, false, false, false);
                        flag = true;
                    }
                }
                if (!flag)
                {
                    image.SetSprite(string.Format("{0}EquipmentSpace", CUIUtility.s_Sprite_Dynamic_Talent_Dir), Singleton<CBattleSystem>.GetInstance().WatchFormScript, true, false, false, false);
                }
            }
        }

        public void ValidateHp()
        {
            if (this._pickedHero != null)
            {
                int actorHp = this._pickedHero.actorHero.handle.ValueComponent.actorHp;
                int totalValue = this._pickedHero.actorHero.handle.ValueComponent.mActorValue[RES_FUNCEFT_TYPE.RES_FUNCEFT_MAXHP].totalValue;
                this._hpImg.CustomFillAmount(((float) actorHp) / ((float) totalValue));
                this._hpTxt.set_text(string.Format("{0}/{1}", actorHp, totalValue));
            }
        }

        public void ValidateKDA()
        {
            if (this._pickedHero != null)
            {
                this._killTxt.set_text(this._pickedHero.numKill.ToString());
                this._deadTxt.set_text(this._pickedHero.numDead.ToString());
                this._assistTxt.set_text(this._pickedHero.numAssist.ToString());
            }
        }

        public void ValidateLevel()
        {
            if (this._pickedHero != null)
            {
                this._levelTxt.set_text(this._pickedHero.actorHero.handle.ValueComponent.actorSoulLevel.ToString());
            }
        }

        public void ValidateMgcDef()
        {
            if (this._pickedHero != null)
            {
                this._mgcDefTxt.set_text(this._pickedHero.actorHero.handle.ValueComponent.mActorValue[RES_FUNCEFT_TYPE.RES_FUNCEFT_MGCDEFPT].totalValue.ToString());
            }
        }

        public void ValidateMoney()
        {
            if (this._pickedHero != null)
            {
                this._moneyTxt.set_text(this._pickedHero.TotalCoin.ToString());
            }
        }

        public void ValidatePhyDef()
        {
            if (this._pickedHero != null)
            {
                this._phyDefTxt.set_text(this._pickedHero.actorHero.handle.ValueComponent.mActorValue[RES_FUNCEFT_TYPE.RES_FUNCEFT_PHYDEFPT].totalValue.ToString());
            }
        }

        public void ValidateReviceCD()
        {
            if (((this._pickedHero != null) && (this._pickedHero.actorHero != 0)) && this._pickedHero.actorHero.handle.ActorControl.IsDeadState)
            {
                this._reviveTxt.set_text(string.Format("{0}", Mathf.FloorToInt(this._pickedHero.actorHero.handle.ActorControl.ReviveCooldown * 0.001f)));
                this._headImg.set_color(CUIUtility.s_Color_Grey);
            }
            else
            {
                this._reviveTxt.set_text(string.Empty);
                this._headImg.set_color(CUIUtility.s_Color_Full);
            }
        }

        public uint PickedHeroID
        {
            get
            {
                return ((this._pickedHero == null) ? 0 : this._pickedHero.actorHero.handle.ObjID);
            }
        }

        public GameObject Root
        {
            get
            {
                return this._root;
            }
        }

        public SkillInfoHud TheSkillHud
        {
            get
            {
                return this._skillInfoHud;
            }
        }
    }
}

