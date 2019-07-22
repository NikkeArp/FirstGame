using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private PlayerInventory inventory;

    private void Awake() {
        inventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    public void OnEndDrag(PointerEventData e)
    {
        GameObject slot = inventory.CurrentSlot;
        if (slot != null)
        {
            Debug.Log("Move to new parent");
            this.transform.SetParent(slot.transform);
            this.transform.position = this.transform.parent.position;
        }
        else
        {
            Debug.Log("back to original parent");
            this.transform.position = this.transform.parent.position;
        }
    }
}
