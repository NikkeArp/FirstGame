using UnityEngine;
using System.Collections;

namespace WizardAdventure.Effects
{
    public class GlowEffect
    {
    #region [Properties]
        /// <summary>
        /// Get and set Light component
        /// </summary>
        /// <value>Parent's Light comonpent that this object is controlling.</value>
        public Light LightComponent { get; private set; }
    #endregion
        
        public GlowEffect(Light lightComponent)
        {
            this.LightComponent = lightComponent;
        }

        /// <summary>
        /// Starts glow effect that loops between objects lights current
        /// intensity and minimum intensity. Other values like time waited
        /// each loop are static.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">The amount intensity is changed each time</param>
        /// <returns></returns>
        public IEnumerator RandomGlowCoroutine(float minIntensity, float glowInterval, float noice)
        {
            float originalGlowIntensity = this.LightComponent.intensity;

            // Loops the light intensity back and forth forever.
            while (true) 
            {
                while (this.LightComponent.intensity > minIntensity)
                {
                    this.LightComponent.intensity -= glowInterval * noice;
                    yield return new WaitForSeconds(0.2f); // Wait
                }
                while (this.LightComponent.intensity < originalGlowIntensity)
                {
                    this.LightComponent.intensity += glowInterval * noice;
                    yield return new WaitForSeconds(0.2f); // wait
                }
            }
        }

        /// <summary>
        /// Starts glow effect that loops between objects lights current
        /// intensity and minimum intensity. Other values like time waited
        /// each loop are static.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">The amount intensity is changed each time</param>
        /// <returns></returns>
        public IEnumerator GlowCoroutine(float minIntensity, float glowInterval)
        {
            float originalGlowIntensity = this.LightComponent.intensity;

            // Loops the light intensity back and forth forever.
            while (true) 
            {
                while (this.LightComponent.intensity > minIntensity)
                {
                    this.LightComponent.intensity -= glowInterval;
                    yield return new WaitForSeconds(0.2f); // Wait
                }
                while (this.LightComponent.intensity < originalGlowIntensity)
                {
                    this.LightComponent.intensity += glowInterval;
                    yield return new WaitForSeconds(0.2f); // wait
                }
            }
        }

        /// <summary>
        /// Starts Glow effect that loops between objects current
        /// Intensity and  minimum intensity. Loop time parameter
        /// defines how long it takes to go from maximum intensity to minimimum intensity.
        /// The effect continues whole objects lifetime.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">The amount intensity is changed each time</param>
        /// <param name="loopTime">Time between intensity increment and substraction</param>
        /// <returns></returns>
        public IEnumerator RandomGlowCoroutine(float minIntensity, float glowInterval, float loopTime, float noice)
        {
            int n = Mathf.CeilToInt((this.LightComponent.intensity - minIntensity) / glowInterval * noice);
            float realInterval = (this.LightComponent.intensity - minIntensity) / (float)n;
            float timePerLoop = loopTime / n;

            // Loops the light intensity back and forth forever.
            while (true) 
            {
                for (int i = 0; i < n; i++)
                {
                    this.LightComponent.intensity -= realInterval;
                    yield return new WaitForSeconds(timePerLoop); // Wait
                }
                for (int i = 0; i < n; i++)
                {
                    this.LightComponent.intensity += realInterval;
                    yield return new WaitForSeconds(timePerLoop); // Wait
                }
            }
        }

        /// <summary>
        /// Starts Glow effect that loops between objects current
        /// Intensity and  minimum intensity. Loop time parameter
        /// defines how long it takes to go from maximum intensity to minimimum intensity.
        /// The effect continues whole objects lifetime.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">The amount intensity is changed each time</param>
        /// <param name="loopTime">Time between intensity increment and substraction</param>
        /// <returns></returns>
        public IEnumerator GlowCoroutine(float minIntensity, float glowInterval, float loopTime)
        {
            int n = Mathf.CeilToInt((this.LightComponent.intensity - minIntensity) / glowInterval);
            float realInterval = (this.LightComponent.intensity - minIntensity) / (float)n;
            float timePerLoop = loopTime / n;

            // Loops the light intensity back and forth forever.
            while (true) 
            {
                for (int i = 0; i < n; i++)
                {
                    this.LightComponent.intensity -= realInterval;
                    yield return new WaitForSeconds(timePerLoop); // Wait
                }
                for (int i = 0; i < n; i++)
                {
                    this.LightComponent.intensity += realInterval;
                    yield return new WaitForSeconds(timePerLoop); // Wait
                }
            }
        }

        /// <summary>
        /// Starts Glow effect that loops between maximum intensity
        /// and  minimum intensity. Loop time parameter
        /// defines how long it takes to go from maximum intensity to minimimum intensity.
        /// Continues whole objects lifetime.
        /// </summary>
        /// <param name="minIntensity">Minimum intensity</param>
        /// <param name="maxIntensity">Maximum intensity</param>
        /// <param name="glowInterval">The amount intensity is changed each time</param>
        /// <param name="loopTime">Time it takes to finish the whole loop</param>
        /// <returns></returns>
        public IEnumerator GlowCoroutine(float minIntensity, float maxIntensity, float glowInterval, float loopTime)
        {
            int n = Mathf.CeilToInt((maxIntensity - minIntensity) / glowInterval);
            float realInterval = (maxIntensity - minIntensity) / (float)n;
            float timePerLoop = loopTime / n;

            // Loops the light intensity back and forth forever.
            while (true) 
            {
                for (int i = 0; i < n; i++)
                {
                    this.LightComponent.intensity -= realInterval;
                    yield return new WaitForSeconds(timePerLoop); // Wait
                }
                for (int i = 0; i < n; i++)
                {
                    this.LightComponent.intensity += realInterval;
                    yield return new WaitForSeconds(timePerLoop); // Wait
                }
            }
        }
    }
}