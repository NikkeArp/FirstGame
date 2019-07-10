using UnityEngine;

namespace WizardAdventure.Spells
{
    public abstract class Spell : MonoBehaviour
    {
        #region [Properties]
        public static float BaseCooldown { get; private set; }
        public Unit Caster { get; set; }
        public bool IsOnCooldown { get; set; }
        public float Cooldown { get; protected set; }
        protected float castRange;
        protected Animator spellAnimator;
        #endregion


        /// <summary>
        /// 
        /// </summary>
        protected virtual void Awake()
        {
            this.InitializeSpell();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeSpell()
        {
            this.spellAnimator = this.GetComponent<Animator>();
        }
    }
}