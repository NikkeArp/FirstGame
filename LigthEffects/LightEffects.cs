using UnityEngine;
using System.Collections;

namespace WizardAdventure.Effects
{
    /// <summary>
    /// This Script enables easy manipulation of its gameObjects light component
    /// </summary>
    public class LightEffects : MonoBehaviour
    {
    #region [Properties]
        public Light LightComponent { get; private set; }

        /// <summary>
        /// Get and Set FadeEffect object that has the functionality to glow
        /// this scripts parent objects light.
        /// </summary>
        /// <value>Get and Set GlowEffect object</value>
        public Glow GlowEffect { get; private set; }

        /// <summary>
        /// Get and Set FadeEffect object that has the functionality to fade
        /// this scripts parent objects light.
        /// </summary>
        /// <value>Get and Set FadeEffect object</value>
        internal FadeEffect FadeEffect { get; private set; }

        /// <summary>
        /// Gets and Sets IntenisfyEffect object. Intensify
        /// effect object increases Unity Light components intensity
        /// and range gradually using coroutines.
        /// </summary>
        /// <value>Get and Set intensifyEffect object</value>
        internal IntensifyEffect IntensifyEffect { get; private set; }

        /// <summary>
        /// Gets and Sets ColorEffect object. Color
        /// effect object manipulates lights colors gradually using
        /// coroutines.
        /// </summary>
        /// <value>Get and Set ColorEffect object</value>
        internal ColorEffect ColorEffect { get; private set; }
    #endregion

    #region [Unity API]

        /// <summary>
        /// Awake is called at the start of this game object's lifetime.
        /// Initializes effect objects.
        /// </summary>
        private void Awake() 
        {
            this.LightComponent = this.GetComponent<Light>();
            this.ColorEffect = new ColorEffect(this.gameObject.GetComponent<Light>());
            this.GlowEffect = new Glow(this.gameObject.GetComponent<Light>());
            this.FadeEffect = new FadeEffect(this.gameObject.GetComponent<Light>());
            this.IntensifyEffect = new IntensifyEffect(this.gameObject.GetComponent<Light>());
        }
        
        /// <summary>
        /// Start is called at the first frame.
        /// Begin effects here.
        /// </summary>
        private void Start()
        {
        }

    #endregion

    #region [Public Methods]

        /// <summary>
        /// Begins to change color to target color.
        /// </summary>
        /// <param name="targetColor">Target color</param>
        public void BeginColorChange(Color targetColor)
        {
            this.StartCoroutine(ColorEffect.ChangeColorCoroutine(targetColor));
        }

        /// <summary>
        /// Begins to blend current light color and specified color.
        /// Blend ratio is default 50:50.
        /// </summary>
        /// <param name="otherColor">Other color participating in the blend</param>
        public void BeginColorBlend(Color otherColor)
        {
            this.StartCoroutine(ColorEffect.BlendColorCoroutine(otherColor));
        }

        /// <summary>
        /// Begins to blend current light color and specified color.
        /// Blend ratio determines wich of the two colors is stronger.
        /// Example:
        ///     Blend ratio is 0.0f => original color 100%, other color 0%
        ///     Blend ratio is 0.5f => original color 50%, other color 50%
        ///     Blend ratio is 1.0f => original color is 0%, other color is 100%
        /// </summary>
        /// <param name="otherColor">Other color participating in blend operation</param>
        /// <param name="blendRatio">Blend ratio</param>
        public void BeginColorBlend(Color otherColor, float blendRatio)
        {
            this.StartCoroutine(this.ColorEffect.BlendColorCoroutine(otherColor, blendRatio));
        }

        /// <summary>
        /// Begins to blend current light color and specified color.
        /// Blend ratio determines wich of the two colors is stronger.
        /// Example:
        ///     Blend ratio is 0.0f => original color 100%, other color 0%
        ///     Blend ratio is 0.5f => original color 50%, other color 50%
        ///     Blend ratio is 1.0f => original color is 0%, other color is 100%
        /// </summary>
        /// <param name="otherColor">Other color participating in blend operation</param>
        /// <param name="blendRatio">Blend ratio</param>
        /// <param name="loopTime">Time waited each loop</param>
        public void BeginColorBlend(Color otherColor, float blendRatio, float loopTime)
        {
            this.StartCoroutine(this.ColorEffect.BlendColorCoroutine(otherColor, blendRatio, loopTime));
        }

        /// <summary>
        /// Sets LightEffect light component active.
        /// </summary>
        public void EnableLight()
        {
            this.LightComponent.gameObject.SetActive(true);
        }

        /// <summary>
        /// Sets LightEffect light component inactive.
        /// </summary>
        public void DisableLight()
        {
            this.LightComponent.gameObject.SetActive(false);
        }

        /// <summary>
        /// Gets  light component color.
        /// </summary>
        /// <returns></returns>
        public Color GetColor()
        {
            return this.LightComponent.color;
        }

        /// <summary>
        /// Changes light component's color to specified color.
        /// </summary>
        /// <param name="newColor">New LightEffect light color</param>
        public void ChangeColor(Color newColor)
        {
            this.LightComponent.color = newColor;
        }

        /// <summary>
        /// Basic Intensify coroutine function. Light intensity and range
        /// Intensify to target parameters. Other variables are default.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        public void BasicFadeCoroutine(float targetIntensity, float targetRange)
        {
            this.StartCoroutine(this.IntensifyEffect.BasicFadeCoroutine(targetIntensity, targetRange));
        }

