using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Vector3 Scale { get; private set; }

    private PlayerInventory inventory;

    private void Awake() {
        this.Scale = this.transform.localScale;
        inventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        this.transform.localScale = new Vector3(
            this.Scale.x + 0.3f, this.Scale.y + 0.3f, this.Scale.z);
    }

    public void OnDrag(PointerEventData eventData)
    {
        GameObject slot = inventory.CurrentSlot;
        if (slot != null)
        {
            this.transform.SetParent(slot.transform.GetChild(0));
        }
        this.transform.position = eventData.position;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    public void OnEndDrag(PointerEventData e)
    {
        this.transform.localScale = this.Scale;
        GameObject slot = inventory.CurrentSlot;
        if (slot != null && slot.GetComponent<Slot>().IsFree)
        {
            this.transform.SetParent(slot.transform.GetChild(0));
            this.transform.position = this.transform.parent.position;
        }
        else
        {
            this.transform.position = this.transform.parent.position;
        }
    }
}
