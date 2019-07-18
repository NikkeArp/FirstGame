
namespace WizardAdventure.Spells
{
    /// <summary>
    /// Utility spells are buffs and other helpful spells. Implements
    /// ICastable.
    /// Utility spells down deal damage to enemies, but they make
    /// the life of their caster easier.
    /// </summary>
    public abstract class UtilitySpell : Spell, ICastable
    {
    #region [Properties]
        /// <summary>
        /// Is aggressive flag.
        /// </summary>
        /// <value>Get and set Agressiveness flag</value>
        public bool IsAgressive { get; protected set; }
    #endregion
        
        /// <summary>
        /// Cast the spell.
        /// </summary>
        /// <param name="caster">Unit that casterd the spell</param>
        /// <param name="faceRight">Direction caster is facing</param>
        public virtual void Cast(Unit caster, bool faceRight)
        {}
    }
}