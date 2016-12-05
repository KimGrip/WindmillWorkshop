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
    private scr_FileHandler FS;
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
    private GameObject[] potionInfoButton = new GameObject[2];
    private int m_gold;
    private Transform selectedTransform;
    private int selectedTransformIndex;
    private int equipedItemIndex;
    private bool potionSelected;
    private bool ableToMove;

	void Start () 
    {
        m_gold = 500;
        potionSelected = false;
        selectedTransform = null;
        ableToMove = true;
        Inventory_BG = transform.FindChild("Inventory_BG");
        potion_info_BG = transform.FindChild("potion_info_BG");
        potionInfoButton[0] = potion_info_BG.FindChild("potion_accept").gameObject;
        potionInfoButton[1] = potion_info_BG.FindChild("potion_decline").gameObject;
        FS = GetComponent<scr_FileHandler>();
        potion_info_Picture = GameObject.Find("potionPicture").GetComponent<SpriteRenderer>();
        potion_info_BG.gameObject.SetActive(false);
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
        for (int i = 0; i < InventorySize.x; i++)
        {
            for (int y = 0; y < InventorySize.y; y++)
            {   
                //All of these variables need to be read in through text file,perhaps new one just for potions.
                InvetoryPotion IP = new InvetoryPotion();
                IP.m_unlocked = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
                IP.m_potionType = UnityEngine.Random.Range(0, 5);
                IP.m_goldCost = UnityEngine.Random.Range(5, 25);
                IP.m_bought = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
                IP.m_description = potionDescritions[IP.m_potionType];
                GameObject obj = (GameObject)Instantiate(l_potionTypes[IP.m_potionType], Inventory_BG);
                IP.m_obj = obj;

                obj.transform.position = new Vector2(potion_0_0.position.x + InvetoryItemSpace.x * i, potion_0_0.position.y - InvetoryItemSpace.y * y);
                IP.m_originalPos = obj.transform.position;
                l_Inventory.Add(IP);
                SpriteRenderer sr = IP.m_obj.GetComponent<SpriteRenderer>();
                
                if(!IP.m_bought)
                {
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.2f);
                }
                if (!IP.m_unlocked)
                {
                    sr.color = Color.black;
                }
            }
        }
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
        selectedTransformIndex = 0;
        potionSelected = false;
        selectedTransform = null;
    }

	void Update () 
    {
        if(Input.GetKey(KeyCode.A))
        {
            FS.WriteEquipedPotions(equipedPotions);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            for (int i = 0; i < 3; i++ )
            {
                Debug.Log(FS.GetEquipedPotions(i));
            }
        }
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
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("deselecting Potino");
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
                //  selectedTransform.position = l_Inventory[selectedTransformIndex].m_originalPos;
                //ableToMove = false;
                //selectedTransform = null;
                //potionSelected = false;
            }
        }
        Debug.Log("ResetPotionPos");
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
        potion_info_Picture.sprite = l_potionTypes[l_Inventory[index].m_potionType].GetComponent<SpriteRenderer>().sprite;
        
        for (int i = 0; i < m_texts.Length; i++)
        {
            m_texts[i].enabled = active;
        }
        for (int i = 0; i < potionInfoButton.Length; i++ )
        {
            potionInfoButton[i].SetActive(!isBought);
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
        Debug.Log("selectpotion");
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
