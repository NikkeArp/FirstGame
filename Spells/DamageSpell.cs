using UnityEngine;

namespace WizardAdventure.Spells
{
    public abstract class DamageSpell : Spell
    {
        new public static float BaseCooldown { get; private set; }
        protected float damage;
        
    }
}