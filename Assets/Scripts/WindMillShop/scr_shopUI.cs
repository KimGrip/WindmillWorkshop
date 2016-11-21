using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class scr_shopUI : MonoBehaviour 
{
    private List<Text> l_Texts;
    private Transform m_canvasTF;
    private Text Money; // change to currency name once its decided
    private Text m_currency;

    public float currency;
	// Use this for initialization
	void Start () 
    {
        m_canvasTF = transform.FindChild("Canvas");   
        Money = m_canvasTF.FindChild("Money").GetComponent<Text>();
        Money = FindTextName("Money");
        l_Texts = new List<Text>();
        l_Texts.Add(Money);
	}
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.W))
        {
            m_currency.text = "currency " + currency.ToString() + "!";
        }
	}
    public int GetCurrency()
    {
        return (int)currency;
    }
    public bool BuyUpdrade(float m_currency, float m_cost )
    {
        if(currency >= m_cost)
        {
            currency = m_currency - m_cost;
            return true;
        }
        else
        {
            Debug.Log("not enoughj money bnitch");
            return false;
        }
    }
    Text FindTextName(string name)
    {
        for (int i = 0; i < l_Texts.Count; i++)
        {
            if (l_Texts[i].name == name)
            {
                return l_Texts[i];
            }
        }
        return null;
    }
}
