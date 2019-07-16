using UnityEngine;

namespace WizardAdventure.Spells
{
    public abstract class Spell : MonoBehaviour
    {
        #region [Properties]
        public static float BaseCooldown { get; private set; }
        public float Cooldown { get; protected set; }
        protected float castRange;
        protected Animator spellAnimator;
        public Unit Caster { get; set; }
        #endregion

        protected virtual void Awake()
        {
            this.InitializeSpell();
        }

        /// <summary>
        /// Initializes spell
        /// </summary>
        protected virtual void InitializeSpell()
        {
            this.spellAnimator = this.GetComponent<Animator>();
        }
    }
}