﻿using UnityEngine;
using System.Collections;


public class scr_EquipmentSlot : MonoBehaviour 
{
    private bool isEmpty;
    private GameObject attachedObject;
    private scr_AlchemyShop AS;
    
	void Start () 
    {
        AS = GetComponentInParent<scr_AlchemyShop>();
        isEmpty = true;
	}
	void Update () 
    {
	    if(attachedObject != null)
        {
            isEmpty = false;
            attachedObject.transform.position = transform.position;
        }
	}
    public bool GetEquipmentSlotStatus()
    {
        return isEmpty;
    }
    public void SetEquipmentSlotStatus(bool p_isEmpty)
    {
        isEmpty = p_isEmpty;
    }
    void OnTriggerEnter2D(Collider2D colli)
    {
        if(colli.gameObject.tag == "potion" && isEmpty)
        {
            attachedObject = colli.gameObject;
            AS.AddItemToEquipment(attachedObject);
            Debug.Log("adding");
        }
    }
    void OnTriggerExit2D(Collider2D colli)
    {
        if (colli.gameObject.tag == "potion" && !isEmpty)
        {
            AS.ResetPotionPos(attachedObject);
            attachedObject = null;
            isEmpty = true;
        }
    }
    public void SetAttachedPotion(GameObject obj)
    {
        attachedObject = obj;
    }
    public GameObject GetAttachedPotion()
    {
        return attachedObject;
    }

}
