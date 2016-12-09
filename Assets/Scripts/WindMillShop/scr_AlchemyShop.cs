using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;
using System;
using System.IO;
public struct InvetoryPotion
{
    public int m_goldCost;
    public string m_description;
    public int m_potionType;
    public bool m_unlocked;
    public bool m_bought;
    public GameObject m_obj;
    public Vector2 m_originalPos;
};
public class scr_AlchemyShop : MonoBehaviour 
{
    private Transform Inventory_BG;
    private Transform potion_info_BG;
    private SpriteRenderer potion_info_Picture;
    private Transform Equipment_BG;
    private Transform potion_0_0;
    private Transform canvas;
    private Text[] m_texts;
    private scr_FileHandler FH;
    public List<String> potionDescritions;
    [Space(20)]
    public List<GameObject> l_potionTypes;
    private List<InvetoryPotion> l_Inventory = new List<InvetoryPotion>();
    private int[] equipedPotions = new int[3];
    public Vector2 InventorySize;
    public Vector2 InvetoryItemSpace;
    [Space(20)]
    private GameObject[] equipmentSlots = new GameObject[3];
    private scr_EquipmentSlot[] ES = new scr_EquipmentSlot[3];
    private GameObject accept;
    private GameObject decline;


    private int m_gold;
    private Transform selectedTransform;
    private int selectedTransformIndex;
    private int equipedItemIndex;
    private bool potionSelected;
    private bool ableToMove;

