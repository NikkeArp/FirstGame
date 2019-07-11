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
    #endregion

    #region [Public Methods]

        /// <summary>
        /// Tries to cast specified spell.
        /// </summary>
        /// <param name="faceRight"></param>
        public bool CastSpell<T>(bool faceRight)
        {
            Type spellType = typeof(T);
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