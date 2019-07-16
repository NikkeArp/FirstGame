using UnityEngine;

namespace WizardAdventure.Spells
{
    public abstract class UtilitySpell : Spell, ICastable
    {
        public bool IsAgressive { get; protected set; }
        public virtual void Cast(Unit caster, bool faceRight)
        {}
    }
}