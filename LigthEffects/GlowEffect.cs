using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
using System;

namespace WizardAdventure.Effects
{
    public class GlowEffect : MonoBehaviour
    {

#region [Properties]
        private Light LightEffect = null;
#endregion
#region [UnityAPI]
        private void Awake()
        {
            this.LightEffect = this.GetComponent<Light>();
        }
#endregion
#region Glow

        /// <summary>
        /// Starts the glow effect that last for the lifetime of the object.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">Glow interval</param>
        public void GlowWithVariance(float minGlowIntensity, float interval)
        {
            StartCoroutine(GlowCoroutine(minGlowIntensity, interval));
        }

        /// <summary>
        /// Starts the glow effect that last for the lifetime of the object.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">Glow interval</param>
        /// <param name="time">Time between intensity increment and substraction</param>
        public void GlowWithVariance(float minGlowIntensity, float interval, float time)
        {
            StartCoroutine(GlowCoroutine(minGlowIntensity, interval, time));
        }

        /// <summary>
        /// Starts the glow effect that last for the lifetime of the object.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">Glow interval</param>
        public void Glow(float minGlowIntensity, float interval)
        {
            StartCoroutine(GlowCoroutine(minGlowIntensity, interval));
        }


        /// <summary>
        /// Starts the glow effect that last for the lifetime of the object.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">Glow interval</param>
        /// <param name="time">Time between intensity increment and substraction</param>
        public void Glow(float minGlowIntensity, float interval, float time)
        {
            StartCoroutine(GlowCoroutine(minGlowIntensity, interval, time));
        }

        /// <summary>
        /// Starts Glow effect on slime. Continues whole
        /// objects lifetime.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">Glow interval</param>
        /// <returns></returns>
        private IEnumerator GlowCoroutine(float minIntensity, float glowInterval)
        {
            float noice = UnityEngine.Random.Range(0.8f, 1.2f);
            float originalGlowIntensity = this.LightEffect.intensity;
            while (true) // Loops the ligth intensity back and forth forever.
            {
                while (this.LightEffect.intensity > minIntensity)
                {
                    this.LightEffect.intensity -= glowInterval * noice;
                    LightEffect.intensity = this.LightEffect.intensity;
                    yield return new WaitForSeconds(0.2f); // Wait
                }
                while (this.LightEffect.intensity < originalGlowIntensity)
                {
                    this.LightEffect.intensity += glowInterval * noice;
                    LightEffect.intensity = this.LightEffect.intensity;
                    yield return new WaitForSeconds(0.2f); // wait
                }
            }
        }

        /// <summary>
        /// Starts Glow effect on slime. Continues whole
        /// objects lifetime.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">Glow interval</param>
        /// <param name="time">Time between intensity increment and substraction</param>
        /// <returns></returns>
        private IEnumerator GlowCoroutine(float minIntensity, float glowInterval, float time)
        {
            float noice = UnityEngine.Random.Range(0.8f, 1.2f);
            float originalGlowIntensity = this.LightEffect.intensity;
            while (true) // Loops the ligth intensity back and forth forever.
            {
                while (this.LightEffect.intensity > minIntensity)
                {
                    this.LightEffect.intensity -= glowInterval * noice;
                    LightEffect.intensity = this.LightEffect.intensity;
                    yield return new WaitForSeconds(time); // Wait
                }
                while (this.LightEffect.intensity < originalGlowIntensity)
                {
                    this.LightEffect.intensity += glowInterval * noice;
                    LightEffect.intensity = this.LightEffect.intensity;
                    yield return new WaitForSeconds(time); // wait
                }
            }
        }


    #endregion
#region BlendColors


        public void EnableLight()
        {
            this.LightEffect.gameObject.SetActive(true);
        }

        public void DisableLight()
        {
            this.LightEffect.gameObject.SetActive(false);
        }

        public Color GetColor()
        {
            return this.LightEffect.color;
        }


        public void ChangeColor(Color newColor)
        {
            this.LightEffect.color = newColor;
        }

        /// <summary>
        /// Blends two colors together.
        /// </summary>
        /// <param name="targetObjectsColor">Other color</param>
        /// <param name="newColor">new color by reference</param>
        public void BlendColor(Color targetObjectsColor, out Color newColor)
        {
            newColor = Color.Lerp(targetObjectsColor,
                    this.LightEffect.color, Mathf.PingPong(Time.time, 2f));
            this.LightEffect.color = newColor;
        }

