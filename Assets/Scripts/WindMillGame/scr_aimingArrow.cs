using UnityEngine;
using System.Collections;

public class scr_aimingArrow : MonoBehaviour {

    private GameObject bag;
    private scr_bagMovement bagMovement;

	// Use this for initialization
	void Start () 
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
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;


        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }
    void AimingArrowSize()
    {
        float size = bagMovement.GetThrowPower();
        transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(size, size, size), 10 * Time.deltaTime);
    }
}
