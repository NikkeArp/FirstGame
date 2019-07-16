using UnityEngine;
using System.Threading.Tasks;
using System;
using WizardAdventure.Spells;

/// <summary>
/// This class contains player functinality for Player-GameObject.
/// </summary>
public class Player : Unit
{
#region [Properties]
    public SpellBook spellBook;
    private bool jumpDownTime = false;
    [HideInInspector] public float y_PositionBeforeJump;
    [HideInInspector] public bool backAtJumpStartPosition;
    private const int JUMP_DOWNTIME = 100;

#endregion
#region [UnityAPI]

    /// <summary>
    /// Players collision eventhandler. If player hits ground,
    /// method updates movestatus and runs jump downtime before
    /// setting it to false.
    /// </summary>
    /// <param name="other">Other gameobject in collision</param>
    /// <returns></returns>
    private async void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.collider.CompareTag("Ground"))
        {
            this.MoveState = MovementState.LANDED;
            await Task.Delay(TimeSpan.FromMilliseconds(JUMP_DOWNTIME));
            this.jumpDownTime = false;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        if (Input.anyKeyDown)
        {
            this.spellBook.AttemptCast();
        }
    }


    /// <summary>
    /// Physics movements. Updates every frame.
    /// </summary>
    protected override void FixedUpdate()
    {
        // Checks if player is back at y-Position after jumping.
        if (this.transform.position.y <= this.y_PositionBeforeJump)
            this.backAtJumpStartPosition = true;

        float horizontal;
        if (PlayerMoved(out horizontal))
        {
            this.MovePLayer(horizontal);
            this.Idle = false;
        }
        else
        {
            this.Idle = true;
        }
        this.UpdateAnimation();
    }

#endregion
#region [Private Methods]

    /// <summary>
    /// Moves player either in x-axis or jumps.
    /// </summary>
    private void MovePLayer(float horizontal)
    {     
        if (Input.GetKey(KeyCode.W) && !jumpDownTime && this.MoveState != MovementState.AIR) 
        {   // Player is pressing up, jump is not on cooldown and character state is not AIR
            float jumpPowerSide = horizontal == 0.0f ? 0.0f : (horizontal < 0.0f) ? -this.jumpPowerUp : this.JumpPowerSide;
            this.Jump(jumpPowerSide);
        }
        else if(!Mathf.Approximately(horizontal, 0.0f) && this.MoveState != MovementState.AIR || this.Rigidbody.velocity.y < 0)
        {   // Player is Moving and not in air.
            this.Move(horizontal);
        }
    } 


    /// <summary>
    /// Checks if player can move.
    /// Horizontal input is passed by reference.
    /// </summary>
    /// <param name="horizontal">Horizontal input</param>
    /// <returns>True if palyer can move.</returns>
    private bool PlayerMoved(out float horizontal)
    {
        horizontal = Input.GetAxis("Horizontal");
        if (this.IsFrozen) return false;
        float verical = Input.GetAxis("Vertical");
        if (Mathf.Approximately(horizontal, 0.0f) && Mathf.Approximately(verical, 0.0f))
        {
            return false;
        }
        this.FaceRigth = horizontal > 0.0f ? true : false;
        return true;
    }


#endregion
#region [Public Methods]

    /// <summary>
    /// 
    /// </summary>
    public override void SetCastAnimation()
    {
        this.UnitAnimator.SetTrigger("Shoot");
    }

    /// <summary>
    /// Override for Unit.Jump(). Stores y-Position before the jump.
    /// Updates jump downtime and movestate for player.
    /// </summary>
    /// <param name="x_AxisMovement">Movement in x axis</param>
    public override void Jump(float x_AxisMovement)
    {
        this.y_PositionBeforeJump = this.transform.position.y;
        this.backAtJumpStartPosition = false;
        base.Jump(x_AxisMovement);
        this.jumpDownTime = true;
        this.MoveState = MovementState.AIR;
    }

#endregion
#region [Protected Methdos]

    /// <summary>
    /// Override for Initialization. Sets values for
    /// movespeed and jump powers.
    /// </summary>
    protected override void InitializeUnit()
    {
        this.spellBook = this.GetComponentInChildren<SpellBook>();
        this.spellBook.Caster = this;

        this.IsFrozen = false;

        this.y_PositionBeforeJump = this.transform.position.y;
        this.backAtJumpStartPosition = true;
        
        this.MoveSpeed = 3.2f;
        this.jumpPowerUp = 10.0f;
        this.JumpPowerSide = 3.0f;
        base.InitializeUnit();
    }

#endregion
}
