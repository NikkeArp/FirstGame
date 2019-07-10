using UnityEngine;
using System;

namespace WizardAdventure.Spells
{
    public class ProjectileSpell : DamageSpell
    {
    #region [Properties]
        new public static float BaseCooldown { get; private set; }
        protected Collider2D projectileCollider = null;
        protected Vector2 LaunchDirection = Vector2.zero;
        protected Rigidbody2D Rigidbody = null;
        protected bool faceRight = false;
    #endregion

    #region [Unity API]

        /// <summary>
        /// Projectile spell collision eventhandler.
        /// Stops projectile and sets "hit" animation.
        /// </summary>
        /// <param name="other">Other object in collision</param>
        protected virtual void OnCollisionEnter2D(Collision2D other) 
        {
            this.StopProjectile();
            this.spellAnimator.SetTrigger("Hit");
        }

        /// <summary>
        /// Projectile object is destroyed if it has travelled beyond the limit.
        /// </summary>
        protected virtual void Update() 
        {
            if (transform.position.magnitude > this.castRange)
            {
                Destroy(gameObject);
            }
        }

    #endregion

    #region [Protected Mehods]

        /// <summary>
        /// Called after Spell is added to game. Sets information
        /// used in launch.
        /// </summary>
        /// <param name="caster">Unit that casted the spell</param>
        /// <param name="faceRight">Direction caster is facing</param>
        /// <param name="startOffset">Offset for projectile's start position</param>
        protected virtual void UpdateSpellInfo(Unit caster, bool faceRight, Tuple<float, float> startOffset)
        {
            this.Caster = caster;
            this.faceRight = faceRight;
            this.LaunchDirection = faceRight ? Vector2.right : Vector2.left;

            Vector3 startPosition = this.Caster.transform.position;
            startPosition.x += faceRight ? startOffset.Item1 : -startOffset.Item1;
            startPosition.y += startOffset.Item2;
            this.transform.position = startPosition;
        }

        /// <summary>
        /// Launches projectile towards given direction
        /// with given speed.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="speed"></param>
        protected virtual void Launch(Vector2 direction, float speed)
        {
            if (!this.faceRight)
            {
                transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, 1);
            }
            this.Rigidbody.velocity = direction * speed;
        }

        /// <summary>
        /// Override for spell initialization. Adds
        /// rigidbody and collider. This Method is invoked
        /// in the Awake() method.
        /// </summary>
        protected override void InitializeSpell()
        {
            base.InitializeSpell();
            this.Rigidbody = this.GetComponent<Rigidbody2D>();
            this.projectileCollider = this.GetComponent<Collider2D>();
        }

        /// <summary>
        /// Stops projectile and disables its collider.
        /// </summary>
        protected virtual void StopProjectile()
        {
            this.Rigidbody.velocity = Vector2.zero;
            this.projectileCollider.enabled = false;
        }

    #endregion
    }
}