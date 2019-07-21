using UnityEngine;

/// <summary>
/// Inventory slot data structure. Keep's track
/// to slot's occupation state. Holds the slot GameObject.
/// </summary>
public struct InventorySlot
{
#region [Properties]
    /// <summary>
    /// Slot's occupation state.
    /// </summary>
    /// <value>Get and Set Slot's occupation state</value>
    public bool IsEmpty { get; set; }

    /// <summary>
    /// Slot's Gameobject
    /// </summary>
    /// <value>Get and Set Slot's GameObject</value>
    public GameObject Slot { get; private set; }
#endregion
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isEmpty"></param>
    /// <param name="slot"></param>
    public InventorySlot(bool isEmpty, GameObject slot)
    {
        this.IsEmpty = isEmpty;
        this.Slot = slot;
    }
}
