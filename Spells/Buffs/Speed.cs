using UnityEngine;

namespace WizardAdventure.Spells
{
    /// <summary>
    /// Speed buff increases it's target's movement
    /// speed for the duration of the buff.
    /// </summary>
    public class Speed : Buff
    {
    #region [Properties]
        /// <summary>
        /// This value is added to buff's target movement speed
        /// for the duration of the buff.
        /// </summary>
        /// <value>Get and Set Movement speed increase</value>
        public float MoveSpeedIncrease { get; private set; }
    #endregion

        /// <summary>
        ///  Set chilled debuff attributes.
        /// </summary>
        protected override void InitializeBuff()
        {
            this.MoveSpeedIncrease = 300f;
            this.DurationInSeconds = 4.0f;
        }
    }
}