        /// <summary>
        /// Intensify coroutine function. Light intensity and range
        /// fade to target values defined in parameters. Interval
        /// parameter is the amount the light intensity and range is increased each
        /// iteration.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <param name="interval">The amount increased from intensity and range</param>
        public void Intensify(float targetIntensity, float targetRange, float interval)
        {
           this.StartCoroutine(this.IntensifyEffect.IntensifyCoroutine(targetIntensity,  targetRange,  interval));
        }

        /// <summary>
        /// Intensify coroutine function. Light intensity and range
        /// intensify to target values defined in parameters. Interval
        /// parameter is the amaunt the light intensity and range is increased each
        /// iteration.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <param name="intensityInterval">The amount increased from intensity</param>
        /// <param name="rangeInterval">The amount increased from range</param>
        public void Intensify(float targetIntensity, float targetRange, float intensityInterval, float rangeInterval)
        {
           this.StartCoroutine(this.IntensifyEffect.IntensifyCoroutine(targetIntensity,  targetRange,  intensityInterval,  rangeInterval));
        }

        /// <summary>
        /// Intensify coroutine function. Light intensity and range
        /// Intenisfy to target parameters.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <param name="intensityInterval">The amount increased from intensity</param>
        /// <param name="rangeInterval">The amount increased from range</param>
        /// <param name="loopTime">Time between intervals</param>
        public void Intensify(float targetIntensity, float targetRange, float intensityInterval, float rangeInterval, float loopTime)
        {
            this.StartCoroutine(this.IntensifyEffect.IntensifyCoroutine( targetIntensity,
                targetRange,  intensityInterval,  rangeInterval,  loopTime));
        }


        /// <summary>
        /// Fade coroutine function. Light intensity and range
        /// fade to target values definned in parameters. Interval
        /// parameter is the amount the light intensity and range is reduced each
        /// iteration.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        /// <param name="interval">The amount reduced from intensity and range</param>
        public void BeginFade(float targetIntensity, float targetRange, float interval)
        {
            this.StartCoroutine(this.FadeEffect.FadeCoroutine(targetIntensity, targetRange, interval));
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
        public void BeginFade(float targetIntensity, float targetRange, float intensityInterval, float rangeInterval)
        {
            this.StartCoroutine(this.FadeEffect.FadeCoroutine(targetIntensity, targetRange,
                intensityInterval, rangeInterval));
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
        public void BeginFade(float targetIntensity, float targetRange, float intensityInterval, float rangeInterval, float loopTime)
        {
            this.StartCoroutine(this.FadeEffect.FadeCoroutine(targetIntensity, targetRange,
                intensityInterval, rangeInterval, loopTime));
        }

        /// <summary>
        /// Fade coroutine function. Light intensity and range
        /// fade to target parameters.
        /// </summary>
        /// <param name="targetIntensity">Target intensity</param>
        /// <param name="targetRange">Target range</param>
        public void BeginFade(float targetIntensity, float targetRange)
        {
            this.StartCoroutine(this.FadeEffect.BasicFadeCoroutine(targetIntensity, targetRange));
        }

        /// <summary>
        /// Starts the glow effect that last for the lifetime of the object.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">The amount intensity is changed each time</param>
        public void GlowWithVariance(float minGlowIntensity, float interval)
        {
            float randomNoice =  UnityEngine.Random.Range(0.8f, 1.2f);
            this.StartCoroutine(this.GlowEffect.RandomGlowCoroutine(minGlowIntensity, interval, randomNoice));
        }

        /// <summary>
        /// Starts the glow effect that last for the lifetime of the object.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">The amount intensity is changed each time</param>
        public void BeginGlow(float minGlowIntensity, float interval)
        {
            this.StartCoroutine(this.GlowEffect.GlowCoroutine(minGlowIntensity, interval));
        }

        /// <summary>
        /// Starts the glow effect that last for the lifetime of the object.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">The amount intensity is changed each time</param>
        /// <param name="time">Time between intensity increment and substraction</param>
        public void BeginGlow(float minIntensity, float interval, float time)
        {
            this.StartCoroutine(this.GlowEffect.GlowCoroutine(minIntensity, interval, time));
        }

        /// <summary>
        /// Starts the glow effect that last for the lifetime of the object.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="maxnIntensity">Maximum glow intensity</param>
        /// <param name="glowInterval">The amount intensity is changed each time</param>
        /// <param name="time">Time between intensity increment and substraction</param>
        public void BeginGlow(float minIntensity, float maxIntensity, float interval, float time)
        {
            this.StartCoroutine(this.GlowEffect.GlowCoroutine(minIntensity, maxIntensity, interval, time));
        }

        /// <summary>
        /// Starts the glow effect that last for the lifetime of the object.
        /// </summary>
        /// <param name="minIntensity">Minimun glow intensity</param>
        /// <param name="glowInterval">The amount intensity is changed each time</param>
        /// <param name="time">Time between intensity increment and substraction</param>
        public void GlowWithVariance(float minGlowIntensity, float interval, float time)
        {
            float randomNoice = UnityEngine.Random.Range(0.8f, 1.2f);
            this. StartCoroutine(this.GlowEffect.RandomGlowCoroutine(minGlowIntensity, interval, time, randomNoice));
        }
        
    #endregion

    }
}