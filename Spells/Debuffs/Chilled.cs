using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

namespace WizardAdventure.Spells
{
    public class Chilled : Debuff
    {
    #region [Properties]
        private const float SLOW_MULTIPLIER = 0.4f;
        private Color targetOriginalColor;
        private Color targetOriginalLigthColor;
        private Color newSpriteColor;
        private Color chillEffectColor;
        public GameObject chilledParticleEffectPrefab;
        private GameObject chilledParticleEffect;
    #endregion

        /// <summary>
        ///  Set chilled debuff attributes.
        /// </summary>
        protected override void InitializeDebuff()
        {
            this.newSpriteColor = Color.cyan;
            this.chillEffectColor = new Color(15, 55, 125);
            this.durationInSeconds = 4.0f;
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

            Vector3 chilledEffetPos = this.target.GetCenterPoint<SpriteRenderer>();
            this.chilledParticleEffect = Instantiate(chilledParticleEffectPrefab,
                chilledEffetPos, Quaternion.identity);
            this.chilledParticleEffect.transform.parent = this.target.transform;

            // Apply chilled debuff effects to target.
            this.AffectTarget();

            // Wait for debuffs duration.
            yield return new WaitForSeconds(this.durationInSeconds);

            // Reset target by removing all debuff effects.
            this.ResetTarget();

            // After duration, remove chilled debuff from targets debuff list.
            this.target.ActiveDebuffs.Remove(DebuffName.Chilled);
        }

        /// <summary>
        /// Set debuff effects to target.
        /// </summary>
        private void AffectTarget()
        {
            // If target has its own light effects => Turn them off 
            if (this.target is ILightUp litObject)
            {
                litObject.GetEffectController().DisableLight();
            }

            // Change sprite color to light blue.
            this.target.ChangeColor(this.newSpriteColor);

            // Activate targets child Debuff effect gameobject
            // and change its color to light blue.
            this.target.DebuffEffect.gameObject.SetActive(true);
            this.target.DebuffEffect.ChangeColor(this.chillEffectColor);

            // Slow target based on chilled slow multiplier.
            this.target.MoveSpeed *= SLOW_MULTIPLIER;
        }

        /// <summary>
        /// Removes any visual or functional debuff effects.
        /// </summary>
        private void ResetTarget()
        {
            // Get units static BaseMoveSpeed propertie dynamically using
            // Reflections. Targets movement speed is returned back to normal.
            Type targetType = this.target.GetType();
            PropertyInfo propInfo = targetType.GetProperty("BaseMoveSpeed", BindingFlags.Public | BindingFlags.Static);
            object value = propInfo.GetValue(null);
            this.target.MoveSpeed = (float)value;

            // Turn units own ligth effect back on.
            if (this.target is ILightUp litObject)
            {
                litObject.GetEffectController().EnableLight();
            }

            // Turn units debuff effects off.
            this.target.DebuffEffect.gameObject.SetActive(false);
            this.target.ChangeColor(targetOriginalColor);
            Destroy(chilledParticleEffect.gameObject);
        }

        /// <summary>
        /// Updates debuff attributes
        /// </summary>
        /// <param name="target">Debuff target</param>
        protected override void UpdateAttributes(Unit target)
        {
            base.UpdateAttributes(target);
            this.targetOriginalColor = this.target.GetColor();

            // If target has its own light effects 
            //    => store color in variable.
            if (target is ILightUp litObject)
            {
                Light targetsOwnLigth = litObject.GetEffectController().GetComponent<Light>();
                this.targetOriginalLigthColor = targetsOwnLigth.color;
            }
        }
    }
}