using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using WizardAdventure.Effects;

namespace WizardAdventure.Spells
{
    public class Blink : UtilitySpell
    {
        #region [Properties]
            new public static float BaseCooldown { get; set; }
            private Light floorLigth = null;
            private GlowEffect glowEffect = null;
            private Light centerLigth = null;
            private GlowEffect centerGlowEffect = null;
        #endregion

    #region [Unity API]

    #endregion

        /// <summary>
        /// Casts blink. Blink transports caster to new position based on given distance and
        /// direction caster is facing. While blinking, casters scale is
        /// lowered when cast starts, and brougth back up when blinking is done.
        /// </summary>
        /// <param name="direction">Direction caster is facing</param>
        public void Cast(Unit caster, bool faceRigth)
        {
            if (SpellEventManager.Instance.BlinkOnCooldown)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                this.Caster = caster;
                Vector2 blinkVector = faceRigth ? new Vector2(this.castRange, 0) : new Vector2(-this.castRange, 0);
                RaycastHit2D hit = Physics2D.Raycast(this.Caster.transform.position, blinkVector);

                if (hit.collider != null)
                {   
                    // Caster can't blink outside of the map.
                    SpellEventManager.Instance.SetCooldown<Blink>();
                    this.Caster.IsFrozen = true;
                    this.ExcecuteBlink(hit, blinkVector);
                }
                else
                {
                    Debug.Log("Raycast didnt hit anything");
                }
            }
        }


        /// <summary>
        /// Starts blinking operation. Uses raycast2d to make sure caster
        /// blinks into safe area. If there is an obstacle betweed caster
        /// and blink transport area, blink transports caster newx to that obstacle.
        /// </summary>
        /// <param name="hit">Raycast hit</param>
        /// <param name="blinkVector">Casters blink vector</param>
        private void ExcecuteBlink(RaycastHit2D hit, Vector2 blinkVector)
        {
            var originalScale = this.Caster.transform.localScale;
            if (hit.collider.gameObject.CompareTag("OutterWall"))
                {
                    float distance = Mathf.Abs(hit.point.x - this.Caster.transform.position.x);
                    if (distance < this.castRange)
                    {
                        // Obstacle in the path. Blink will transport caster next to it.

                        // Buffer to make sure caster is not colliding with the obstacle
                        float buffer = blinkVector.x > 0.0f ? -0.5f : 0.5f; 

                        Vector3 newPosition = new Vector3(hit.transform.position.x + buffer, hit.transform.position.y,
                            this.Caster.transform.position.z);
                        BlinkWithWarp(originalScale, newPosition);
                    }
                    else
                    {   // Raycast hitted obstacle, but distance is greater than blinking cast distance.
                        BlinkForward(blinkVector, originalScale);
                    }
                }
            else
            {   // Raycast hitted obstacle but it wasn't in predefined list of colliders.
                BlinkForward(blinkVector, originalScale);
            }
        }


        /// <summary>
        /// Blinks Caster forward. Scales Caster down and
        /// back up during blinking.
        /// </summary>
        /// <param name="blinkVector">Direction</param>
        /// <param name="originalScale">Casters original scale</param>
        private void BlinkForward(Vector2 blinkVector, Vector3 originalScale)
        {
            Vector3 newPosition = (Vector2)this.Caster.transform.position + blinkVector;
            BlinkWithWarp(originalScale, newPosition);
        }


        /// <summary>
        /// Blinks caster to new position with warping effect.
        /// </summary>
        /// <param name="originalScale"></param>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        private async void BlinkWithWarp(Vector3 originalScale, Vector3 newPosition) 
        {
            float originalIntensity = this.floorLigth.intensity;
            float originalRange = this.floorLigth.range;

            LigthEffectsAsync(originalIntensity);

            // Scale Caster down gradually.
            while (this.Caster.transform.localScale.y > 0.2f)
            {
                Vector3 newScale = this.Caster.transform.localScale;
                newScale.y -= 0.1f;
                newScale.x -= 0.1f;
                this.Caster.transform.localScale = newScale;
                await Task.Delay(15); // Delay for gradual scale down effect
            }

            // Move caster and floor ligth effect in between scaling.
            this.Caster.transform.position = newPosition;
            this.floorLigth.transform.position = new Vector3(newPosition.x, newPosition.y,
                floorLigth.transform.position.z);
            this.centerLigth.transform.position = new Vector3(newPosition.x, newPosition.y,
                floorLigth.transform.position.z);

            // Scale caster back up to original scale.
            while (this.Caster.transform.localScale.y < originalScale.y)
            {
                Vector3 newScale = this.Caster.transform.localScale;
                newScale.y += 0.1f;
                newScale.x += 0.1f;
                this.Caster.transform.localScale = newScale;
                await Task.Delay(5); // Delay for gradual scale up effect
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalIntensity"></param>
        /// <returns></returns>
        private async void LigthEffectsAsync(float originalIntensity)
        {
            await this.centerGlowEffect.IntesifyAsync(8f, 0.5f, 1);
            await this.glowEffect.FadeAsync(0.0f, 0.8f, 20);
            await this.glowEffect.IntesifyAsync(originalIntensity, 0.8f, 20);
            await this.centerGlowEffect.FadeAsync(0.0f, 1f, 1);
            Destroy(this.gameObject);
            this.Caster.IsFrozen = false;
        }


        /// <summary>
        /// Set initial values for attributes.
        /// </summary>
        protected override void InitializeSpell()
        {
            this.floorLigth = this.transform.Find("FloorLight").GetComponent<Light>();
            this.glowEffect = this.transform.Find("FloorLight").GetComponent<GlowEffect>();
            this.centerLigth = this.transform.Find("CenterLight").GetComponent<Light>();
            this.centerGlowEffect = this.transform.Find("CenterLight").GetComponent<GlowEffect>();

            this.IsOnCooldown = false;
            this.castRange = 5.0f;
            this.isAgressive = false;
            this.Cooldown = 5.0f;
            BaseCooldown = this.Cooldown;
            base.InitializeSpell();
        }
    }
}