	void Start () 
    {
        potionSelected = false;
        selectedTransform = null;
        ableToMove = true;
        Inventory_BG = transform.FindChild("Inventory_BG");
        potion_info_BG = GameObject.Find("potion_info_BG").transform;
        accept = GameObject.Find("potion_accept").gameObject;
        decline = GameObject.Find("potion_decline").gameObject;

        FH = GetComponent<scr_FileHandler>();
        m_gold = FH.GetGold();
        potion_info_Picture = GameObject.Find("potionPicture").GetComponent<SpriteRenderer>();
        potion_info_BG.gameObject.SetActive(false);
        accept.SetActive(false);
        decline.SetActive(false);
        potion_info_Picture.gameObject.SetActive(false);


        canvas = transform.FindChild("Canvas");
        m_texts = new Text[canvas.childCount];
        for (int i = 0; i < m_texts.Length; i++ )
        {
            m_texts[i] = canvas.GetChild(i).GetComponent<Text>();
            m_texts[i].enabled = false;
        }
        Equipment_BG = transform.FindChild("Equipment_BG");
        potion_0_0 = GameObject.Find("potion_0_0").transform;
        equipmentSlots = GameObject.FindGameObjectsWithTag("slot");
        for (int i = 0; i < equipmentSlots.Length; i++ )
        {
            ES[i] = equipmentSlots[i].GetComponent<scr_EquipmentSlot>();
        }
        
        InvetoryPotion[,] potions = FH.GetInventory();

        for (int i = 0; i < InventorySize.x; i++)
        {
            for (int y = 0; y < InventorySize.y; y++)
            {   

                InvetoryPotion IP = new InvetoryPotion();
                IP.m_potionType = potions[i,y].m_potionType;
                IP.m_description = potionDescritions[IP.m_potionType];
                IP.m_unlocked = potions[i,y].m_unlocked;
                GameObject obj = (GameObject)Instantiate(l_potionTypes[IP.m_potionType], Inventory_BG);
                obj.transform.position = new Vector2(potion_0_0.position.x + InvetoryItemSpace.x * i, potion_0_0.position.y - InvetoryItemSpace.y * y);
                IP.m_obj = obj;                
                IP.m_goldCost = UnityEngine.Random.Range(5, 15);
                IP.m_bought = potions[i, y].m_bought;
                IP.m_originalPos = obj.transform.position;
                l_Inventory.Add(IP);
                SpriteRenderer sr = IP.m_obj.GetComponent<SpriteRenderer>();
                
                if(!IP.m_bought)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.2f);
                if (!IP.m_unlocked)
                    sr.color = Color.black;

                //Index is LINE.
            }
        }
	}
    void UpdateGoldText()
    {
        m_texts[2].enabled = true;
        m_texts[2].text = "Gold: " + m_gold.ToString();
    }
    public void AddItemToEquipment(GameObject obj)
    {
        if (equipedItemIndex == equipedPotions.Length)
        {
            equipedItemIndex =0;
        }
        for (int i = 0; i < l_Inventory.Count; i++) // goes through all the inventory
        {
            if (obj == l_Inventory[i].m_obj)  // if the object exsists within the invenotry
            {
                equipedPotions[equipedItemIndex] = l_Inventory[i].m_potionType;
                equipedItemIndex += 1;
            }
        }



        FH.WriteEquipedPotions(equipedPotions);



        selectedTransformIndex = 0;
        potionSelected = false;
        selectedTransform = null;
    }
	void Update () 
    {

        UpdateGoldText();

        if (selectedTransform != null)
        {
            potionSelected = true;
        }
        if(potionSelected)    //selected transfom != null
        {
            if (l_Inventory[selectedTransformIndex].m_bought && ableToMove )  // Move and display info for bought objects
            {
                MovePotion(l_Inventory[selectedTransformIndex].m_obj.transform);
                DisplayPotionInfo(potionSelected, selectedTransformIndex, l_Inventory[selectedTransformIndex].m_bought);
            }
            else
            {
                DisplayPotionInfo(potionSelected, selectedTransformIndex, l_Inventory[selectedTransformIndex].m_bought);
            }
            if (Input.GetMouseButtonDown(0) && selectedTransform != null)
            {
                ResetPotionPos(selectedTransform.gameObject);
                ableToMove = false;
                selectedTransform = null;
                potionSelected = false;
            }
        }
        else  //Nothing is selected
        {
            Debug.Log("Nothing is selected");
            if (Input.GetMouseButton(0) == false)
            {  
               selectedTransform = SelectPotion();
            }
            DisplayPotionInfo(potionSelected, selectedTransformIndex, l_Inventory[selectedTransformIndex].m_bought);
        }
	}
    public void ResetPotionPos(GameObject obj)
    {
        for (int i = 0; i < ES.Length; i++)
        {
            if (ES[i].GetAttachedPotion() == obj && selectedTransform != null)
            {
                //selectedTransform.position = l_Inventory[selectedTransformIndex].m_originalPos;
                //ableToMove = false;
                //selectedTransform = null;
                //potionSelected = false;
                //selectedTransformIndex = 0;
            }
        }
        obj.transform.position = l_Inventory[selectedTransformIndex].m_originalPos;
    }

    void MovePotion(Transform obj)
    {
        if (Input.GetMouseButton(0))
        {
            Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePos = Input.mousePosition;
            mousePos = new Vector3(mousePos.x, mousePos.y, 6);
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            float y = objectPos.y;
            float x = objectPos.x;
            obj.position =  Vector3.MoveTowards(obj.position,new Vector3(x, y, 0), 3.0f * Time.deltaTime);
        }
        else if (!Input.GetMouseButton(0))
        {
            for(int i = 0; i < l_Inventory.Count; i++)
            {
                if(l_Inventory[i].m_obj.transform == obj)
                {
                    obj.position = l_Inventory[i].m_originalPos;
                }
            }
        }
    }

    void DisplayPotionInfo(bool active , int index, bool isBought)
    {
        potion_info_BG.gameObject.SetActive(active);
        accept.gameObject.SetActive(active);
        decline.gameObject.SetActive(active);
        potion_info_Picture.gameObject.SetActive(active);
        potion_info_Picture.sprite = l_potionTypes[l_Inventory[index].m_potionType].GetComponent<SpriteRenderer>().sprite;
        
        for (int i = 0; i < 2; i++)
        {
            m_texts[i].enabled = active;
        }

        m_texts[0].text = "Cost: " + l_Inventory[index].m_goldCost.ToString();
        m_texts[1].text = l_Inventory[index].m_description;

        if (Input.GetMouseButton(0))
        {
            Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layer_mask = LayerMask.GetMask("button");
            int potion_mask = LayerMask.GetMask("potion");
            if (Physics2D.Raycast(toMouse.origin, toMouse.direction, 999f, layer_mask))
            {
                Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
                if(obj.name == "potion_accept")
                {
                    for (int i = 0; i < l_Inventory.Count; i++)
                    {
                        if (l_Inventory[i].m_obj.transform == selectedTransform && l_Inventory[i].m_unlocked)
                        {
                            if (m_gold >= l_Inventory[i].m_goldCost && !l_Inventory[i].m_bought)
                            {
                                m_gold = m_gold - l_Inventory[i].m_goldCost;
                                InvetoryPotion inv = new InvetoryPotion();
                                inv = l_Inventory[i];
                                inv.m_bought = true;
                                SpriteRenderer sr = inv.m_obj.GetComponent<SpriteRenderer>();
                                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f);
                                selectedTransformIndex = i;
                                l_Inventory[i] = inv;

                                potionSelected = false;
                                selectedTransform = null;
                                FH.WriteGoldAmount(m_gold.ToString());
                                FH.WriteInventory(i, 1, 1, l_Inventory[i].m_potionType, l_Inventory[i].m_goldCost, l_Inventory[i].m_originalPos);
                                break;
                            }
                            else if (m_gold < l_Inventory[i].m_goldCost)
                            {
                                Debug.Log("cant afford");
                                break;
                            }
                        }
                    }
                }
                else if (obj.name == "potion_decline")
                {
                    potionSelected = false;
                    selectedTransform = null;
                }   
            }
            if (Physics2D.Raycast(toMouse.origin, toMouse.direction, 999f, potion_mask))
            {
                Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
                for (int i = 0; i < l_Inventory.Count; i++)
                {
                    if (l_Inventory[i].m_obj == obj.gameObject && l_Inventory[i].m_unlocked && selectedTransform == null)
                    {
                        selectedTransformIndex = i;
                        ableToMove = true;
                        selectedTransform = obj;
                    }
                }
                if(obj == null)
                {
                    Debug.Log("null");
                    potionSelected = false;
                    selectedTransform = null;
                    selectedTransformIndex = 0;
                }
            }
        }
    }
    Transform SelectPotion()
    {
        Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layer_mask = LayerMask.GetMask("potion");
        if (Physics2D.Raycast(toMouse.origin, toMouse.direction, 999f, layer_mask))
        {
            Transform obj = Physics2D.Raycast(toMouse.origin, toMouse.direction).transform;
            for (int i = 0; i < l_Inventory.Count; i++)
            {
                if (l_Inventory[i].m_obj == obj.gameObject && l_Inventory[i].m_unlocked && obj == selectedTransform)
                {
                    selectedTransformIndex = i;
                    Debug.Log("found object");
                    ableToMove = true;
                    return obj;
                }
            }
        }
        return null;
    }
}
