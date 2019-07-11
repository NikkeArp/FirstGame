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

    public bool colorChanged = false;

    private Color originalColor;

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

#region [Public Methods]

    /// <summary>
    /// Returns the center point of the game object.
    /// Type params should be either SpriteRenderer or
    /// Collider2D.
    /// </summary>
    /// <typeparam name="T">SpriteRenderer or Collider2D</typeparam>
    /// <returns>Center point of either SpriteRenderer or Collider2D. If
    /// Type param doesnt match, return empty vector.</returns>
    public Vector3 GetCenterPoint<T>()
    {
        Vector3 centerPoint;
        if (typeof(T) == typeof(SpriteRenderer))
        {
            // Unit's renderer's center point vector.
            centerPoint = this.Renderer.bounds.center;
        }
        else if (typeof(T) == typeof(Collider2D))
        {
            // Unit's collider's center point vector.
            centerPoint = this.UnitCollider.bounds.center;
        }
        else
        {
            // Type parameter doesn't match.
            centerPoint = Vector3.zero;
        }
        return centerPoint;
    }

    /// <summary>
    /// Returns the size of the game object.
    /// Type params should be either SpriteRenderer or
    /// Collider2D.
    /// </summary>
    /// <typeparam name="T">SpriteRenderer or Collider2D</typeparam>
    /// <returns>Size of either SpriteRenderer or Collider2D. If
    /// Type param doesnt match, return empty vector.</returns>
    public Vector3 GetSize<T>()
    {
        Vector3 size;
        if (typeof(T) == typeof(SpriteRenderer))
        {
            // Unit's renderer's size vector.
            size = this.Renderer.bounds.size;
        }
        else if (typeof(T) == typeof(Collider2D))
        {
            // Unit's collider's size vector.
            size = this.UnitCollider.bounds.size;
        }
        else 
        {
            // Type parameter doesn't match.
            size = Vector3.zero;
        }
        return size;
    }

    /// <summary>
    /// Get the Bounds struct of either the units SpriteRenderer
    /// or Collider2D object. Type param should always be either
    /// SpriteRenderer or Collider2D Type.
    /// </summary>
    /// <typeparam name="T">SpriteRenderer or Collider2D</typeparam>
    /// <returns>Bounds struct of either SpriteRenderer or Collider2D. If
    /// Type param doesnt match, return new Bounds object</returns>
    public Bounds GetBounds<T>()
    {
        Bounds result;
        if (typeof(T) == typeof(SpriteRenderer))
        {
            // Unit's renderer's Bounds.
            result = this.Renderer.bounds;
        }
        else if (typeof(T) == typeof(Collider2D))
        {
            // unit's collider's Bounds.
            result = this.UnitCollider.bounds;
        }
        else 
        {
            // Type parameter doesn't match.
            result = new Bounds();
        }
        return result;
    }

    /// <summary>
    /// Returns the depth of the game object.
    /// Type params should be either SpriteRenderer or
    /// Collider2D.
    /// </summary>
    /// <typeparam name="T">SpriteRenderer or Collider2D</typeparam>
    /// <returns>depth of either SpriteRenderer or Collider2D. If
    /// Type param doesnt match, returns -99.9</returns>
    public float GetDepth<T>()
    {
        float depth;
        if (typeof(T) == typeof(SpriteRenderer))
        {
            // Unit's renderer's depth.
            depth = this.Renderer.bounds.size.z;
        }
        else if (typeof(T) == typeof(Collider2D))
        {
            // Unit's collider's depth.
            depth = this.UnitCollider.bounds.size.z;
        }
        else 
        {
            // type parameter doesn't match.
            depth = -99.9f;
        }
        return depth;
    }

    /// <summary>
    /// Returns the width of the game object.
    /// Type params should be either SpriteRenderer or
    /// Collider2D.
    /// </summary>
    /// <typeparam name="T">SpriteRenderer or Collider2D</typeparam>
    /// <returns>Width of either SpriteRenderer or Collider2D. If
    /// Type param doesnt match, returns -99.9</returns>
    public float GetWidth<T>()
    {
        float width;
        if (typeof(T) == typeof(SpriteRenderer))
        {
            // Unit's renderer's width.
            width = this.Renderer.bounds.size.x;
        }
        else if (typeof(T) == typeof(Collider2D))
        {
            // Unit's collider's width.
            width = this.UnitCollider.bounds.size.x;
        }
        else 
        {
            // Type parameter doesn't match.
            width = -99.9f;
        }
        return width;
    }

    /// <summary>
    /// Returns the Height of the game object.
    /// Type params should be either SpriteRenderer or
    /// Collider2D.
    /// </summary>
    /// <typeparam name="T">SpriteRenderer or Collider2D</typeparam>
    /// <returns>Height of either SpriteRenderer or Collider2D. If
    /// Type param doesnt match, returns -99.9</returns>
    public float GetHeight<T>()
    {
        float height;
        if (typeof(T) == typeof(SpriteRenderer))
        {
            // Units renderer's height.
            height = this.Renderer.bounds.size.y;
        }
        else if (typeof(T) == typeof(Collider2D))
        {
            // Units collider's height.
            height = this.UnitCollider.bounds.size.y;
        }
        else 
        {
            // Type parmameter doesnt match.
            height = -99.9f;
        }
        return height;
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Color GetColor()
    {
        return this.Renderer.color;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    public void ChangeColor(Color color)
    {
        originalColor = this.Renderer.color;
        this.Renderer.color = color;
        this.colorChanged = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetColor()
    {
        this.Renderer.color = originalColor;
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