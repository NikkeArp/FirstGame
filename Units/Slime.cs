using System;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using WizardAdventure.Structures;
using WizardAdventure.Projectiles;
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
        // Slime got hitted by projectile.
        Projectile projectile;
        if (other.gameObject.TryGetComponent<Projectile>(out projectile))
        {
            OnGotHit(projectile);
        }
    }

    /// <summary>
    /// First frame
    /// </summary>
    protected override void Start()
    {
        // Start the glow effect with random variance added
        GlowLigth.GlowWithVariance(MIN_GLOW_INTENSITY, GLOW_INTERVAL, 0.02f);
        this.StartPatrolling(5.0f, 2.0f, true);
        base.Start();
    }

    /// <summary>
    /// Every frame
    /// </summary>
    protected override void Update()
    {
        // Movement and behaviour
        //this.FollowPlayer(1.2f, 10.0f); 
        base.Update();
    }


#endregion

#region [Protected Methods]

    /// <summary>
    /// Jumps up based on objects jump power properties.
    /// </summary>
    /// <param name="x_Axis_movement">X-Axis velocity at the time of the jump</param>
    public override void Jump(float x_Axis_movement)
    {
        base.Jump(x_Axis_movement);
        this.UnitAnimator.SetTrigger("Jump");
    }


    public GlowEffect GetEffectController()
    {
        return this.GlowLigth;
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

#endregion

#region [Private Methods]

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
    /// GotHit event handler.
    /// </summary>
    /// <param name="projectile">Projectile that hitted object</param>
    /// <returns></returns>
    private async void OnGotHit(Projectile projectile)
    {
        this.Health -= projectile.damage;
        {
            if (this.Health <= 0)
            {   // Death animation and ligth is fading. Object is destroyed.
                this.GlowLigth.StartFade(1.0f, 2.0f);
                this.UnitAnimator.SetTrigger("Death");
                Destroy (this.gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
                return;
            }
        }

        var hittedAnimation = this.transform.Find("Hitted");
        if (hittedAnimation != null)
        {
            var hittedAnimator = hittedAnimation.GetComponent<Animator>();
            hittedAnimation.gameObject.SetActive(true);
            hittedAnimator.SetBool("RightSide", projectile.transform.position.x < this.transform.position.x ? false : true);
            hittedAnimator.SetTrigger("Hit");
            hittedAnimation.transform.position = projectile.transform.position;
            // Fix rigth side position.
            if (projectile.transform.position.x > this.transform.position.x)
            {
                var animationNewPos = hittedAnimation.position;
                animationNewPos.x -= 0.3f;
                hittedAnimation.position = animationNewPos;
            }
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            if (this.gameObject != null)
            {
                hittedAnimation.gameObject.SetActive(false);
            }
        }
    }
#endregion
}
