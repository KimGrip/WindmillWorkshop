using UnityEngine;
using System.Collections;

public class scr_aimingArrow : MonoBehaviour {

    private GameObject bag;
    private scr_bagMovement bagMovement;

	// Use this for initialization
	void Awake () 
    {
        bag = GameObject.FindGameObjectWithTag("bag");
        bagMovement = bag.GetComponent<scr_bagMovement>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        AimingArrowRotation();
        AimingArrowSize();
	}
    void AimingArrowRotation()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

		Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
		mousePos.x = mousePos.x - bag.transform.position.x;
		mousePos.y = mousePos.y - bag.transform.position.y;


        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		Vector3 BagTransform = new Vector3 (bag.transform.position.x, bag.transform.position.y);
		Vector3 ArrowTransform = new Vector3 (transform.position.x, transform.position.y);
		Quaternion lookAt = Quaternion.LookRotation(BagTransform - ArrowTransform);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, lookAt, 0f);
       
    }
    void AimingArrowSize()
    {
        float size = bagMovement.GetThrowPower();
        transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(size, size, size), 10 * Time.deltaTime);
    }
}
