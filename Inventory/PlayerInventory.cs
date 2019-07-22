#define DEBUG

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using WizardAdventure.Items;
using UnityEngine.EventSystems;

/// <summary>
/// Players inventory class. Inherits abstarct Inventory
/// base class.
/// </summary>
public sealed class PlayerInventory : Inventory
{
#region [Properties]
    public GameObject inventory;

    /// <summary>
    /// Boolean flag value if inventory's active state.
    /// </summary>
    /// <value>Get and Set inventory's activity flag</value>
    public bool InventoryActive { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public GameObject[] InventorySlots { get; private set; }

    /// <summary>
    /// Dictionary that holds item prefabs.
    /// item id:s are keys.
    /// </summary>
    /// <typeparam name="string">Item ID</typeparam>
    /// <typeparam name="GameObject">Item Prefab</typeparam>
    /// <returns>Get and Set Item dictionary.</returns>
    public Dictionary<string, GameObject> ItemTable { get; set; }

    public GameObject CurrentSlot { get; set; }

#endregion


#region [Protected Methods]

    /// <summary>
    /// Overrides Initialize from Inventory class.
    /// Sets max item count value.
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
        this.MaxItemCount = 20;

        // Set item prefabs to dictionary.
        this.ItemTable = new Dictionary<string, GameObject>();
        this.FillItemTable();

        // Initialize Slot struct array.
        this.FillSlotObjects();
        this.InventoryActive = true;
    }

    

#endregion

#region [Public Methods]

    /// <summary>
    /// Toggles inventory's active flag.
    /// </summary>
    public void ToggleInventory()
    {
        this.InventoryActive = !this.InventoryActive;
    }

    /// <summary>
    /// Sets inventory active based on flag specified
    /// in the parameters.
    ///     flag == true => Activate inventory.
    ///     flag == false => Deactivate inventory.
    /// </summary>
    /// <param name="flag">boolean flag</param>
    public void Activate(bool flag)
    {
        if (flag)
        {
            InventoryController.Instance.MoveUp();
        }
        else
        {
            InventoryController.Instance.MoveDown();
        }
    }

#endregion

#region [Private Methods]
    /// <summary>
    /// Set all the inventory slots into
    /// InventorySlot array.
    /// </summary>
    private void FillSlotObjects()
    {
        this.InventorySlots = new GameObject[this.MaxItemCount];
        for (int i = 0; i < this.MaxItemCount; i++)
        {
            this.InventorySlots[i] = this.inventory.transform.GetChild(i).gameObject;
        }
    }

    /// <summary>
    /// Fills item prefab table with item id - itemprefab pairs.
    /// </summary>
    private void FillItemTable()
    {
        this.ItemTable.Add("8ef780361b64437cbbe559c593f7c3de", this.GreaterHealingPotionPrefab);
        this.ItemTable.Add("c5aab76932f645188725a768d948deb6", this.GreaterManaPotionPrefab);
        this.ItemTable.Add("ca48495a75b9468fa770bdc39a42dfe7", this.SwiftnessPotionPrefab);
    }

    /// <summary>
    /// Creates item to inventory. Searches inventory item's prefab
    /// from dictionary using item id as the key. If prefab is found,
    /// instantiates prefab as a child of the first free inventory slot.
    /// Set's item's sprite to slot's image. Marks slot occupied.
    /// </summary>
    /// <param name="id">Item Id</param>
    public void CreateItem(string id)
    {
        // Search for the item prefab.
        GameObject inventoryItemPrefab;
        this.ItemTable.TryGetValue(id, out inventoryItemPrefab);

        // If prefab is found
        if (inventoryItemPrefab != null)
        {
            // Go through all the slots and find the first unoccupied slot.
            for (int i = 0; i < this.MaxItemCount; i++)
            {
                var currentSlot = this.InventorySlots[i].GetComponent<Slot>();
                if (currentSlot.IsFree)
                {
                    // Instantiate inventory item and set it as a child of current slot.
                    GameObject inventoryItem = Instantiate(inventoryItemPrefab, currentSlot.transform.GetChild(0).position, Quaternion.identity);
                    inventoryItem.transform.SetParent(currentSlot.transform.GetChild(0));

                    // Slot is no longer empty
                    this.InventorySlots[i].GetComponent<Slot>().IsFree = false;
                    return;
                }
            }
        }
#if (DEBUG)
        else // Prefab was not found.
        {
            Debug.Log($"Item Prefab '{id}' not found.\nItem Creation failed.");
        }
#endif
    }

#endregion

}
