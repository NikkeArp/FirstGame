using UnityEngine;
using WizardAdventure.Effects;
using System;

namespace WizardAdventure.Spells
{
    /// <summary>
    /// Fireball projectile spell that shoots ball of flaming
    /// rock at caster's facing direction. If projectile hits enemy Unit,
    /// Explodes and deals damage.
    /// </summary>
    public class Fireball : ProjectileSpell
    {
    #region [Properties]
        /// <summary>
        /// Fireball's base cooldown. Effects all fireball objects.
        /// YOU SHOULD NOT EDIT THIS!!
        /// No one should. Ever. Just let it be.
        /// </summary>
        /// <value>Gets and Sets Fireball base cooldown</value>
        new public static float BaseCooldown { get; private set; }

        /// <summary>
        /// LightEffects script handles this spell's 
        /// light effects.
        /// </summary>
        /// <value>Get and Set light effect script</value>
        public LightEffects LightEffect { get; private set; }
    #endregion

    #region [Unity API]

        /// <summary>
        /// Collision event for frostblast. If projectile hits enemy, it deals
        /// damage. Otherwise it just shatters.
        /// </summary>
        /// <param name="other">Other object in collision</param>
        protected override void OnCollisionEnter2D(Collision2D other) 
        {
            base.OnCollisionEnter2D(other);
            Unit collidedUnit = other.gameObject.GetComponent<Unit>();
            this.LightEffect.InstantIntesify(5.0f, 2.0f);
            this.LightEffect.BeginFade(0.0f, 0.0f, 0.2f, 0.2f, 0.01f);
            Destroy(this.gameObject, 0.4f);
        }

        /// <summary>
        /// Adds fireballs projectile's velocity gradually,
        /// until it has reached max speed. Destroys fireball
        /// if it has reached its castrange.
        /// </summary>
        protected override void Update() 
        {
            SpeedUp();
            base.Update();
        }

    #endregion
    
    #region [Protected Methods]

        /// <summary>
        /// Initializes spell by setting all properties.
        /// This method is invoked in the Awake() method.
        /// </summary>
        protected override void InitializeSpell()
        {
            base.InitializeSpell();
            this.StartSpeed = 2f;
            this.MaxSpeed = 15f;
            this.CastRange = 50f;

            this.LightEffect = GetComponentInChildren<LightEffects>();
            this.SpawnOffset = new Tuple<float, float>(1.0f, 0.5f);

            this.Damage = 2.0f;
            this.Cooldown = BaseCooldown = 3.0f;
        }
        
    #endregion

    }
}