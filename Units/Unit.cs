using UnityEngine;
using System.Collections.Generic;
using WizardAdventure.Effects;

public enum DebuffName
{
    Chilled
}

public abstract class Unit : MonoBehaviour, IMoveable
{
#region [Properties]

    public int Health { get; set; }
    public int Damage { get; set; }
    public bool IsFrozen { get; set; }

    public float MoveSpeed { get; set; }

    public List<DebuffName> ActiveDebuffs { get; protected set; }
    public GlowEffect DebuffEffect;

    protected float jumpPowerUp;
    protected float JumpPowerSide;

    public bool FaceRigth { get; protected set; } = true;
    public bool Idle      { get; protected set; } = true;

    protected Rigidbody2D    Rigidbody    { get; private set; }
    protected Animator       UnitAnimator {  get; private set; }
    protected Collider2D     UnitCollider { get; private set; }
    public MovementState     MoveState    { get; set; } = MovementState.NULL;
    protected SpriteRenderer Renderer     { get; private set; }

#endregion
#region [UnityAPI]

    protected virtual void Awake() 
    {
        InitializeUnit();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update() 
    {
        UpdateAnimation();
    }

    protected virtual void FixedUpdate()
    {
        
    }

#endregion
#region [Protected Methods]

    /// <summary>
    /// Sets boolean values for unit's animator for "FaceRigth" and "Idle"
    /// </summary>
    protected virtual void UpdateAnimation()
    {
        this.UnitAnimator.SetBool("FaceRigth", this.FaceRigth);
        this.UnitAnimator.SetBool("Idle", this.Idle);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector2 direction)
    {
        if (Mathf.Approximately(direction.y , 0.0f))
        {   // Horizontal move only!
            this.FaceRigth = direction.x > 0.0f ? true : false;
            this.Idle = Mathf.Approximately(direction.x, 0.0f) ? true : false;
            this.transform.Translate(direction * this.MoveSpeed * Time.deltaTime);
        }
        else 
        {
            this.Idle = true;
        }
    }

    public Color GetColor()
    {
        return this.Renderer.color;
    }

    public void ChangeColor(Color color)
    {
        this.Renderer.color = color;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x_AxisMovement"></param>
    public virtual void Move(float x_AxisMovement)
    {
        if (!Mathf.Approximately(x_AxisMovement, 0.0f))
        {
            Vector2 newVelocity = this.Rigidbody.velocity;
            newVelocity.x += 1f * x_AxisMovement;
            if (newVelocity.x > this.MoveSpeed)
            {
                newVelocity.x = this.MoveSpeed;
            }
            else if (newVelocity.x < -this.MoveSpeed)
            {
                newVelocity.x = -this.MoveSpeed;
            }
            this.Rigidbody.velocity = newVelocity;
            this.Idle = false;
        }
        else
        {
            this.Idle = true;
        }
        this.FaceRigth = x_AxisMovement > 0.0f ? true : false;
    }


    /// <summary>
    /// 
    /// </summary>
    public virtual void Jump()
    {
        this.Rigidbody.velocity = new Vector2(0, this.jumpPowerUp);
        this.Idle = true;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="x_Axis_movement"></param>
    public virtual void Jump(float x_Axis_movement)
    {
        float jumpPowerSide;
        if (Mathf.Approximately(x_Axis_movement, 0.0f))
        {
            jumpPowerSide = 0.0f;
            this.FaceRigth = true;
        }
        else if (x_Axis_movement < 0.0f)
        {
            jumpPowerSide = -this.JumpPowerSide;
            this.FaceRigth = false;
        }
        else
        {
            jumpPowerSide = this.JumpPowerSide;
        }
        this.Idle = true;
        this.Rigidbody.velocity = new Vector2(jumpPowerSide, this.jumpPowerUp);
    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void InitializeUnit()
    {
        this.DebuffEffect = this.transform.Find("DebuffEffect")?.GetComponent<GlowEffect>();
        this.ActiveDebuffs = new List<DebuffName>();
        this.Rigidbody = this.GetComponent<Rigidbody2D>();
        this.UnitAnimator = this.GetComponent<Animator>();
        this.Renderer = this.GetComponent<SpriteRenderer>();
        this.UnitCollider = this.GetComponent<Collider2D>();
    }

#endregion
}