using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



[System.Serializable]
public struct shop_Upgrade
{
    public int Cost;
    public GameObject Upgrade;
    public bool isBought;
    public Vector2 position;
    
};
public class scr_WindMillShop : MonoBehaviour 
{
    public List<shop_Upgrade> l_shopUpgrades;
    private scr_shopUI shopUi;

    public GameObject buyConfirmationBox;



    private GameObject m_confirmationBox;


    private Vector3 confirmationBoxPos;
    private bool confirmationBoxActive;
    private int upgradeIndex;
    //dictionary

    //Needs unique cost for each
    //bool or int when bought
    //needs to modify values already there

	// Use this for initialization
	void Start () 
    {


        shopUi = GameObject.FindGameObjectWithTag("shop").GetComponent<scr_shopUI>();
        for(int i = 0; i < l_shopUpgrades.Count; i ++)
        {
         
            SpriteRenderer sUSR = l_shopUpgrades[i].Upgrade.GetComponent<SpriteRenderer>();
            Color m_color = Color.red;
            sUSR.color = m_color;
        }
	}
    void GoToMenu()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics2D.Raycast(toMouse.origin, toMouse.direction).transform != null)
            {
                Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
                if (obj.name == "GoToMenu")
                {
                    Application.LoadLevel(0);
                }
            }
        }
    }
	void EnableBuyBox(Vector3 pos, int index)
    {
        if (m_confirmationBox == null)
        {
            m_confirmationBox = (GameObject)Instantiate(buyConfirmationBox, pos, Quaternion.identity);
        }

        else //box is there, need to find its childobjkect by mouseposition
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics2D.Raycast(toMouse.origin, toMouse.direction).transform != null)
                {
                    Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
                    if (obj.name == "a")
                    {
                        if(shopUi.GetCurrency() >= l_shopUpgrades[index].Cost)
                        {
                            SpriteRenderer sr = l_shopUpgrades[index].Upgrade.GetComponent<SpriteRenderer>();
                            sr.color = Color.green;
                            confirmationBoxActive = false;
                            Destroy(m_confirmationBox);
                        }
                    }
                    if (obj.name == "d")
                    {
                        confirmationBoxActive = false;
                        Destroy(m_confirmationBox);
                        
                    }
                }
            }
        }
    }
    void UpgradeSelecter()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics2D.Raycast(toMouse.origin, toMouse.direction).collider != null)
            {
                Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
                for (int i = 0; i < l_shopUpgrades.Count; i++ )
                {
                    if (l_shopUpgrades[i].Upgrade == obj.gameObject && l_shopUpgrades[i].isBought == false)
                    {
                        confirmationBoxActive = true;
                        upgradeIndex = i;
                        confirmationBoxPos = obj.position;
                    }
                  
                }
            }
        }
    }
	// Update is called once per frame
	void Update () 
    {
        GoToMenu();
        UpgradeSelecter();
        if (confirmationBoxActive)
        {
            EnableBuyBox(confirmationBoxPos, upgradeIndex);
        }
	}
    
}
