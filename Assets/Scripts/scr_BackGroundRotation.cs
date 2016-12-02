using UnityEngine;
using System.Collections;

public class scr_BackGroundRotation : MonoBehaviour 
{
    public float rotationSpeed;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(transform.eulerAngles, rotationSpeed);
	}
}
