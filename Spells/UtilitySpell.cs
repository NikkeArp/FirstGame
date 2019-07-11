using UnityEngine;

namespace WizardAdventure.Spells
{
    public abstract class UtilitySpell : Spell, ICastable
    {
        new public static float BaseCooldown { get; private set; }
        protected bool isAgressive;

        public virtual void Cast(Unit caster, bool faceRight)
        {}
    }
}