    #endregion
#region Fade
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetIntensity"></param>
        /// <param name="fadeSpeed"></param>
        /// <param name="timeIntervals"></param>
        /// <returns></returns>
        public async Task FadeAsync(float targetIntensity, float fadeSpeed, int timeIntervals)
        {
            while (this.LightEffect.intensity > targetIntensity)
            {
                this.LightEffect.intensity -= fadeSpeed;
                await Task.Delay(timeIntervals);
            }
        }


        /// <summary>
        /// Ligth intensity and range fade to target parameters.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        public void StartFade(float targetIntensity, float targetRange)
        {
            StartCoroutine(this.Fade(targetIntensity, targetRange));
        }

        /// <summary>
        /// Ligth intensity and range fade to target parameters.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <param name="intervals">The amount added to range and intensity</param>
        public void StartFade(float targetIntensity, float targetRange, float intervals)
        {
            StartCoroutine(this.Fade(targetIntensity, targetRange, intervals));
        }

        /// <summary>
        /// Ligth intensity and range fade to target parameters.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <param name="intervals">The amount added to range and intensity</param>
        /// <param name="time">Time between intervals</param>
        public void StartFade(float targetIntensity, float targetRange, float intervals, float time)
        {
            StartCoroutine(this.Fade(targetIntensity, targetRange, intervals));
        }

        /// <summary>
        /// Fade coroutine function. Ligth intensity and range
        /// fade to target parameters.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetIntensity, float targetRange)
        {
            while (this.LightEffect.intensity > targetIntensity)
            {
                this.LightEffect.intensity -= 0.5f;
                if (this.LightEffect.range > targetRange)
                {
                    this.LightEffect.range -= 0.5f;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        /// Fade coroutine function. Ligth intensity and range
        /// fade to target parameters.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <param name="intervals">The amount added to intensity and range</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetIntensity, float targetRange, float intervals)
        {
            while (this.LightEffect.intensity > targetIntensity)
            {
                this.LightEffect.intensity -= intervals;
                if (this.LightEffect.range > targetRange)
                {
                    this.LightEffect.range -= intervals;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        /// Fade coroutine function. Ligth intensity and range
        /// fade to target parameters.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <param name="intervals">The amount added to intensity and range</param>
        /// <param name="time">Time between intervals</param>
        /// <returns></returns>
        public IEnumerator Fade(float targetIntensity, float targetRange, float intervals, float time)
        {
            while (this.LightEffect.intensity > targetIntensity)
            {
                this.LightEffect.intensity -= intervals;
                if (this.LightEffect.range > targetRange)
                {
                    this.LightEffect.range -= intervals;
                }
                yield return new WaitForSeconds(time);
            }
        }
    #endregion
#region Intesify

        /// <summary>
        /// Intesifies light gradually.
        /// </summary>
        /// <param name="targetIntensity">Target intesity</param>
        /// <param name="intervals">Intervals subtracted from range and intesity</param>
        /// <param name="targetRange">Target range</param>
        public void Intesify(float targetIntensity, float intervals, float targetRange)
        {
            StartCoroutine(IntesifyCoroutine(targetIntensity, intervals, targetRange));
        }

        /// <summary>
        /// Intesifies light gradually.
        /// </summary>
        /// <param name="targetIntensity">Target intesity</param>
        /// <param name="intervals">Intervals subtracted from range and intesity</param>
        /// <param name="targetRange">Target range</param>
        /// <returns></returns>
        private IEnumerator IntesifyCoroutine(float targetIntensity, float targetRange, float intervals)
        {
            while (this.LightEffect.intensity < targetIntensity)
            {
                this.LightEffect.intensity += intervals;
                if (this.LightEffect.range < targetRange)
                {
                    this.LightEffect.range += intervals;
                }
                yield return new WaitForSeconds(0.05f);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetIntensity"></param>
        /// <param name="intensifySpeed"></param>
        /// <param name="timeIntervals"></param>
        /// <returns></returns>
        public async Task IntesifyAsync(float targetIntensity, float intensifySpeed, int timeIntervals)
        {
            while (this.LightEffect.intensity < targetIntensity)
            {
                this.LightEffect.intensity += intensifySpeed;
                await Task.Delay(timeIntervals);
            }
        }


        /// <summary>
        /// Intesifies ligth instantly
        /// </summary>
        /// <param name="increseIntesity">Target intensity</param>
        /// <param name="increaseRange">target range</param>
        public void Intesify(float increseIntesity, float increaseRange)
        {
            this.LightEffect.intensity += increseIntesity;
            this.LightEffect.range += increaseRange;
        }
    #endregion
    
    }
}
