using UnityEngine;
using System.Collections;

public class scr_urlOpener : MonoBehaviour {
    private Transform jenovaLink;
    private Transform flourLink1;
    private Transform flourLink2;
    public Camera cam;
	// Use this for initialization
	void Start ()
    {
        //jenovaLink = GameObject.Find("JenovaFacebook").transform;
        //flourLink1 = GameObject.Find("FlourFacebook").transform;
        //flourLink2 = GameObject.Find("FlourWordpress").transform;
	
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray toMouse = cam.ScreenPointToRay(Input.mousePosition);
            int layer_mask = LayerMask.GetMask("button");

            if (Physics2D.Raycast(toMouse.origin, toMouse.direction, 999f, layer_mask))
            {
                Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
                if (obj.name == "JenovaFacebook")
                {
                    Application.OpenURL("https://www.facebook.com/JenovaCo/?fref=ts");
                }
                else if (obj.name == "FlourFacebook")
                {
                    Application.OpenURL("https://www.facebook.com/WindmillWorkshop/?fref=ts");
                }
                else if (obj.name == "FlourWordpress")
                {
                    Application.OpenURL("https://windmillworkshopblog.wordpress.com/");
                }
            }
        }
    }
}
