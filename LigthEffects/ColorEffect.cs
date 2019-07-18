using UnityEngine;
using System.Collections;
using System;

namespace WizardAdventure.Effects
{
    /// <summary>
    /// Class for controlling light component's
    /// color changes.
    /// </summary>
    internal class ColorEffect
    {

    #region [Properties]

        /// <summary>
        /// Get and set Light component
        /// </summary>
        /// <value>Parent's Light comonpent that this object is controlling.</value>
        public Light LightComponent { get; private set; }

        /// <summary>
        /// Sets and Gets Default loop time value. This value is
        /// used in light fade loops as the time in seconds waited
        /// at the end each loop.
        /// </summary>
        /// <value>Gets the value of default loop time</value>
        public float DefaultLoopTime { get; set; } = 0.02f;

        /// <summary>
        /// Sets and Gets default interval used in this object. Interval
        /// that is added or substracted from RGBA-values.
        /// </summary>
        /// <value>Gets the value of default interval</value>
        public float DefaultInterval { get; set; } = 0.02f;

    #endregion

    #region [Constructors]

        /// <summary>
        /// Constructor for Color effect object.
        /// Color effect object manipulates Unity Light components RGBA-color
        /// values gradually using coroutines. Functions from this class
        /// must be invoked inside StartCoroutine function in MonoBehaviour objects.
        /// </summary>
        /// <param name="lightComponent">Unity Light component this object effects</param>
        public ColorEffect(Light lightComponent)
        {
            this.LightComponent = lightComponent;
        }

        /// <summary>
        /// Constructor for Color effect object.
        /// Color effect object manipulates Unity Light components RGBA-color
        /// values gradually using coroutines. Functions from this class
        /// must be invoked inside StartCoroutine function in MonoBehaviour objects.
        /// </summary>
        /// <param name="lightComponent">Unity Light component this object effects</param>
        public ColorEffect(Light lightComponent, float loopTime)
        {
            this.LightComponent = lightComponent;
            this.DefaultLoopTime = loopTime;
        }
        
    #endregion

    #region [Public Methods]

        /// <summary>
        /// Begins to blend current light color and specified color.
        /// Uses default 50:50 blend ratio.
        /// Example:
        ///     Blend ratio is 0.0f => original color 100%, other color 0%
        ///     Blend ratio is 0.5f => original color 50%, other color 50%
        ///     Blend ratio is 1.0f => original color is 0%, other color is 100% 
        /// </summary>
        /// <param name="otherColor">Other color in blending operation</param>
        /// <returns></returns>
        public IEnumerator BlendColorCoroutine(Color otherColor)
        {
            // Determine target color by blending current and other color.
            Color targetColor = Color.Lerp(this.LightComponent.color, otherColor, 0.5f);

            // Array of the amounts that each color changes. This is used to find out
            // the smallest change range.
            float smallestRange = 0f;
            float[] rangeArray = createRangeArray(targetColor, ref smallestRange);

            // Create individual intervals for each colorvalue
            rangeArray = createPrivateIntervals(rangeArray, smallestRange);

            // Changes color to target color over many frames.
            Color newColor = this.LightComponent.color;
            while (!newColor.Equals(targetColor))
            {
                newColor.r = newColor.r > targetColor.r ? newColor.r -= rangeArray[0] : targetColor.r;
                newColor.g = newColor.g > targetColor.g ? newColor.g -= rangeArray[1] : targetColor.g;
                newColor.b = newColor.b > targetColor.b ? newColor.b -= rangeArray[2] : targetColor.b;
                newColor.a = newColor.a > targetColor.a ? newColor.a -= rangeArray[3] : targetColor.a;

                this.LightComponent.color = newColor;
                // Wait default wait time before continuing.
                yield return new WaitForSeconds(this.DefaultLoopTime);
            }
        }

        /// <summary>
        /// Begins to blend current light color and specified color.
        /// Blend ratio determines wich of the two colors is stronger.
        /// Example:
        ///     Blend ratio is 0.0f => original color 100%, other color 0%
        ///     Blend ratio is 0.5f => original color 50%, other color 50%
        ///     Blend ratio is 1.0f => original color is 0%, other color is 100% 
        /// </summary>
        /// <param name="otherColor">Other color in blending operation</param>
        /// <param name="blendRatio">Blend ratio</param>
        /// <returns></returns>
        public IEnumerator BlendColorCoroutine(Color otherColor, float blendRatio)
        {
            Color targetColor = Color.Lerp(this.LightComponent.color, otherColor, blendRatio);
            
            // Array of the amounts that each color changes. This is used to find out
            // the smallest change range.
            float smallestRange = 0;
            float[] rangeArray = createRangeArray(targetColor, ref smallestRange);

            // Create individual intervals for each colorvalue
            rangeArray = createPrivateIntervals(rangeArray, smallestRange);

            // Changes color to target color over many frames.
            Color newColor = this.LightComponent.color;
            while (!newColor.Equals(targetColor))
            {
                newColor.r = newColor.r > targetColor.r ? newColor.r -= rangeArray[0] : targetColor.r;
                newColor.g = newColor.g > targetColor.g ? newColor.g -= rangeArray[1] : targetColor.g;
                newColor.b = newColor.b > targetColor.b ? newColor.b -= rangeArray[2] : targetColor.b;
                newColor.a = newColor.a > targetColor.a ? newColor.a -= rangeArray[3] : targetColor.a;

                this.LightComponent.color = newColor;

                // Wait default wait time before continuing.
                yield return new WaitForSeconds(this.DefaultLoopTime);
            }
        }

