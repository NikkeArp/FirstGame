using UnityEngine;
using System.Threading.Tasks;
using WizardAdventure.Effects;

namespace WizardAdventure.Spells
{
    /// <summary>
    /// Blink is castable utility spell that teleports
    /// caster at this facing direction. If there is an 
    /// obstacle in the way, blink transports caster right before it.
    /// </summary>
    public class Blink : UtilitySpell
    {

    #region [Properties]
        /// <summary>
        /// Blink's base cooldown. Effects all Blink objects.
        /// YOU SHOULD NOT EDIT THIS!!
        /// No one should. Ever. Just let it be.
        /// </summary>
        /// <value>Gets and Sets Blink base cooldown</value>
        new public static float BaseCooldown { get; private set; }

        /// <summary>
        /// Light component in the floor.
        /// </summary>
        /// <value>Gets and Sets light component</value>
        public Light FloorLight { get; private set; }

        /// <summary>
        /// Light component in the center of the blink gameobject.
        /// </summary>
        /// <value>Gets and Sets light component</value>
        public Light CenterLight { get; private set; }

        /// <summary>
        /// LightEffects script handles this spell's 
        /// light effects.
        /// </summary>
        /// <value>Get and Set light effect script</value>
        public LightEffects FloorLightEffect { get; private set; }

        /// <summary>
        /// LightEffects script handles this spell's 
        /// light effects.
        /// </summary>
        /// <value>Get and Set light effect script</value>
        public LightEffects CenterLightEffect { get;  private set; }
    #endregion

    #region [Public Methods]

        /// <summary>
        /// Casts blink. Blink transports caster to new position based on given distance and
        /// direction caster is facing. While blinking, casters scale is
        /// lowered when cast starts, and brougth back up when blinking is done.
        /// </summary>
        /// <param name="direction">Direction caster is facing</param>
        public override void Cast(Unit caster, bool faceRigth)
        {
            if (SpellEventManager.Instance.BlinkOnCooldown)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                this.Caster = caster;
                Vector2 blinkVector = faceRigth ? new Vector2(this.CastRange, 0) : new Vector2(-this.CastRange, 0);
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

    #endregion
    
    #region [Protected]

        /// <summary>
        /// Set initial values for attributes.
        /// </summary>
        protected override void InitializeSpell()
        {
            this.FloorLight = this.transform.Find("FloorLight").GetComponent<Light>();
            this.CenterLight = this.transform.Find("CenterLight").GetComponent<Light>();
            this.FloorLightEffect = this.transform.Find("FloorLight").GetComponent<LightEffects>();
            this.CenterLightEffect = this.transform.Find("CenterLight").GetComponent<LightEffects>();

            this.CastRange = 5.0f;
            this.IsAgressive = false;
            this.Cooldown = BaseCooldown =  5.0f;
            base.InitializeSpell();
        }
    
    #endregion

    #region [Private Methods]
        
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
                    if (distance < this.CastRange)
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
            float originalIntensity = this.FloorLight.intensity;
            float originalRange = this.FloorLight.range;

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
            this.FloorLight.transform.position = new Vector3(newPosition.x, newPosition.y,
                FloorLight.transform.position.z);
            this.CenterLight.transform.position = new Vector3(newPosition.x, newPosition.y,
                FloorLight.transform.position.z);

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
            await this.CenterLightEffect.IntensifyEffect.IntesifyAsync(8f, 0.5f, 1);
            await this.FloorLightEffect.FadeEffect.FadeAsync(0.0f, 0.8f, 20);
            await this.FloorLightEffect.IntensifyEffect.IntesifyAsync(originalIntensity, 0.8f, 20);
            await this.CenterLightEffect.FadeEffect.FadeAsync(0.0f, 1f, 1);
            Destroy(this.gameObject);
            this.Caster.IsFrozen = false;
        }
    #endregion

    }
}

