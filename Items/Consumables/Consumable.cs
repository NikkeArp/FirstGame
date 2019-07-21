
namespace WizardAdventure.Items
{
    /// <summary>
    /// Consumable items can be consumed to heal for example.
    /// All consumable items have time cooldown on their usage.
    /// </summary>
    public class Consumable : Item
    {
    #region [Properties]
        /// <summary>
        /// Consumable items have cooldowns
        /// to prevent item spamming.
        /// </summary>
        /// <value>Get and Set consumable cooldown.</value>
        public float Cooldown { get; protected set; }
    #endregion

    #region [Protected Methods]
        protected virtual void ConsumeItem()
        {

        }
    #endregion

    }
}