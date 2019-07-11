using UnityEngine;
using WizardAdventure.Effects;
using System;
using System.Collections;

namespace WizardAdventure.Spells
{
    public class Frostblast : ProjectileSpell
    {

    #region [Properties]
        new public static float BaseCooldown { get; private set; }
        private const float SLOW_EFFECT = 0.4f;
        private const float SLOW_DURATION = 2.0f;
        private GlowEffect glowEffect = null;
    #endregion

    #region [Unity API]

        /// <summary>
        /// Adds frostblast projectile's velocity gradually,
        /// until it has reached max speed. Destroys projectile
        /// if it has reached its castrange.
        /// </summary>
        protected override void Update()
        {
            this.SpeedUp();
            base.Update();
        }

        /// <summary>
        /// When Frosblast spell is instantiated, its starts to glow.
        /// </summary>
        protected virtual void Start() 
        {
            this.RandomGlowPatter();
        }

        /// <summary>
        /// Collision event for frostblast. If projectile hits enemy, it deals
        /// damage. Otherwise it just shatters.
        /// </summary>
        /// <param name="other">Other object in collision</param>
        protected override void OnCollisionEnter2D(Collision2D other) 
        {
            base.OnCollisionEnter2D(other);
            Unit collidedUnit = other.gameObject.GetComponent<Unit>();
            if (collidedUnit != null)
            {
                SpellEventManager.Instance.SetDebuff(DebuffName.Chilled, collidedUnit);
            }
            this.glowEffect.Intesify(5.0f, 2.0f);
            this.glowEffect.StartFade(0, 0, 0.2f, 0.01f);
            Destroy(this.gameObject, 0.4f);
        }
 
    #endregion

    #region [Private Methods]

        /// <summary>
        /// Slows hitted enemy. Debuff is lifted automatically
        /// after duration has passed.
        /// </summary>
        /// <param name="target">Hitted target</param>
        /// <returns></returns>
        private void SlowTarget(Unit target)
        {
            if (target.ActiveDebuffs.Contains(DebuffName.Chilled)) 
            {
                return;
            }
            this.StartCoroutine(SlowTargetRoutine(target));
        }

        /// <summary>
        /// Slows hitted enemy. Debuff is lifted automatically
        /// after duration has passed.
        /// </summary>
        /// <param name="target">Hitted target</param>
        /// <returns></returns>
        private IEnumerator SlowTargetRoutine(Unit target)
        {
            target.ActiveDebuffs.Add(DebuffName.Chilled);
            target.MoveSpeed *= SLOW_EFFECT;
            yield return new WaitForSeconds(SLOW_DURATION);
            Debug.Log(Slime.BaseMoveSpeed);
            target.MoveSpeed = Slime.BaseMoveSpeed;
            target.ActiveDebuffs.Remove(DebuffName.Chilled);
        }

        /// <summary>
        /// Adds randomization for ligths glow effects.
        /// </summary>
        private void RandomGlowPatter()
        {
            this.StartCoroutine(RandomGlowRoutine());
        }

        /// <summary>
        /// Adds randomization for ligths glow effects.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RandomGlowRoutine()
        {
            var tailOne = this.transform.Find("TailLightOne");
            var tailTwo = this.transform.Find("TailLightTwo");

            // Starts main light's glow effect
            this.glowEffect.Glow(1.0f, 0.2f);

            // After random timespan, enables tail lights and starts their glow effect
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.5f));

            tailOne.GetComponent<Light>().enabled = true;
            tailOne.GetComponent<GlowEffect>().Glow(0.2f, 0.2f);
            
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.5f));
            tailTwo.GetComponent<Light>().enabled = true;
            tailTwo.GetComponent<GlowEffect>().Glow(0.2f, 0.2f);
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
            this.SpawnOffset = new Tuple<float, float>(1.0f, 0.5f);
            this.MaxSpeed = 15f;
            this.glowEffect = GetComponentInChildren<GlowEffect>();
            this.damage = 2;
            this.Cooldown = BaseCooldown =  2.0f;
            this.castRange = 50f;
        }

    #endregion

    }
}