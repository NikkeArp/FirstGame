using UnityEngine;
using System.Collections.Generic;
using WizardAdventure.Items;
using System.Text;

/// <summary>
/// Abstract inventory base class. Players
/// items are stored inside inventory. Inventory has
/// maximum capasity.
/// </summary>
public abstract class Inventory
{
#region [Properties]

    /// <summary>
    /// Inventory's max capasity.
    /// </summary>
    /// <value>Get and Set Inventory's max item count.</value>
    protected int MaxItemCount { get; set; }

    /// <summary>
    /// List that holds all the current items in
    /// inventory.
    /// </summary>
    /// <value>Get and Set Inventory's Item list</value>
    protected List<Item> Items { get; set; }

#endregion

#region [Protected Methods]
    
    /// <summary>
    /// Initializes Inventory object's
    /// properties.
    /// </summary>
    protected virtual void Initialize()
    {
        this.Items = new List<Item>();
    }

#endregion

#region [Public Methods]
    
    /// <summary>
    /// Adds items from item stack one by one to inventory.
    /// If inventory is full and there are still items left
    /// in the stack, return false, otherwise return true.
    /// Stack is passed by reference, so possible remaining items
    /// are still there.
    /// </summary>
    /// <param name="items">Item stack</param>
    /// <returns>True if all items are unloaded into inventory</returns>
    public virtual bool AddItems(ref Stack<Item> items)
    {
        // Unloads items from the stack one by one.
        while (this.Items.Count > this.MaxItemCount)
        {
            // Unload items into inventory from item stack.
            this.Items.Add(items.Pop());
        }
        if (items.Count != 0)
        {
            // There are still items left in the stack.
            Debug.Log("Items still left in the stack");
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Adds item into inventory. Return true
    /// if this action was successful, otherwise
    /// returns false.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>True if item is added into inventory</returns>
    public virtual bool AddItem(Item item)
    {
        if (this.Items.Count < this.MaxItemCount)
        {
            this.Items.Add(item);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public virtual bool RemoveItem(Item item)
    {
        return this.Items.Remove(item);
    }

    /// <summary>
    /// Pops the first item from inventory.
    /// FIFO-princible.
    /// </summary>
    /// <returns>Returns the oldest item from inventory.</returns>
    public virtual Item PopItem()
    {
        if (Items.Count > 0)
        {
            Item item = this.Items[0];
            this.Items.RemoveAt(0);
            return item;
        }
        return null;
    }

    /// <summary>
    /// Checks if Inventory has spare space.
    /// </summary>
    /// <returns>
    /// True if there is space remaining, otherwise
    /// returns false.
    /// </returns>
    public bool HasSpace()
    {
        if (this.Items.Count < MaxItemCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Returns the room left in Inventory.
    /// </summary>
    /// <returns>Return remaining space in the Inventory.</returns>
    public int GetSpaceCount()
    {
        return this.MaxItemCount - this.Items.Count;
    }

    /// <summary>
    /// Returns Inventory's contents in string form.
    /// </summary>
    /// <returns>Inventory's contents list</returns>
    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (Item item in Items)
        {
            stringBuilder.Append(item.Name + "\n");
        }
        return stringBuilder.ToString();
    }

#endregion


}
