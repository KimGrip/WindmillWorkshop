using UnityEngine;
using System.Collections;

public class scr_CanvasStuff : MonoBehaviour 
{
    public GameObject obj;
    public Vector2 pos;

    public Vector2 pos2;

    private RectTransform RT;
    private bool up;
    private GameObject screenMenuButton;

	// Use this for initialization
	void Start () 
    {
        RT = GetComponent<RectTransform>();
        screenMenuButton = GameObject.Find("Options");
        //Debug.Log("found screen menu: " + screenMenuButton);
	}
    public void SetGameMenuState(bool state)
    {
        up = state;
    }
	
	// Update is called once per frame
	void Update () 
    {
        //if (Input.GetMouseButton(0))
        //{
        //    Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    int layer_mask = LayerMask.GetMask("button");
        //    if (Physics2D.Raycast(toMouse.origin, toMouse.direction, 999f, layer_mask))
        //    {
        //        Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
        //        if (obj.name == "Options")
        //        {
        //            up = true;
        //        }
        //    }
        //}


        if (Input.GetKeyDown(KeyCode.Escape) && up == false)
        {
            up = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && up == true)
        {
            up = false;
        }
        if (up == true)
        {
            RT.localPosition = Vector2.MoveTowards
                (RT.localPosition, pos, 5);
        }
        else if (up == false)
        {
            RT.localPosition = Vector2.MoveTowards
                (RT.localPosition, pos2, 5);
        }
    }

}
