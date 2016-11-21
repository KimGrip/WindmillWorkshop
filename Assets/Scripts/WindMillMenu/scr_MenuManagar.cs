using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scr_MenuManagar : MonoBehaviour {

    private List<GameObject> menuButtons;
	// Use this for initialization
	void Start () 
    {
	    menuButtons = new List<GameObject>();
        menuButtons.Add(GameObject.Find("ResumeGame"));
        menuButtons.Add(GameObject.Find("QuitGame"));
        menuButtons.Add(GameObject.Find("StartGame"));

	}
	// THIS SCRIPT SHOULD CONTAIN ALL MENU FUNCTIONS
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics2D.Raycast(toMouse.origin, toMouse.direction).transform != null)
            {
                Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
                if(obj.name == "QuitGame")
                {
                    Application.Quit();
                }
                else if (obj.name == "StartGame")
                {
                    Application.LoadLevel(1);

                }
            }
        }
	}
}
