using UnityEngine;
using System;

namespace WizardAdventure.Spells
{
    /// <summary>
    /// Abstact class of projectile spell. Implements ICastable.
    /// Projectile spells shoot projectiles at caster's facing direction,
    /// usually dealing damage and debuffs to hitted enemy Units.
    /// </summary>
    public abstract class ProjectileSpell : DamageSpell, ICastable
    {
    #region [Properties]

        /// <summary>
        /// Projectile spell's 2D Collider component.
        /// </summary>
        /// <value>Get and Set Collider component.</value>
        public Collider2D ProjectileCollider { get; protected set; }

        /// <summary>
        /// Projectile spell's 2D Rigidbody component.
        /// </summary>
        /// <value>Get and Set Rigidbody component.</value>
        public Rigidbody2D Rigidbody { get; protected set; }

        /// <summary>
        /// Projectile spell's launch direction vector.
        /// </summary>
        /// <value>Get and Set projectile spell's launch direction vector.</value>
        public Vector2 LaunchDirection { get; protected set; } = Vector2.zero;

        /// <summary>
        /// Projectile spell's face right flag.
        /// </summary>
        /// <value>Get and Set projectile spell's face right flag.</value>
        public bool FaceRight { get; protected set; } = false;

        /// <summary>
        /// Projectile spell's spawn offset as float Tuple.
        /// When projectile gameobject is instansiated, this offset
        /// is used to move projectile away from caster's game object.
        /// </summary>
        /// <value>Get and Set projectile spell's spawn offset</value>
        public Tuple<float, float> SpawnOffset { get; protected set; }

        /// <summary>
        /// Projectile spell's max speed.
        /// </summary>
        /// <value>Get and Set Max speed</value>
        public float MaxSpeed { get; protected set; }

        /// <summary>
        /// Projectile spells start speed.
        /// </summary>
        /// <value>Get and set start speed.</value>
        public float StartSpeed { get; protected set; }
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
            this.SpellAnimator.SetTrigger("Hit");
        }

        /// <summary>
        /// Projectile object is destroyed if it has travelled beyond the limit.
        /// </summary>
        protected virtual void Update() 
        {
            if (transform.position.magnitude > this.CastRange)
            {
                Destroy(this.gameObject);
            }
        }

    #endregion

    #region [Protected Mehods]

        /// <summary>
        /// Sets caster information for frostblast object and
        /// then launches it in facing direction.
        /// </summary>
        /// <param name="caster">Unit that casted the spell</param>
        /// <param name="faceRight">Direction caster is facing</param>
        public virtual void Cast(Unit caster, bool faceRight)
        {
            this.UpdateSpellInfo(caster, faceRight, this.SpawnOffset);
            this.Launch(this.LaunchDirection, this.StartSpeed);
        }

        /// <summary>
        /// Adds frostblast projectile's velocity gradually,
        /// until it has reached max speed.
        /// </summary>
        protected virtual void SpeedUp()
        {
            // Increases frostblast-projectiles velocity over time.
            if (Mathf.Abs(this.Rigidbody.velocity.x) < this.MaxSpeed)
            {
                Vector2 newSpeed = this.Rigidbody.velocity;
                newSpeed.x += this.FaceRight ? 0.2f : -0.2f;
                this.Rigidbody.velocity = newSpeed;
            }
        }

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
            this.FaceRight = faceRight;
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
            if (!this.FaceRight)
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
            this.ProjectileCollider = this.GetComponent<Collider2D>();
        }

        /// <summary>
        /// Stops projectile and disables its collider.
        /// </summary>
        protected virtual void StopProjectile()
        {
            this.Rigidbody.velocity = Vector2.zero;
            this.ProjectileCollider.enabled = false;
        }

    #endregion
    
    }
}