        /// <summary>
        /// Begins to blend current light color and specified color.
        /// Blend ratio determines wich of the two colors is stronger.
        /// Example:
        ///     Blend ratio is 0.0f => original color 100%, other color 0%
        ///     Blend ratio is 0.5f => original color 50%, other color 50%
        ///     Blend ratio is 1.0f => original color is 0%, other color is 100% 
        /// </summary>
        /// <param name="otherColor">Other color in blending operation</param>
        /// <param name="blendRatio">Blend ratio</param>
        /// <param name="loopTime">Time waited each loop</param>
        /// <returns></returns>
        public IEnumerator BlendColorCoroutine(Color otherColor, float blendRatio, float loopTime)
        {
            Color targetColor = Color.Lerp(this.LightComponent.color, otherColor, blendRatio);
            
            // Array of the amounts that each color changes. This is used to find out
            // the smallest change range.
            float smallestRange = 0;
            float[] rangeArray = createRangeArray(targetColor, ref smallestRange);

            // Create individual intervals for each colorvalue
            rangeArray = createPrivateIntervals(rangeArray, smallestRange);

            // Changes color to target color over many frames.
            Color newColor = this.LightComponent.color;
            while (!newColor.Equals(targetColor))
            {
                newColor.r = newColor.r > targetColor.r ? newColor.r -= rangeArray[0] : targetColor.r;
                newColor.g = newColor.g > targetColor.g ? newColor.g -= rangeArray[1] : targetColor.g;
                newColor.b = newColor.b > targetColor.b ? newColor.b -= rangeArray[2] : targetColor.b;
                newColor.a = newColor.a > targetColor.a ? newColor.a -= rangeArray[3] : targetColor.a;

                this.LightComponent.color = newColor;

                // Wait default wait time before continuing.
                yield return new WaitForSeconds(loopTime);
            }
        }

        /// <summary>
        /// Changes color of the light component to 
        /// match specified color. Change happends gradually through
        /// multiple frames. Uses default wait time.
        /// </summary>
        /// <param name="targetColor">Target color</param>
        /// <returns></returns>
        public IEnumerator ChangeColorCoroutine(Color targetColor)
        {
            // Array of the amounts that each color changes. This is used to find out
            // the smallest change range.
            float smallestRange = 0;
            float[] rangeArray = createRangeArray(targetColor, ref smallestRange);

            // Create individual intervals for each colorvalue
            rangeArray = createPrivateIntervals(rangeArray, smallestRange);

            // Changes color to target color over many frames.
            Color newColor = this.LightComponent.color;
            while (!newColor.Equals(targetColor))
            {
                newColor.r = newColor.r > targetColor.r ? newColor.r -= rangeArray[0] : targetColor.r;
                newColor.g = newColor.g > targetColor.g ? newColor.g -= rangeArray[1] : targetColor.g;
                newColor.b = newColor.b > targetColor.b ? newColor.b -= rangeArray[2] : targetColor.b;
                newColor.a = newColor.a > targetColor.a ? newColor.a -= rangeArray[3] : targetColor.a;

                this.LightComponent.color = newColor;

                // Wait default wait time before continuing.
                yield return new WaitForSeconds(this.DefaultLoopTime);
            }
        }

    #endregion

    #region [Private Methods]

        /// <summary>
        /// Creates individual interval values for each color
        /// values. These values replace values in RGB-range array.
        /// Same array is returned.
        /// </summary>
        /// <param name="array">Array containing RGB-color values change ranges</param>
        /// <param name="smallestRange">smallest value in RGB-color change ranges</param>
        /// <returns>Array containing individual interval values for each RGB-color value</returns>
        private float[] createPrivateIntervals(float[] array, float smallestRange)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = smallestRange / array[i] * this.DefaultInterval;
            }
            return array;
        }

        /// <summary>
        /// Creates change range value array.
        /// Finds smallest range.
        /// </summary>
        /// <param name="targetColor">Target color</param>
        /// <param name="smallestRange">Smallest of calculated ranges</param>
        /// <returns>Array containing RGBA-color values change ranges</returns>
        private float[] createRangeArray(Color targetColor, ref float smallestRange)
        {
            float[] rangeArray = { Math.Abs(this.LightComponent.color.r - targetColor.r),
                Math.Abs(this.LightComponent.color.g - targetColor.g),
                Math.Abs(this.LightComponent.color.b - targetColor.b),
                Math.Abs(this.LightComponent.color.a - targetColor.a) };
            
            smallestRange = rangeArray[0];
            for (int i = 1; i < rangeArray.Length; i++)
            {
                if (rangeArray[i] > smallestRange)
                {
                    smallestRange = rangeArray[i];
                }
            }
            return rangeArray;
        }

    #endregion

    }
}