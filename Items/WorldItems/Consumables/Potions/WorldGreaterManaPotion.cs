using UnityEngine;
using WizardAdventure.Effects;

namespace WizardAdventure.Items
{
    public class WorldGreaterManaPotion : WorldManaItem
    {
        #region [Properties]
        /// <summary>
        /// Light effect script
        /// </summary>
        /// <value>Get and Set light effect controller.</value>
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
            this.Id = "c5aab76932f645188725a768d948deb6";
            this.LightEffect = this.GetComponentInChildren<LightEffects>();
        }
    #endregion
    }
}