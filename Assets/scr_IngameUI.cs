using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class scr_IngameUI : MonoBehaviour 
{
    public Text score;
    private Text scoreUntillNextMedal;
    private Text scoreMax;
    private Text remainingBags;
    private Text gold;

    private scr_winbagBehaviour WBH;
    private scr_GameManager GM;
    private scr_bagMovement BM;
    private scr_FileHandler FH;
    private Image UIBar;
    private RectTransform UIBarRect;
    private float m_multiplier;
    //private scr_FileHandler FH;
    private int[] activePotions = new int[3];
    private GameObject[] potionsLeft = new GameObject[3];
    private GameObject[] bagsLeft = new GameObject[3];
    private GameObject potionSelect;

    public List<Sprite> potionsSprites;

    private int selectedPotionIndex;

    void Awake()
    {
        WBH = GameObject.FindGameObjectWithTag("win").GetComponent<scr_winbagBehaviour>();

        GM = GameObject.Find("GameManager").GetComponent<scr_GameManager>();
        FH = GameObject.Find("GameManager").GetComponent<scr_FileHandler>();
    }
	void Start () 
    {
        //FH = GameObject.Find("GameManager").GetComponent<scr_FileHandler>();
        BM = GameObject.FindGameObjectWithTag("bag").GetComponent<scr_bagMovement>();
       
        score = transform.Find("score").GetComponent<Text>();
        scoreUntillNextMedal = transform.Find("scoreUntillNextMedal").GetComponent<Text>();
        scoreMax = transform.Find("scoreMax").GetComponent<Text>();
        remainingBags = transform.Find("bagsleft").GetComponent<Text>();;
        gold = transform.Find("gold").GetComponent<Text>();
        //bagsLeft[0] = GameObject.Find("Bag_Wheat1");
        //bagsLeft[1] = GameObject.Find("Bag_Wheat2");
        //bagsLeft[2] = GameObject.Find("Bag_Wheat3");

        //Searching the scene for GameObject 
        potionSelect = GameObject.Find("active0");

        for (int i = 0; i < bagsLeft.Length; i++)
        {
            bagsLeft[i] = GameObject.Find("bagWheat" + i.ToString());


           // Debug.Log("How many bags are left: " + GM.GetRemainingBags());
        //Finds active potions     
        }
        for (int i = 0; i < activePotions.Length; i++)
        {
            activePotions[i] = FH.GetEquipedPotions(i);
            Debug.Log(FH.GetEquipedPotions(i));
        }

        //SpriteRenderer sr = new SpriteRenderer();
        //sr = potionsLeft[i].GetComponent<SpriteRenderer>();
        //sr.sprite = potionsSprites[0];
        
        for (int i = 0; i < potionsLeft.Length; i++)
        {
            potionsLeft[i] = GameObject.Find("Potion" + i.ToString());
            potionsLeft[i].GetComponent<SpriteRenderer>().sprite = potionsSprites[activePotions[i]];
        }

        //for (int i = 0; i < potionsLeft.Length; i++)
        //{
        //    potionSelect[i] = GameObject.Find("active" + i.ToString());
        //    //potionSelect[i].gameObject.SetActive
        //    //potionsLeft[i].GetComponent<SpriteRenderer>().sprite
        //}
    }


    //    Bronze: 25 gold, 4k flour score.
    //    Silver: 50 gold, 5500 flour score.
    //    Gold: 100 gold, 7500 flour score.
    //    Master: 200 gold, 9000 flour score.

    //    Swapping a potion: 150 gold. TEST THIS.
    //    Unlocking base upgrade: Expensive, start at 1000. Double every new tier; 1000 -> 2k -> 4k etc… MAKE EM FUCKING WORK FOR IT.
    //    Cool upgrades at first rank and then every other rank, alternate with other buildings

    int CalculateRemainingParticlesForNextMedal()
    {
        if (WBH.GetScore() < GM.GetMedalValue(0) * WBH.GetMaxScore())  //Bronze
        {
            m_multiplier = WBH.GetMaxScore() * GM.GetMedalValue(0);
        }
        else if (WBH.GetScore() < GM.GetMedalValue(1) * WBH.GetMaxScore())   // Silver
        {
            m_multiplier = WBH.GetMaxScore() * GM.GetMedalValue(1);   
        }
        else if (WBH.GetScore() < GM.GetMedalValue(2) * WBH.GetMaxScore())   // gold
        {
            m_multiplier = WBH.GetMaxScore() * GM.GetMedalValue(2);
        }
        else if (WBH.GetScore() < GM.GetMedalValue(3) * WBH.GetMaxScore())  // master
        {
            m_multiplier = WBH.GetMaxScore() * GM.GetMedalValue(3);
        }
        float answer =  m_multiplier - WBH.GetScore();
        return (int)answer;
    }
    public int GetScoreText()
    {
        return WBH.GetScore();
    }
    //Show Medal instead of text for next medal;
	void Update () 
    {
        UpdateTexts();
        if (Input.GetKey(KeyCode.S))
            Debug.Log(FH.GetEquipedPotions(2));

        selectedPotionIndex = GM.GetRemainingBags();
        potionSelect.transform.position = potionsLeft[selectedPotionIndex].transform.position;


    }
    void UpdateTexts()
    {
        score.text = "Points: " + WBH.GetScore().ToString();
        scoreMax.text = "Max Level Score: " + WBH.GetMaxScore().ToString() ;
        scoreUntillNextMedal.text = "Next Medal: " + CalculateRemainingParticlesForNextMedal().ToString();
        int bags = GM.GetRemainingBags() + 1;
        remainingBags.text = "Bags Left: ";
        gold.text = GM.GetGold().ToString();
        int test = 0;
        test = bagsLeft.Length - bags;

        if (test != 0)
        {
            bagsLeft[test - 1].SetActive(false);
        }

        if (GM.GetRemainingBags() == 0 && GameObject.FindGameObjectWithTag("bag") == null) 
        {
            for (int i = 0; i < bagsLeft.Length; i++)
            {
                bagsLeft[i].SetActive(false);
            }
        }

        

    }
}
