using UnityEngine;
using System;
using System.Collections;
using WizardAdventure.Effects;

namespace WizardAdventure.Items
{
    public class WorldHealingPotion : WorldHealingItem
    {
    #region [Properties]
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public LightEffects LightEffect { get; private set; }
    #endregion

    #region [Unity API]
        
        private void Start()
        {
            this.LightEffect.BeginGlow(1.5f, 0.02f, 0.01f);
        }

    #endregion

    #region [Protected Methods]
        protected override void InitializeItem()
        {
            base.InitializeItem();
            this.Cooldown = 5;
            this.Description = "asd";
            this.HealthIncrease = 15;
            this.Id = 666;
            this.LightEffect = this.GetComponentInChildren<LightEffects>();
            this.Lore = "Cool shit";
            this.Name = "Greater healing potion";
        }
    #endregion

    }
}