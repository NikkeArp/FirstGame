using UnityEngine;
using System;

namespace WizardAdventure.Spells
{
    /// <summary>
    /// SpellBook game object controlls players spells.
    /// If player presses spell key, the spell casting starts here.
    /// SpellBook contains spell gameobject prefabs, that are set in editor.
    /// </summary>
    public class SpellBook : MonoBehaviour
    {
    #region [Properties]
        /// <summary>
        /// Caster Unit.
        /// </summary>
        /// <value>Get and set caster Unit</value>
        public Unit Caster { get; set; }

        /// <summary>
        /// Frostblast Prefab. Set in editor.
        /// </summary>
        public GameObject FrostBlastPrefab = null;

        /// <summary>
        /// Blink spell prefab. Set in editor.
        /// </summary>
        public GameObject BlinkPrefab = null;

        /// <summary>
        /// Fireball prefab. Set in editor.
        /// </summary>
        public GameObject FireballPrefab = null;
    #endregion

    #region [Public Methods]

        /// <summary>
        /// Attempts to cast spells
        /// </summary>
        public void AttemptCast()
        {
            if (SpellEventManager.Instance.GlobalCooldown)
            {
                // Global cooldown is active.
                return;
            }
            else
            {
                // Blink
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    this.Caster.SetCastAnimation();
                    if(this.CastSpell<Blink>(Caster.FaceRigth))
                    {
                        SpellEventManager.Instance.SetGlobalCooldown();
                    }
                }
                // FrostBlast
                if (Input.GetKeyDown(KeyCode.R))
                {
                    this.Caster.SetCastAnimation();
                    if(this.CastSpell<Frostblast>(Caster.FaceRigth))
                    {
                        SpellEventManager.Instance.SetGlobalCooldown();
                    }
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    this.Caster.SetCastAnimation();
                    if (this.CastSpell<Fireball>(Caster.FaceRigth))
                    {
                        SpellEventManager.Instance.SetGlobalCooldown();
                    }
                }
            }
        }

        
        /// <summary>
        /// Casts specified spell if its not
        /// on cooldown. Returns true if cast is succesful.
        /// </summary>
        /// <param name="faceRight">Direction caster is facing</param>
        /// <typeparam name="T">Spell type</typeparam>
        /// <returns>True if spell is casted</returns>
        public bool CastSpell<T>(bool faceRight) where T : ICastable
        {
            Type spellType = typeof(T);
            if (spellType == typeof(Fireball))
            {
                return TryCasting<T>(faceRight, FireballPrefab);
            }
            if (spellType == typeof(Blink))
            {
                return TryCasting<T>(faceRight, BlinkPrefab);
            }
            if (spellType == typeof(Frostblast))
            {
                return TryCasting<T>(faceRight, FrostBlastPrefab);
            }
            return false;
        }

        /// <summary>
        /// Checks if spell is on cooldown. If
        /// not -> casts the spell.
        /// </summary>
        /// <param name="faceRight">Direction caster is facing</param>
        /// <param name="prefab">Spell prefab</param>
        /// <typeparam name="T">Spell type</typeparam>
        /// <returns></returns>
        private bool TryCasting<T>(bool faceRight, GameObject prefab) where T : ICastable
        {
            if (SpellEventManager.Instance.GetCooldownStatus<T>())
            {
                return false;
            }
            else 
            {
                CreateSpell<T>(prefab, faceRight);
                return true;
            }
        }

        /// <summary>
        /// Creates spell game object and adds it to the game.
        /// Sets casted spell on cooldown.
        /// </summary>
        /// <param name="prefab">Spell prefab</param>
        /// <param name="faceRight">Direction caster is facing</param>
        /// <typeparam name="T">Spell type</typeparam>
        private void CreateSpell<T>(GameObject prefab, bool faceRight) where T : ICastable
        {
            GameObject spellGameObject = Instantiate(prefab, this.Caster.transform.position, Quaternion.identity);
            T spellScript = spellGameObject.GetComponent<T>();
            spellScript.Cast(this.Caster, faceRight);
            SpellEventManager.Instance.SetCooldown<T>();
        }

    #endregion
        
    }
}