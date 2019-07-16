namespace WizardAdventure.Spells
{
    /// <summary>
    /// Abstract damage spell class. Inplements IDamageSource
    /// </summary>
    public abstract class DamageSpell : Spell, IDamageSource
    {
    #region [Properties]
        public float Damage { get; protected set; }   
    #endregion
        
        /// <summary>
        /// Get damage amount.
        /// </summary>
        /// <returns>Damage amount</returns>
        public float GetDamage()
        {
            return this.Damage;
        }
    }
}