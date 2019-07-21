using WizardAdventure.Spells;

namespace WizardAdventure.Items
{
    /// <summary>
    /// Swiftness potion aplies temporary speed buff to
    /// it's drinker.
    /// </summary>
    public class SwiftnessPotion : BuffItem
    {
    #region [Properties]
        
    #endregion

    #region [Protected Methods]

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeItem()
        {
            base.InitializeItem();
            this.Name = "Greater Mana Potion";
            this.Description = "asdasds";
            this.Lore = "asdasdsad";

            this.Buff = null;
            //this.Buff = new Speed();
            this.Cooldown = 2f;
            
            this.Id = "ca48495a75b9468fa770bdc39a42dfe7";
        }
    #endregion
    }
}