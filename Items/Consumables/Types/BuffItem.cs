using WizardAdventure.Spells;

namespace WizardAdventure.Items
{
    /// <summary>
    /// Buff items are used to add buffs
    /// to unit for a limited time.
    /// </summary>
    public class BuffItem : Consumable
    {
    #region [Properties]
        public Buff Buff { get; set; }
    #endregion

    }
}