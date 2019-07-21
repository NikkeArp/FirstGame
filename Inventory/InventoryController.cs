using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryController : MonoBehaviour
{
#region [Properties]

    public bool InventoryMoving { get; private set; }

    /// <summary>
    /// Inventory's down position y-coordinate.
    /// </summary>
    /// <value>Get and Set down position</value>
    public float DownPosition { get; private set; }

    /// <summary>
    /// Inventory's up position y-coordinate.
    /// </summary>
    /// <value>Get and Set up position</value>
    public float UpPosition { get; private set; }

    /// <summary>
    /// Transform
    /// </summary>
    /// <value>Get and Set inventory's transform.</value>
    public RectTransform Transform { get; private set; }

    /// <summary>
    /// Static instance of this class, available
    /// to all. Only one of these may ever exist.
    /// </summary>
    /// <value>Get and Set instance of this class.</value>
    public static InventoryController Instance { get; private set; } = null;
#endregion
    
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        if (Instance is null) 
        {
            Instance = this;
        }
        else if (Instance != this) 
        {
            Destroy(gameObject);
        }

        // Set Transform property.
        this.Transform = this.GetComponent<RectTransform>();

        // Set up and down positions.
        this.DownPosition = -220f;
        this.UpPosition = 200f;
    }

    /// <summary>
    /// Moves inventory down, out of the view.
    /// </summary>
    public void MoveDown()
    {
        this.InventoryMoving = true;
        this.StartCoroutine(this.MoveDown(this.DownPosition, -1f));
    }

    /// <summary>
    /// Moves inventory up, into the view.
    /// </summary>
    public void MoveUp()
    {
        this.InventoryMoving = true;
        this.StartCoroutine(this.MoveUp(this.DownPosition, 1f));
    }

    /// <summary>
    /// Move coroutine function that moves inventory
    /// up and down depending on the predicate specified in the
    /// parameters.
    /// </summary>
    /// <param name="destination">Destination y-coordinate</param>
    /// <param name="limit">Predicate</param>
    /// <returns></returns>
    private IEnumerator MoveUp(float destination, float move)
    {
        float multiplier = 1.1f;
        float moveSpeed = move * multiplier; 
        Vector3 startPosition = this.Transform.position;
        while(Vector3.Distance(startPosition, this.Transform.position) < 300)
        {
            Vector3 previousPos = this.Transform.position;
            this.Transform.position = new Vector3(previousPos.x, previousPos.y + moveSpeed, previousPos.z);
            moveSpeed *= multiplier;
            yield return null;
        }
        this.InventoryMoving = false;
    }

    /// <summary>
    /// Move coroutine function that moves inventory
    /// up and down depending on the predicate specified in the
    /// parameters.
    /// </summary>
    /// <param name="destination">Destination y-coordinate</param>
    /// <param name="limit">Predicate</param>
    /// <returns></returns>
    private IEnumerator MoveDown(float destination, float move)
    {
        float multiplier = 1.1f;
        float moveSpeed = move * multiplier; 
        Vector3 startPosition = this.Transform.position;
        while(Vector3.Distance(startPosition, this.Transform.position) < 300)
        {
            Vector3 previousPos = this.Transform.position;
            this.Transform.position = new Vector3(previousPos.x, previousPos.y + moveSpeed, previousPos.z);
            moveSpeed *= multiplier;
            yield return null;
        }
        this.InventoryMoving = false;
    }
}
