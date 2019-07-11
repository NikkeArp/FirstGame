#define DEBUG

using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace WizardAdventure.Spells
{
    public class SpellEventManager : MonoBehaviour
    {
    #region [Properties]
        public static SpellEventManager Instance { get; private set; } = null;
        public bool BlinkOnCooldown { get; private set; } = false;
        public bool FireballOnCooldown { get; private set; } = false;
        public bool FrostBlastOnCooldown { get; private set; } = false;
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
            if (Instance == null)
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
        /// <param name="debuffName">Debuff identifier</param>
        /// <param name="target">Debuff target</param>
        public void SetDebuff(DebuffName debuffName, Unit target)
        {
            switch (debuffName)
            {
                // Chilled debuff slows freezes its target, slowing
                // movement speed and adding icy visual effects. Dispelled
                // automatically after it's duration has passed.
                case DebuffName.Chilled:
                {
                    // Target is already chilled.
                    if (target.ActiveDebuffs.Contains(DebuffName.Chilled))
                    {
                        break;
                    }
                    else
                    {   // Apply chilled debuff to target.
                        this.GetComponent<Chilled>().ApplyDebuff(target);
                        break;
                    }
                }
                default:
                    break;
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
        /// <param name="spell">Spell object</param>
        [Obsolete("This overload is not recommended, please use Generic version instead.", true)]
        public void SetCooldown(Spell spell)
        {
            // Get the type of the spell object.
            // All spells are inherited from the Spell class.
            Type type =  spell.GetType();

            // Blink spell teleports caster forward
            if (type == typeof(Blink))
            {
                this.StartCoroutine(setCD(spell));
                return;
            }
            // Frostblast shoots iceshard forwards, that
            // slows enemies that got hit.
            if (type == typeof(Frostblast))
            {
                this.StartCoroutine(setCD(spell));
                return;
            }
        }

        /// <summary>
        /// Set specified spell on cooldown. Cooldown
        /// is removed automatically after duration has passed.
        /// </summary>
        /// <param name="spell">Spell object</param>
        [Obsolete("This overload is not recommended, please use Generic version instead.", true)]
        public void SetCooldown(Type spell)
        {
            // Blink spell teleports caster forward
            if (spell == typeof(Blink))
            {
                this.StartCoroutine(setCD<Blink>());
                return;
            }
            // Frostblast shoots iceshard forwards, that
            // slows enemies that got hit.
            if (spell == typeof(Frostblast))
            {
                this.StartCoroutine(setCD<Frostblast>());
                return;
            }
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
                this.StartCoroutine(setCD<Fireball>());
                return;
            }
            if (spellType == typeof(Blink))
            {
                this.StartCoroutine(setCD<Blink>());
                return;
            }
            if (spellType == typeof(Frostblast))
            {
                this.StartCoroutine(setCD<Frostblast>());
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
        /// <param name="spell"></param>
        /// <returns></returns>
        private IEnumerator setCD(Spell spell)
        {
            if (spell is Blink)
            {
                this.BlinkOnCooldown = true;
                yield return new WaitForSeconds(spell.Cooldown);
                this.BlinkOnCooldown = false;
            }
            else if (spell is Frostblast)
            {
                this.FrostBlastOnCooldown = true;
                yield return new WaitForSeconds(spell.Cooldown);
                this.FrostBlastOnCooldown = false;
            }
        }

        /// <summary>
        /// Sets specified spell on cooldown. Cooldown
        /// is removed automatically.
        /// </summary>
        /// <typeparam name="T">Type of the spell</typeparam>
        /// <returns></returns>
        private IEnumerator setCD<T>()
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