using UnityEngine;

namespace WizardAdventure.Spells
{
    public abstract class Debuff : MonoBehaviour
    {
    #region [Properties]
        protected float durationInSeconds;
        protected Unit target;
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
            this.target = target;
        }

        /// <summary>
        /// Called in Awake() mewthod.
        /// </summary>
        protected abstract void InitializeDebuff();

    }
}