
namespace WizardAdventure.Items
{
    public class GreaterManaPotion : ManaItem
    {
    #region [Properties]
        
    #endregion

    #region [Protected Methods]
        protected override void InitializeItem()
        {
            base.InitializeItem();
            this.Name = "Greater Mana Potion";
            this.Description = "asdasds";
            this.Lore = "asdasdsad";

            this.Cooldown = 2f;
            this.ManaIncrease = 15;
            
            this.Id = "c5aab76932f645188725a768d948deb6";
        }
    #endregion
    }
}