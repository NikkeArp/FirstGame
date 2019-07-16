using UnityEngine;
using System.Collections;

/// <summary>
/// Camera controller script providing smooth movement and
/// following player functionality.
/// </summary>
public class CameraController : MonoBehaviour
{

#region [Properties]
    [SerializeField] private Player player = null;
    private const float Z_AXIS = -10;
    private Vector3 currentVelocity = Vector3.zero;
    private bool startCameraMoveDone = false;
    public Camera Camera { get; private set; }
 #endregion
#region [Unity API]

    private void Awake() 
    {
        this.Camera = this.GetComponent<Camera>();
        StartCoroutine(StartZoom());
    }

    /// <summary>
    /// Camera movement frame by frame.
    /// </summary>
    private void FixedUpdate() 
    {
        if (this.StartMovementFinished(ref this.startCameraMoveDone))
        {
            this.MovePlayCamera(0.1f);
        }
    }

#endregion

#region [Protected Methods]

    protected IEnumerator StartZoom()
    {
        while (Camera.orthographicSize > 5.0f)
        {
            Camera.orthographicSize -= 0.02f;
            yield return new WaitForSeconds(0.03f);
        }
    }

    /// <summary>
    /// Checks if camera is at its target position with given precision.
    /// </summary>
    /// <param name="targetPos">Camera's target position</param>
    /// <param name="precision">Precision</param>
    /// <returns>True if camera is at its position wiht given precision</returns>
    protected virtual bool CameraAtPosition(Vector3 targetPos, float precision)
    {
        if (Mathf.Abs (this.transform.position.x - targetPos.x) > precision) return false;
        if (Mathf.Abs (this.transform.position.y - targetPos.y) > precision) return false;
        return true;
    }


    /// <summary>
    /// Checks if camera's start movement is finished.
    /// If not moves camera to its start destination and
    /// returns false.
    /// </summary>
    /// <param name="movementDone"></param>
    /// <returns>true if start movement is finished.</returns>
    protected virtual bool StartMovementFinished(ref bool movementDone)
    {
        if (this.startCameraMoveDone) return true;
        if (!this.startCameraMoveDone && this.CameraAtPosition(player.transform.position, 0.1f))
        {
            movementDone = true;
            return true;
        }
        else
        {
            this.MoveCamera(player.transform.position, 1.0f);
        }
        return false;
    }


    /// <summary>
    /// Moves camera to given new position smoothly.
    /// </summary>
    /// <param name="newPos">Vector3 presentation of position in world.</param>
    /// <param name="time">Time to reach position</param>
    protected virtual void MoveCamera(Vector3 newPos, float time)
    {
        newPos.z = Z_AXIS;
        this.transform.position = Vector3.SmoothDamp(this.transform.position,
                newPos, ref this.currentVelocity, time);
    }


    /// <summary>
    /// Moves camera to given new position smoothly. Locks
    /// camera movement to given y-coordinate.
    /// </summary>
    /// <param name="newPos">Vector3 presentation of position in world.</param>
    /// <param name="time">Time to reach position</param>
    /// <param name="lock_Y_AxisPos">Locks cameras y-position</param>
    protected virtual void MoveCamera(Vector3 newPos, float time, float lock_Y_AxisPos)
    {
        newPos.z = Z_AXIS;
        newPos.y = lock_Y_AxisPos;
            this.transform.position = Vector3.SmoothDamp(this.transform.position,
                newPos, ref this.currentVelocity, time);
    }


    /// <summary>
    /// Moves camera around in playmode.
    /// </summary>
    protected virtual void MovePlayCamera(float speed)
    {
        if (player.backAtJumpStartPosition)
        {
            MoveCamera(player.transform.position, speed);
        }
        else
        {
            MoveCamera(player.transform.position, speed, player.y_PositionBeforeJump);
        }
    }

#endregion

}
