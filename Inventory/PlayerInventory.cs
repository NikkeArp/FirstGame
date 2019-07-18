using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Players inventory class. Inherits abstarct Inventory
/// base class.
/// </summary>
public sealed class PlayerInventory : Inventory
{
    /// <summary>
    /// Constructor for player's inventory object.
    /// Sets item limit.
    /// </summary>
    public PlayerInventory()
    {
        this.Initialize();
    }

    /// <summary>
    /// Overrides Initialize from Inventory class.
    /// Sets max item count value.
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
        this.MaxItemCount = 15;
    }
}
