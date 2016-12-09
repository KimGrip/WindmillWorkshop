using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class scr_CameraPan : MonoBehaviour
{
    private Transform anchor1;
    private Transform anchor2;
    private Vector3 camPos;
    public float panSpeed;
    private bool isPan = true;
    public LayerMask mouseHitLayer;

	// Use this for initialization
	void Start ()
    {
        anchor1 = transform.GetChild(0);
        anchor2 = GameObject.Find("cameraAnchor2").transform;
        camPos = this.transform.position;
        this.transform.position = anchor1.position;
    }

    // Update is called once per frame
    void Update ()
    {
        if (isPan == true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, anchor2.position, panSpeed);
            if (this.transform.position == anchor2.position)
            {
                isPan = false;
            }

        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
                //int layer_mask = LayerMask.GetMask("button");

                if (Physics2D.Raycast(toMouse.origin, toMouse.direction, 999f, mouseHitLayer))
                {
                    Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
                    if (obj.name == "mainMenu")
                    {
                        SceneManager.LoadScene("Main_Menu");
                    }
                }
            }
        }

    }
}
