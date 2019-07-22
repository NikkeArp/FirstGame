using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
#region [Properties]

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public bool IsFree { get; set; } = true;

    public PlayerInventory asd {get; set;}
#endregion

    private void Awake() {
        this.asd = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    public void OnPointerEnter(PointerEventData e)
    {
        this.asd.CurrentSlot = this.gameObject;
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (this.asd.CurrentSlot = this.gameObject)
        this.asd.CurrentSlot = null;
    }
}
