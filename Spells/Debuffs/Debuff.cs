using UnityEngine;

namespace WizardAdventure.Spells
{
    /// <summary>
    /// Debuff adds negative modifiers to it's target Unit.
    /// Debuffs are usually removed after fixed duration has passed. 
    /// </summary>
    public abstract class Debuff : MonoBehaviour
    {
    #region [Properties]

        /// <summary>
        /// Debuff duration in seconds.
        /// </summary>
        /// <value>Gets and Sets debuff duration value in seconds.</value>
        public float DurationInSeconds { get; protected set; }

        /// <summary>
        /// Debuff's target unit.
        /// </summary>
        /// <value>Sets and Gets debuff's tareget unit.</value>
        public Unit Target { get; protected set; }
    #endregion
        
        protected void Awake() 
        {
            InitializeDebuff();
        }

        /// <summary>
        /// Update Attributes after creation.
        /// </summary>
        /// <param name="target"></param>
        protected virtual void UpdateAttributes(Unit target)
        {
            this.Target = target;
        }

        /// <summary>
        /// Called in Awake() method.
        /// </summary>
        protected abstract void InitializeDebuff();

    }
}