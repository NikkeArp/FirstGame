using UnityEngine;
using WizardAdventure.Effects;
using System;

namespace WizardAdventure.Spells
{
    public class Fireball : ProjectileSpell
    {
    #region [Properties]
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        new public static float BaseCooldown { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
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
            this.castRange = 50f;

            this.LightEffect = GetComponentInChildren<LightEffects>();
            this.SpawnOffset = new Tuple<float, float>(1.0f, 0.5f);

            this.damage = 2.0f;
            this.Cooldown = BaseCooldown = 3.0f;
        }
        
    #endregion

    }
}