using UnityEngine;

namespace WizardAdventure.Spells
{
    public abstract class UtilitySpell : Spell
    {
        new public static float BaseCooldown { get; private set; }
        protected bool isAgressive;
    }
}