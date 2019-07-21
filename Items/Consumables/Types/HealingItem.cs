
namespace WizardAdventure.Items
{
    /// <summary>
    /// Healing items like potions and herbs can
    /// heal player when consumed.
    /// </summary>
    public class HealingItem : Consumable
    {
    #region [Properties]
        /// <summary>
        /// The amount this healing item heals.
        /// </summary>
        /// <value>Get and Set heal amount</value>
        public int HealthIncrease { get; protected set; }
    #endregion

    }
}