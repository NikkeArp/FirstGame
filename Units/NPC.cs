using UnityEngine;
using System.Collections;
using System;

public abstract class NPC : Unit
{
#region [Properties]
    // AI
    public Player HumanPlayer { get; private set; }
#endregion
    
#region [Protected Methods]


    /// <summary>
    /// 
    /// </summary>
    protected override void InitializeUnit()
    {
        base.InitializeUnit();
        this.HumanPlayer = GameObject.Find("Player").GetComponent<Player>();
    }


    /// <summary>
    /// NPC starts patrolling given distance left to rigth.
    /// </summary>
    /// <param name="x_Distance"></param>
    /// <param name="idleTime"></param>
    /// <param name="moveLeft"></param>
    protected virtual void StartPatrolling(float x_Distance, float idleTime, bool moveLeft)
    {
        StartCoroutine(MoveRoutine(x_Distance, idleTime, moveLeft));
    }


    /// <summary>
    /// Slime Move routine. Moves across x-axis.virtual idles for few
    /// seconds in betweent traveling. Jumps just before idling begins.
    /// </summary>
    /// <param name="x_Distance">Move distance</param>
    /// <param name="idleTime">Idle time</param>
    /// <param name="moveLeft">Move direction toggle</param>
    /// <returns></returns>
    protected virtual IEnumerator MoveRoutine(float x_Distance, float idleTime, bool moveLeft) 
    {
        while (true)
        {
            float startPosition = this.transform.position.x;
            while (Math.Abs(startPosition - this.transform.position.x) < x_Distance)
            {   // Distance between start and current position is smaller than required distance
                this.Move(moveLeft ? Vector2.left : Vector2.right);
                UpdateAnimation();
                yield return null; // return control to Unity
            }
            // Unit has reached its goal distance
            this.Idle = true;
            this.Jump(moveLeft ? -1.0f : 1.0f); // Jump with current x-axis velocity
            moveLeft = !moveLeft; // flip the move direction switch
            UpdateAnimation();
            yield return new WaitForSeconds(idleTime);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="minDistance"></param>
    /// <param name="maxDistance"></param>
    protected virtual void FollowPlayer(float minDistance, float maxDistance)
    {
        float distanceBetween = Vector2.Distance(this.transform.position, HumanPlayer.transform.position);

        bool distanceInsideLimits = distanceBetween >= minDistance && distanceBetween <= maxDistance;
        bool SimilarX_Pos = (Math.Abs(this.transform.position.x - this.HumanPlayer.transform.position.x) < 1f);

        if (distanceInsideLimits && !SimilarX_Pos)
        {
            if (this.HumanPlayer.transform.position.x < this.transform.position.x)
            {   // Player is on the left side of the unit
                this.Move(Vector2.left);
            }
            else
            {   // Player is on the left side of the unit
                this.Move(Vector2.right);
            }
        }
        else
        {
            this.Move(Vector2.zero);
        }
    }
#endregion
    
}
