using UnityEngine;

namespace WizardAdventure.Spells
{
    /// <summary>
    /// Spells are magical weapons, projectiles, buffs, debuffs and so on.
    /// Only the immagination is limit here. Spells still do have cooldowns, casters,
    /// and effect ranges to have some sense in the game.
    /// </summary>
    public abstract class Spell : MonoBehaviour
    {
        #region [Properties]
        /// <summary>
        /// Spell's base cooldown. Effects all spells that are same
        /// type. EFFECT INSTANTIATED OBJECTS!!!
        /// YOU SHOULD NOT EDIT THIS!!
        /// No one should. Ever. Just let it be.
        /// </summary>
        /// <value>Gets and Sets spell's base cooldown</value>
        public static float BaseCooldown { get; private set; }

        /// <summary>
        /// Spell's cooldown. After the spell is casted,
        /// it's set on cooldown, preventing spamming same spell over and
        /// over again. 
        /// </summary>
        /// <value>Get and Set spell's cooldown</value>
        public float Cooldown { get; protected set; }

        /// <summary>
        /// Spell's cast range. Limits spell's effective range.
        /// </summary>
        /// <value>Get's and Sets spell's cast range.</value>
        public float CastRange { get; protected set; }

        /// <summary>
        /// Spell's Animator component.
        /// </summary>
        /// <value>Get and Set spell's animator component.</value>
        public Animator SpellAnimator { get; protected set; }

        /// <summary>
        /// Spell's caster. Unit that casted the spell.
        /// </summary>
        /// <value>Get and Set spell's caster.</value>
        public Unit Caster { get; protected set; }
        #endregion

        /// <summary>
        /// Initialize spell properties and attributes on the awake.
        /// </summary>
        protected virtual void Awake()
        {   
            this.InitializeSpell();
        }

        /// <summary>
        /// Initializes spell
        /// </summary>
        protected virtual void InitializeSpell()
        {
            this.SpellAnimator = this.GetComponent<Animator>();
        }
    }
}