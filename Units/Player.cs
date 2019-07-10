using UnityEngine;
using System.Threading.Tasks;
using System;
using WizardAdventure.Projectiles;
using WizardAdventure.Spells;

/// <summary>
/// This class contains player functinality for Player-GameObject.
/// </summary>
public class Player : Unit
{
#region [Properties]
    public GameObject FireballRigthGameObj;
    public GameObject FireballLeftGameObj;

    public GameObject Blink;

    public SpellBook spellBook;

    private bool jumpDownTime = false;
    public float y_PositionBeforeJump;
    public bool backAtJumpStartPosition;
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
        if (SpellEventManager.Instance.GlobalCooldown)
        {
            return;
        }
            
        if (Input.GetKeyDown(KeyCode.R))
        {
            this.UnitAnimator.SetTrigger("Shoot");
            if (this.spellBook.CastSpell<Frostblast>(this.FaceRigth))
            {
                SpellEventManager.Instance.SetGlobalCooldown();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.UnitAnimator.SetTrigger("Shoot");
            if(this.spellBook.CastSpell<Blink>(this.FaceRigth))
            {
                SpellEventManager.Instance.SetGlobalCooldown();
            }
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
            if ((Input.GetKeyDown(KeyCode.Mouse0)))
                this.Shoot();
            this.Idle = false;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {   
            this.Shoot();
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


    /// <summary>
    /// Sets animator trigger shoot active.
    /// </summary>
    private async void Shoot()
    {
        if (GameController.Instance.PlayerCanShoot) 
        {
            this.UnitAnimator.SetTrigger("Shoot");
            GameController.Instance.PlayerShoot();
            await Task.Delay((TimeSpan.FromMilliseconds(Fireball.CAST_TIME)));

            Vector3 projectilePos = this.transform.position;
            projectilePos.y -= 0.10f;
            float x_Axis;
            GameObject fireball;
            if (this.FaceRigth)
            {
                projectilePos.x += 0.35f;
                x_Axis = 1.0f;
                fireball = Instantiate(FireballRigthGameObj, projectilePos + Vector3.up * 0.5f, Quaternion.identity);
            }
            else
            {
                projectilePos.x -= 0.35f;
                x_Axis = -1.0f;
                fireball = Instantiate(FireballLeftGameObj, projectilePos + Vector3.up * 0.5f, Quaternion.identity);
            }
            Fireball fb = fireball.GetComponent<Fireball>();
            fb?.Launch(new Vector2(x_Axis, 0.0f));
        }
    }
#endregion
#region [Public Methods]

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
        spellBook.Caster = this;
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
