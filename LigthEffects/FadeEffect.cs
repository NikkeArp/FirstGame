using UnityEngine;
using System.Collections;

namespace WizardAdventure.Effects
{
    /// <summary>
    /// Fade effect object lowers Unity Light components intensity
    /// and range gradually using coroutines. Functions from this class
    /// must be invoked inside StartCoroutine function in MonoBehaviour objects.
    /// </summary>
    internal class FadeEffect
    {
    #region [Properties]
        /// <summary>
        /// Light component that this object is controlling.
        /// </summary>
        /// <value>Gets the Fade Light Unity component</value>
        public Light LightComponent { get; private set; }
        
        /// <summary>
        /// Sets and Gets Default interval value. This value is
        /// used in light fade loops as the amount substracted from
        /// current intensity or range.
        /// </summary>
        /// <value>Gets the value of default interval</value>
        public float DefaultInterval { get; set; }

        /// <summary>
        /// Sets and Gets Default loop time value. This value is
        /// used in light fade loops as the time in seconds waited
        /// at the end each loop.
        /// </summary>
        /// <value>Gets the value of default loop time</value>
        public float DefaultLoopTime { get; set; }
    #endregion

    /// <summary>
    /// Constructor for Fade effect object.
    /// Fade effect object lowers Unity Light components intensity
    /// and range gradually using coroutines. Functions from this class
    /// must be invoked inside StartCoroutine function in MonoBehaviour objects.
    /// </summary>
    /// <param name="lightComponent">Unity Light component this object effects</param>
    public FadeEffect(Light lightComponent)
    {
        this.LightComponent = lightComponent;

        // Assign default values used in coroutines.
        this.DefaultInterval = 0.01f;
        this.DefaultLoopTime = 0.02f;
    }

        /// <summary>
        /// Basic Fade coroutine function. Light intensity and range
        /// fade to target parameters. Other variables are default.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <returns></returns>
        public IEnumerator BasicFadeCoroutine(float targetIntensity, float targetRange)
        {
            while (this.LightComponent.intensity > targetIntensity
                && this.LightComponent.range > targetRange)
            {
                // Reduce intensity
                if (this.LightComponent.intensity > targetIntensity)
                {
                    this.LightComponent.intensity -= DefaultInterval;
                }
                
                // Reduce range
                if (this.LightComponent.range > targetRange)
                {
                    this.LightComponent.range -= DefaultInterval;
                }

                // Return contorl back to unity
                yield return new WaitForSeconds(DefaultLoopTime); 
            }
        }

        /// <summary>
        /// Fade coroutine function. Light intensity and range
        /// fade to target values defined in parameters. Interval
        /// parameter is the amount the light intensity and range is reduced each
        /// iteration.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <param name="interval">The amount reduced from intensity and range</param>
        /// <returns></returns>
        public IEnumerator FadeCoroutine(float targetIntensity, float targetRange, float interval)
        {
            while (this.LightComponent.intensity > targetIntensity
                && this.LightComponent.range > targetRange)
            {
                // Reduce intensity
                if (this.LightComponent.intensity > targetIntensity)
                {
                    this.LightComponent.intensity -= interval;
                }
                
                // Reduce Range
                if (this.LightComponent.range > targetRange)
                {
                    this.LightComponent.range -= interval;
                }

                // Return controll back to unity
                yield return new WaitForSeconds(DefaultLoopTime);
            }
        }

        /// <summary>
        /// Fade coroutine function. Light intensity and range
        /// fade to target values definned in parameters. Interval
        /// parameter is the amaunt the light intensity and range is reduced each
        /// iteration.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <param name="intensityInterval">The amount reduced from intensity</param>
        /// <param name="rangeInterval">The amount reduced from range</param>
        /// <returns></returns>
        public IEnumerator FadeCoroutine(float targetIntensity, float targetRange, float intensityInterval, float rangeInterval)
        {
            while (this.LightComponent.intensity > targetIntensity
                && this.LightComponent.range > targetRange)
            {
                // Reduce intensity
                if (this.LightComponent.intensity > targetIntensity)
                {
                    this.LightComponent.intensity -= intensityInterval;
                }
                
                // Reduce Range
                if (this.LightComponent.range > targetRange)
                {
                    this.LightComponent.range -= rangeInterval;
                }

                // Return controll back to unity
                yield return new WaitForSeconds(DefaultLoopTime);
            }
        }

        /// <summary>
        /// Fade coroutine function. Light intensity and range
        /// fade to target parameters.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <param name="intensityInterval">The amount reduced from intensity</param>
        /// <param name="rangeInterval">The amount reduced from range</param>
        /// <param name="loopTime">Time between intervals</param>
        /// <returns></returns>
        public IEnumerator FadeCoroutine(float targetIntensity, float targetRange, float intensityInterval, float rangeInterval, float loopTime)
        {
            // Calculate the range of difference in target range and intensity to current values.
            // The smaller of those ranges is needed to evaluate loop iteration amount in this function.
            float rangeRange = this.LightComponent.range - targetRange;
            float intensityRange = this.LightComponent.intensity - targetIntensity;
            float biggerRange = rangeRange >= intensityRange ? rangeRange : intensityRange; 

            int n = Mathf.CeilToInt(biggerRange / intensityInterval);
            float realIntensityInterval = intensityRange / (float)n;
            float realrangeInterval = rangeRange / (float)n;
            float timePerLoop = loopTime / n;

            // This coroutine loop runs fixed 'n' times. Interval amounts are calculated that t
            for (int i = 0; i < n; i++)
            {
                // Reduce light intensity
                if (this.LightComponent.intensity != targetIntensity)
                {
                    this.LightComponent.intensity -= realIntensityInterval;
                }

                // Reduce Light range
                if (this.LightComponent.range > targetRange)
                {
                    this.LightComponent.range -= realrangeInterval;
                }

                yield return new WaitForSeconds(timePerLoop);
            }
        }
    }
}