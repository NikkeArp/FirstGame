using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace WizardAdventure.Spells
{
    /// <summary>
    /// SpellEventManager manages spell cooldowns and possible
    /// debuffs. This class is used by static instance. Only one
    /// Instance is allowed to exist at the time.
    /// </summary>
    public class SpellEventManager : MonoBehaviour
    {
    #region [Properties]
        /// <summary>
        /// Static instance of this class. This instance
        /// is the spell event manager that is referenced when setting
        /// cooldowns etc. Only one instance of manager is allowed.
        /// </summary>
        /// <value>Gets and Sets static SpellEventManager Instance</value>
        public static SpellEventManager Instance { get; private set; } = null;

        /// <summary>
        /// Utility spell Blink's cooldown flag.
        /// Blink teleport spell can't be casted during cooldown.
        /// </summary>
        /// <value>Gets and Sets Blink cooldown.</value>
        public bool BlinkOnCooldown { get; private set; } = false;

        /// <summary>
        /// Projectile spell Fireball's cooldown flag.
        /// Fireball spell can't be casted during cooldown.
        /// </summary>
        /// <value>Gets and Sets Fireball cooldown.</value>
        public bool FireballOnCooldown { get; private set; } = false;

        /// <summary>
        /// Projectile spell Frostblast's cooldown flag.
        /// Frostblast spell can't be casted during cooldown.
        /// </summary>
        /// <value>Gets and Sets Frostblast cooldown.</value>
        public bool FrostBlastOnCooldown { get; private set; } = false;

        /// <summary>
        /// Global Cooldown is set after every successful spellcast.
        /// No spell can be cast during Global cooldown.
        /// </summary>
        /// <value>Gets and Sets global cooldown.</value>
        public bool GlobalCooldown { get; private set; } = false;
        
    #endregion
        
    #region [UnityAPI]

        /// <summary>
        /// Runs before the first frame.
        /// Ensures only one instance of this class exists
        /// at any time.
        /// </summary>
        private void Awake() 
        {
            if (Instance is null)
            {
                Instance = this;
            }   
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    #endregion

    #region [Public Methods]

        /// <summary>
        /// Get spell's cooldown status
        /// </summary>
        /// <typeparam name="T">Spell Type</typeparam>
        /// <returns></returns>
        public bool GetCooldownStatus<T>() where T: ICastable
        {
            if (typeof(T) == typeof(Frostblast))
                return this.FrostBlastOnCooldown;
            if (typeof(T) == typeof(Fireball))
                return this.FireballOnCooldown;
            if (typeof(T) == typeof(Blink))
                return this.BlinkOnCooldown;
            return true;
        }

        /// <summary>
        /// Sets debuff to target. Before setting
        /// the debuff, checks if the target unit is already
        /// affected by the same type of debuff.
        /// </summary>
        /// <param name="target">Debuff target</param>
        /// <typeparam name="T">Type of debuff</typeparam>
        public void SetDebuff<T>(Unit target) 
            where T : Debuff
        {
            Type spellType = typeof(T);
            if (spellType == typeof(Chilled))
            {
                if (!target.ActiveDebuffs.Contains(DebuffName.Chilled))
                {   
                    // Apply chilled debuff to target.
                    this.GetComponent<Chilled>().ApplyDebuff(target);
                    return;
                }
            }
        }

        /// <summary>
        /// Sets global cooldown to spells. Global cooldown is
        /// invoked after every spell cast.
        /// </summary>
        public void SetGlobalCooldown()
        {
            this.StartCoroutine(SetGlobalCooldownRoutine());
        }

        /// <summary>
        /// Set specified spell on cooldown. Cooldown
        /// is removed automatically after duration has passed.
        /// </summary>
        public void SetCooldown<T>()
        {
            Type spellType = typeof(T);

             if (spellType == typeof(Fireball))
            {
                this.StartCoroutine(AddCooldown<Fireball>());
                return;
            }
            if (spellType == typeof(Blink))
            {
                this.StartCoroutine(AddCooldown<Blink>());
                return;
            }
            if (spellType == typeof(Frostblast))
            {
                this.StartCoroutine(AddCooldown<Frostblast>());
                return;
            }
        }

    #endregion

    #region [Private methods]

        /// <summary>
        /// Sets global cooldown. Cooldown is removed
        /// automatically.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetGlobalCooldownRoutine()
        {
            this.GlobalCooldown = true;
            yield return new WaitForSeconds(0.75f);
            this.GlobalCooldown = false;
        }

        /// <summary>
        /// Sets specified spell on cooldown. Cooldown
        /// is removed automatically.
        /// </summary>
        /// <typeparam name="T">Type of the spell</typeparam>
        /// <returns></returns>
        private IEnumerator AddCooldown<T>()
        {
            Type spellType = typeof(T);
            if (spellType == typeof(Fireball))
            {
                this.FireballOnCooldown = true;
                PropertyInfo propInfo = spellType.GetProperty("BaseCooldown",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                object value = propInfo.GetValue(null);
                yield return new WaitForSeconds((float)value);
                this.FireballOnCooldown = false;
            }
            if (spellType == typeof(Blink))
            {
                this.BlinkOnCooldown = true;
                PropertyInfo propInfo = spellType.GetProperty("BaseCooldown",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                object value = propInfo.GetValue(null);
                yield return new WaitForSeconds((float)value);
                this.BlinkOnCooldown = false;
            }
            else if (spellType == typeof(Frostblast))
            {
                this.FrostBlastOnCooldown = true;
                PropertyInfo propInfo = spellType.GetProperty("BaseCooldown",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                object value = propInfo.GetValue(null);
                yield return new WaitForSeconds((float)value);
                this.FrostBlastOnCooldown = false;
            }
        }
    #endregion
        
    }
}