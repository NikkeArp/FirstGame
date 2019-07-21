using UnityEngine;
using System.Threading.Tasks;
using System;
using WizardAdventure.Spells;

/// <summary>
/// This class contains player functinality for Player-GameObject.
/// </summary>
public sealed class Player : Unit
{
#region [Properties]

    /// <summary>
    /// Player's inventory object, that holds all the items
    /// player gathers from the gameworld.
    /// </summary>
    /// <value>Get and Set PLayer's inventory</value>
    public PlayerInventory Inventory { get; private set; }

    /// <summary>
    /// Player's spellbook object, that handles
    /// casting spells.
    /// </summary>
    /// <value>Get and Set player's spellbook</value>
    public SpellBook SpellBook { get; private set; }

    /// <summary>
    /// Jump Downtime for player gameobject.
    /// This flag makes sure player can't spam
    /// jump.
    /// </summary>
    /// <value>Get and Set Player's jump downtime</value>
    public bool JumpDownTime { get; private set; }

    /// <summary>
    /// Player's y-position before jumping. This is used to
    /// calculate when player is back at the same level after jumping.
    /// </summary>
    /// <value>Get and Set player's y-position before the jump.</value>
    public float Y_PositionBeforeJump { get; private set; }

    /// <summary>
    /// Flag for indicating that player is back
    /// at jump position in y-axis.
    /// </summary>
    /// <value>
    /// Get and Set player's flag for being back at jump start level
    /// </value>
    public bool BackAtJumpStartPosition { get; private set; }

    /// <summary>
    /// Player's jump downtime in milliseconds.
    /// </summary>
    /// <value>Get and set player's jump downtime in milliseconds</value>
    public int JumpDownTimeMS { get; private set; }

    /* /// <summary>
    /// Players inventory. Inventory holds all the
    /// loot player gathers.
    /// </summary>
    /// <value>Get and Set player's inventory.</value>
    public PlayerInventory Inventory { get; private set; } */

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
        if (Tags.TagsContainOneTag(other.gameObject.tag, "Ground", "Enemy"))
        {
            this.MoveState = MovementState.LANDED;
            await Task.Delay(TimeSpan.FromMilliseconds(JumpDownTimeMS));
            this.JumpDownTime = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        if (Input.anyKeyDown)
        {
            // Toggle inventory on and off
            this.ToggleInventory();

            // Cast a spell
            this.SpellBook.AttemptCast();
        }
    }

    /// <summary>
    /// Physics movements. Updates every frame.
    /// </summary>
    protected override void FixedUpdate()
    {
        // Checks if player is back at y-Position after jumping.
        if (this.transform.position.y <= this.Y_PositionBeforeJump)
            this.BackAtJumpStartPosition = true;

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
    /// Toggles inventory if I-key is pressed down.
    /// </summary>
    private void ToggleInventory()
    {
        // Inventory hotkey pressed.
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryController.Instance.InventoryMoving)
            {
                return;
            }

            // Change inventory's active state.
            this.Inventory.ToggleInventory();

            // Set inventory active or inactive based on
            // inventory's active state.
            this.Inventory.Activate(this.Inventory.InventoryActive);
        }
    }

    /// <summary>
    /// Moves player either in x-axis or jumps.
    /// </summary>
    private void MovePLayer(float horizontal)
    {     
        if (Input.GetKey(KeyCode.W) && !JumpDownTime && this.MoveState != MovementState.AIR) 
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
        this.Y_PositionBeforeJump = this.transform.position.y;
        this.BackAtJumpStartPosition = false;
        base.Jump(x_AxisMovement);
        this.JumpDownTime = true;
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
        this.Inventory = this.GetComponent<PlayerInventory>();

        this.SpellBook = this.GetComponentInChildren<SpellBook>();
        this.SpellBook.Caster = this;

        this.IsFrozen = false;

        this.Y_PositionBeforeJump = this.transform.position.y;
        this.BackAtJumpStartPosition = true;
        
        this.MoveSpeed = 3.2f;
        this.jumpPowerUp = 10.0f;
        this.JumpPowerSide = 3.0f;
        base.InitializeUnit();
    }

#endregion
}
