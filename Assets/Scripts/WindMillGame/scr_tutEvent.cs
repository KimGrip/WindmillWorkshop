using UnityEngine;
using System.Collections;

public class scr_tutEvent : MonoBehaviour {

    private scr_GameManager GM;
    private scr_CanvasStuff CS;
    public GameObject trigger1;
    public GameObject trigger2;

    // Use this for initialization
    void Start () {

        GM = GetComponent<scr_GameManager>();
        CS = GameObject.Find("PopUpMenu").GetComponent<scr_CanvasStuff>();
	}
	public void PauseForEvent()
    {
        CS.gameObject.SetActive(false);
        GM.SetEndGameStateMenu(true);
        Time.timeScale = 0;
        if (trigger1 == true)
        {

        }
    }
    
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PauseForEvent();    
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            CS.gameObject.SetActive(true);
            GM.SetEndGameStateMenu(false);

        }
    }
}
