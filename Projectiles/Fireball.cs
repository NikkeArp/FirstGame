using UnityEngine;
using WizardAdventure.Effects;

namespace WizardAdventure.Projectiles
{
    public class Fireball : Projectile
    {

    #region [Properties]
        private Animator animator;
        private GlowEffect glowEffect;
        private const float SPEED = 300.0f;
        public static readonly int CAST_TIME = 300;
        private const float MIN_GLOW_INTENSITY = 1.0f;
        private const float GLOW_INTERVAL = 0.2f;
    #endregion

    #region [Unity API]

        /// <summary>
        /// Collision event for fireball. If fireball hits enemy, it deals
        /// damage. Otherwise it just explodes.
        /// </summary>
        /// <param name="other">Other object in collision</param>
        /// <returns></returns>
        private void OnCollisionEnter2D(Collision2D other)
        {
            // Stop the fireball and set hit animation. Disable collider.
            this.animator.SetTrigger("Hit");
            this.Stop();
            this.glowEffect.Intesify(5.0f, 2.0f);
            this.glowEffect.StartFade(0, 0, 0.2f, 0.01f);
            Destroy(this.gameObject, 0.35f);
        }

        private void Start()
        {
            this.glowEffect.Glow(MIN_GLOW_INTENSITY, GLOW_INTERVAL);
        }

    #endregion

    #region [Protected Methods]

        /// <summary>
        /// Overrides initialization. Adds animator to be initialized.
        /// </summary>
        protected override void InitilizeProjectile()
        {
            this.damage = 3;
            this.animator = this.GetComponent<Animator>();
            this.glowEffect = this.GetComponentInChildren<GlowEffect>();
            base.InitilizeProjectile();
        }

    #endregion

    #region [Private Methods]

        /// <summary>
        /// Stops fireball and disables its collider.
        /// </summary>
        private void Stop()
        {
            this.Rigidbody.velocity = Vector2.zero;
            this.Collider.enabled = false;
        }

    #endregion

    #region [Public Methods]

        /// <summary>
        /// Launch method using vector2 as parameter.
        /// Speed is fireballs constant speed.
        /// </summary>
        /// <param name="direction"></param>
        public void Launch(Vector2 direction)
        {
            base.Launch(direction, SPEED);
        }

    #endregion

    }
}


