using UnityEngine;

namespace WizardAdventure.Spells
{
    /// <summary>
    /// Buff adds positive modifiers to it's target Unit.
    /// Buffs are usually removed after fixed duration has passed. 
    /// </summary>
    public abstract class Buff : MonoBehaviour
    {
    #region [Properties]
        /// <summary>
        /// Buff duration in seconds.
        /// </summary>
        /// <value>Gets and Sets Buff duration value in seconds.</value>
        public float DurationInSeconds { get; protected set; }

        /// <summary>
        /// Debuff's target unit.
        /// </summary>
        /// <value>Sets and Gets Buff's tareget unit.</value>
        public Unit Target { get; protected set; }
    #endregion

    #region [Unity API]
        protected void Awake() 
        {
            InitializeBuff();
        }
    #endregion

    #region [Protected Methods]

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
        protected abstract void InitializeBuff();
        
    #endregion
    }
}