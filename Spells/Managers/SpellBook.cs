using UnityEngine;
using System;
using System.Collections.Generic;

namespace WizardAdventure.Spells
{
    public class SpellBook : MonoBehaviour
    {
    #region [Properties]
        public Unit Caster { get; set; }
        public GameObject FrostBlastPrefab = null;
        public GameObject BlinkPrefab = null;
        public GameObject FireballPrefab = null;
    #endregion

    #region [Public Methods]

        /// <summary>
        /// 
        /// </summary>
        public void AttemptCast()
        {
            if (SpellEventManager.Instance.GlobalCooldown)
            {
                return;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    this.Caster.SetCastAnimation();
                    if(this.CastSpell<Blink>(Caster.FaceRigth))
                    {
                        SpellEventManager.Instance.SetGlobalCooldown();
                    }
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    this.Caster.SetCastAnimation();
                    if(this.CastSpell<Frostblast>(Caster.FaceRigth))
                    {
                        SpellEventManager.Instance.SetGlobalCooldown();
                    }
                }
            }
        }

        /// <summary>
        /// Tries to cast specified spell.
        /// </summary>
        /// <param name="faceRight"></param>
        public bool CastSpell<T>(bool faceRight)
        {
            Type spellType = typeof(T);

            if (spellType == typeof(Fireball))
            {
                if (SpellEventManager.Instance.FireballOnCooldown) return false;
                GameObject fireballGameObject = Instantiate(FireballPrefab, this.Caster.transform.position, Quaternion.identity);
                Fireball fireball = fireballGameObject.GetComponent<Fireball>();
                fireball.Cast(this.Caster, faceRight);
                SpellEventManager.Instance.SetCooldown<Fireball>();
                return true;
            }
            if (spellType == typeof(Blink))
            {
                if (SpellEventManager.Instance.BlinkOnCooldown) return false;
                GameObject blinkGameObject = Instantiate(BlinkPrefab, this.Caster.transform.position, Quaternion.identity);
                Blink blink = blinkGameObject.GetComponent<Blink>();
                blink.Cast(this.Caster, faceRight);
                SpellEventManager.Instance.SetCooldown<Blink>();
                return true;
            }
            if (spellType == typeof(Frostblast))
            {
                if (SpellEventManager.Instance.FrostBlastOnCooldown) return false;
                GameObject frostBlastGameObj = Instantiate(FrostBlastPrefab, this.Caster.transform.position, Quaternion.identity);
                Frostblast frostBlast = frostBlastGameObj.GetComponent<Frostblast>();
                frostBlast.Cast(this.Caster, faceRight);
                SpellEventManager.Instance.SetCooldown<Frostblast>();
                return true;
            }
            return false;
        }

    #endregion
        
    }
}