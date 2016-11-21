using UnityEngine;
using System.Collections;

public class scr_CanvasStuff : MonoBehaviour 
{
    public GameObject obj;
    public Vector2 pos;

    public Vector2 pos2;

    private RectTransform RT;
    private bool up;

	// Use this for initialization
	void Start () 
    {
        RT = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetKeyDown(KeyCode.Escape) && up == false)
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
