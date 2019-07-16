using System.Collections;
using UnityEngine;
using WizardAdventure.Structures;
using WizardAdventure.Effects;

/// <summary>
/// Slime enemy behaviour class. Slimes glow in dark and 
/// roam around in wizard's tower. When two slimes collide,
/// they migth merge together as one big slime. HP and damage is incresed.
/// </summary>
public class Slime : Enemy, ILightUp
{

#region [Properties]
    public GlowEffect GlowLigth;
    public static float BaseMoveSpeed { get; private set; } // monkaS
    private const float GROWTH_INTERVAL  = 0.1f;
    private const float NEW_GROWTH_SCALE = 1.5f;
    private const float MIN_GLOW_INTENSITY = 4.0f;
    private const float GLOW_INTERVAL = 0.2f;
#endregion

#region [UnityAPI]

    /// <summary>
    /// Collision Event
    /// </summary>
    /// <param name="other">Other party in collision</param>
    protected virtual void OnCollisionEnter2D(Collision2D other) 
    {
        // Collision with other slimes makes them fuse together.
        // Also Blends their colors to one.
        if (other.collider.CompareTag("EnemySlime"))
        {
            var otherSlime = other.gameObject.GetComponent<Slime>();
            if(GameController.Instance.AddSlimePair(new SlimeMergePair(this, otherSlime)))
            {
                Color newColor;
                GlowLigth.BlendColor(other.gameObject.GetComponentInChildren<Light>().color, out newColor);
                this.transform.position = Vector2.Lerp(this.transform.position, other.transform.position, 0.5f);
                this.Jump();
                this.Renderer.color = newColor;
                StartCoroutine(Grow(NEW_GROWTH_SCALE, GROWTH_INTERVAL));
            }
        }
        // Check if hitted object was damage source
        else if (other.collider.gameObject.GetComponent<IDamageSource>() != null)
        {
            if (this.OnGotHit(other.collider.gameObject.GetComponent<IDamageSource>(), other.contacts[0].point))
            {
                StopAllCoroutines();
            }
        }
    }

    /// <summary>
    /// First frame
    /// </summary>
    protected override void Start()
    {
        // Changes hitted animation sprite color to match units sprite color
        // dynamically.
        SyncColors(); // Coroutine that runs every half second.

        // Start the glow effect with random variance added
        GlowLigth.GlowWithVariance(MIN_GLOW_INTENSITY, GLOW_INTERVAL, 0.02f);
        //this.StartPatrolling(5.0f, 2.0f, true);
        base.Start();
    }

    /// <summary>
    /// Every frame
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }

#endregion

#region [Protected Methods]

    /// <summary>
    /// Handles slime hitted event. Displays hit animation
    /// and substracts health. If health is below 0,
    /// destroys slime game object.
    /// </summary>
    /// <param name="damageSrc">Damage source</param>
    /// <param name="hitLocation">Where slime got hit</param>
    /// <returns>True if slime dies</returns>
    protected override bool OnGotHit(IDamageSource damageSrc, Vector2 hitLocation)
    {
        if(base.OnGotHit(damageSrc, hitLocation))
        {
            this.Die();
            return true;
        }
        StartCoroutine(DisplayHit(hitLocation));
        return false;
    }

    /// <summary>
    /// Initilizes Ligth-component,
    /// Move constants and health/damage.
    /// </summary>
    protected override void InitializeUnit()
    {
        base.InitializeUnit();
        this.GlowLigth = this.GetComponentInChildren<GlowEffect>();
        this.Damage = 5;
        this.Health = 10;
        this.MoveSpeed = BaseMoveSpeed = 4f;
        this.jumpPowerUp = 5f;
        this.JumpPowerSide =  2f;
    }

    /// <summary>
    /// Sets death animation and ligth fades.
    /// </summary>
    protected override void Die()
    {
        this.GlowLigth.gameObject.SetActive(true);
        this.GlowLigth.StartFade(1.0f, 2.0f);
        base.Die();
    }

#endregion

#region [Public Methods]

    /// <summary>
    /// Jumps up based on objects jump power properties.
    /// </summary>
    /// <param name="x_Axis_movement">X-Axis velocity at the time of the jump</param>
    public override void Jump(float x_Axis_movement)
    {
        base.Jump(x_Axis_movement);
        this.UnitAnimator.SetTrigger("Jump");
    }

    /// <summary>
    /// Get Slime's glow effect object.
    /// </summary>
    /// <returns>Slime's glow effect object</returns>
    public GlowEffect GetEffectController()
    {
        return this.GlowLigth;
    }
    
#endregion

#region [Private Methods]

    /// <summary>
    /// Synchronizes unit's components colors every
    /// half second.
    /// </summary>
    private void SyncColors()
    {
        StartCoroutine(UpdateHittedColor());
    }

    /// <summary>
    /// Starts routine that lasts objects whole lifetime.
    /// Synchronizes units components colors to match.
    /// Runs every half second.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateHittedColor()
    {
        while (true) 
        {
            if (this.ColorChanged)
            {
                var hittedObject = this.transform.Find("Hitted");
                hittedObject.GetComponent<SpriteRenderer>().color = this.Renderer.color;
            }
            yield return new WaitForSeconds(0.5f); // Delay
        }
    }

    /// <summary>
    /// Grows slime gradually to new scale. Growth is linear.
    /// </summary>
    /// <param name="newScale">Scale after growth</param>
    /// <param name="growthInerval">Growth intervals</param>
    /// <returns></returns>
    private IEnumerator Grow(float newScale, float growthInerval)
    {
        Vector3 newScaleVector = new Vector3(transform.localScale.x, transform.localScale.y, 1f);
        while (this.transform.localScale.x < newScale)
        {
            newScaleVector.x = newScaleVector.y += 0.1f;
            this.transform.localScale = newScaleVector;
            yield return new WaitForSeconds(growthInerval);
        }
        newScaleVector.x = newScaleVector.y = newScale;
        this.transform.localScale = newScaleVector;

        // Slime size has grown, so let ramp up the ligthing to match it.
        this.GlowLigth.Intesify(0.4f, 0);
    }

    /// <summary>
    /// Displays slime's hitted animation.
    /// </summary>
    /// <param name="hitLocation">Where slime was hitted</param>
    /// <returns></returns>
    private IEnumerator DisplayHit(Vector2 hitLocation)
    {
        var hittedAnimation = this.transform.Find("Hitted");
        if (hittedAnimation != null)
        {
            var hittedAnimator = hittedAnimation.GetComponent<Animator>();
            hittedAnimation.gameObject.SetActive(true);
            hittedAnimator.SetBool("RightSide", hitLocation.x < this.transform.position.x ? false : true);
            hittedAnimator.SetTrigger("Hit");
            hittedAnimation.transform.position = hitLocation;
            // Fix rigth side position.
            if (hitLocation.x > this.transform.position.x)
            {
                var animationNewPos = hittedAnimation.position;
                animationNewPos.x -= 0.3f;
                hittedAnimation.position = animationNewPos;
            }
            yield return new WaitForSeconds(0.2f);
            if (this.gameObject != null)
            {
                hittedAnimation.gameObject.SetActive(false);
            }
        } 
    }

#endregion

}
