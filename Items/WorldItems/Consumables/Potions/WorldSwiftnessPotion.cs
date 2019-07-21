using WizardAdventure.Effects;

namespace WizardAdventure.Items
{
    public class WorldSwiftnessPotion : WorldBuffItem
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
            this.Id = "ca48495a75b9468fa770bdc39a42dfe7";
            this.LightEffect = this.GetComponentInChildren<LightEffects>();
        }

    #endregion
    }
}