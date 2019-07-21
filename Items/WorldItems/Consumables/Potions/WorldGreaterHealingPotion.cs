using WizardAdventure.Effects;

namespace WizardAdventure.Items
{
    public class WorldGreaterHealingPotion : WorldHealingItem
    {
    #region [Properties]
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        protected LightEffects LightEffect { get; set; }
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
            this.Id = "8ef780361b64437cbbe559c593f7c3de";
            this.LightEffect = this.GetComponentInChildren<LightEffects>();
        }
    #endregion

    }
}