
namespace WizardAdventure.Items
{
    public class GreaterHealingPotion : HealingItem
    {
    #region [Properties]
        
    #endregion

    #region [Protected Methods]
        protected override void InitializeItem()
        {
            base.InitializeItem();
            this.Name = "Greater Healing Potion";
            this.Description = "asdasds";
            this.Lore = "asdasdsad";

            this.Cooldown = 3f;
            this.HealthIncrease = 15;
            
            this.Id = "8ef780361b64437cbbe559c593f7c3de";
        }
    #endregion
    }
}