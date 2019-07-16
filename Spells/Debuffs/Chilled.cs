using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

namespace WizardAdventure.Spells
{
    /// <summary>
    /// Chilled debuff slows it's target for fixed duration.
    /// Also modifies target's appereance using Unit's child DebuffEffect gameobject.
    /// After duration has passed, slow and visual modifiers are removed.
    /// </summary>
    public class Chilled : Debuff
    {
    #region [Properties]

        /// <summary>
        /// Chilled particle effect prefab. Set in editor.
        /// </summary>
        public GameObject chilledParticleEffectPrefab;
        
        /// <summary>
        /// Chilled debuff's constant slow multiplier.
        /// </summary>
        private const float SLOW_MULTIPLIER = 0.4f;

        /// <summary>
        /// Target's original sprite color.
        /// </summary>
        /// <value>Get and Set target's original sprite color.</value>
        public Color TargetOriginalColor { get; private set; }

        /// <summary>
        /// Target's light component's original color.
        /// </summary>
        /// <value>Get and Set target's original light color.</value>
        public Color TargetOriginalLightColor { get; private set; }

        /// <summary>
        /// New Sprite color
        /// </summary>
        /// <value>Get and Set new sprite color</value>
        public Color NewSpriteColor { get; private set; }

        /// <summary>
        /// Chilled effect color. usually blueish
        /// </summary>
        /// <value>Get and Set Chilled debuff effect color.</value>
        public Color ChillEffectColor { get; private set; }

    
        /// <summary>
        /// Chilled particle effect. Shoots snowflakes around.
        /// </summary>
        /// <value>Get and Set Chilled particle effect.</value>
        public GameObject ChilledParticleEffect { get; private set; }
    #endregion

        /// <summary>
        ///  Set chilled debuff attributes.
        /// </summary>
        protected override void InitializeDebuff()
        {
            this.NewSpriteColor = Color.cyan;
            this.ChillEffectColor = new Color(15, 55, 125);
            this.DurationInSeconds = 4.0f;
        }

        /// <summary>
        /// Applys chilled debuff to target.
        /// Removes effect automatically after duration.
        /// </summary>
        /// <param name="target">Debuff target</param>
        public void ApplyDebuff(Unit target)
        {
            // Set information from the target as Debuff object's attributes.
            UpdateAttributes(target); 

            // Add Chilled debuff enum to targets active debuffs list to make
            // sure only one chilled effect is active at all times.
            target.ActiveDebuffs.Add(DebuffName.Chilled);

            // Start Debuff routine.
            this.StartCoroutine(this.ApplyDebuff());
        }

        private IEnumerator ApplyDebuff()
        {
            // Instantiate chilled particle effect prefab and set it
            // as a child of the target.

            Vector3 chilledEffetPos = this.Target.GetCenterPoint<SpriteRenderer>();
            this.ChilledParticleEffect = Instantiate(chilledParticleEffectPrefab,
                chilledEffetPos, Quaternion.identity);
            this.ChilledParticleEffect.transform.parent = this.Target.transform;
            this.ChilledParticleEffect.gameObject.SetActive(true);

            // Apply chilled debuff effects to target.
            this.AffectTarget();

            // Wait for debuffs duration.
            yield return new WaitForSeconds(this.DurationInSeconds);

            // Reset target by removing all debuff effects.
            this.ResetTarget();

            // After duration, remove chilled debuff from targets debuff list.
            this.Target.ActiveDebuffs.Remove(DebuffName.Chilled);
        }

        /// <summary>
        /// Set debuff effects to target.
        /// </summary>
        private void AffectTarget()
        {
            // If target has its own light effects => Turn them off 
            if (this.Target is ILightUp litObject)
            {
                litObject.GetEffectController().DisableLight();
            }

            // Change sprite color to light blue.
            this.Target.ChangeColor(this.NewSpriteColor);

            // Activate targets child Debuff effect gameobject
            // and change its color to light blue.
            this.Target.DebuffEffect.gameObject.SetActive(true);
            this.Target.DebuffEffect.ChangeColor(this.ChillEffectColor);

            // Slow target based on chilled slow multiplier.
            this.Target.MoveSpeed *= SLOW_MULTIPLIER;
        }

        /// <summary>
        /// Removes any visual or functional debuff effects.
        /// </summary>
        private void ResetTarget()
        {
            // Get units static BaseMoveSpeed propertie dynamically using
            // Reflections. Targets movement speed is returned back to normal.
            Type targetType = this.Target.GetType();
            PropertyInfo propInfo = targetType.GetProperty("BaseMoveSpeed", BindingFlags.Public | BindingFlags.Static);
            object value = propInfo.GetValue(null);
            this.Target.MoveSpeed = (float)value;

            // Turn units own ligth effect back on.
            if (this.Target is ILightUp litObject)
            {
                litObject.GetEffectController()?.EnableLight();
            }

            // Turn units debuff effects off.
            this.Target.DebuffEffect.gameObject.SetActive(false);
            this.Target.ChangeColor(TargetOriginalColor);
            Destroy(ChilledParticleEffect.gameObject);
        }

        /// <summary>
        /// Updates debuff attributes
        /// </summary>
        /// <param name="target">Debuff target</param>
        protected override void UpdateAttributes(Unit target)
        {
            base.UpdateAttributes(target);
            this.TargetOriginalColor = this.Target.GetColor();

            // If target has its own light effects 
            //    => store color in variable.
            if (target is ILightUp litObject)
            {
                Light targetsOwnLigth = litObject.GetEffectController().GetComponent<Light>();
                this.TargetOriginalLightColor = targetsOwnLigth.color;
            }
        }
    }
}