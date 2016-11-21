using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class scr_IngameUI : MonoBehaviour 
{
    private Text score;
    private Text scoreUntillNextMedal;
    private Text scoreMax;
    private Text remainingBags;
    private scr_winbagBehaviour WBH;
    private scr_GameManager GM;
    private scr_bagMovement BM;
    private Image UIBar;
    private RectTransform UIBarRect;
    private float m_multiplier;
	void Start () 
    {
        WBH = GameObject.FindGameObjectWithTag("win").GetComponent<scr_winbagBehaviour>();
        GM = GameObject.Find("GameManager").GetComponent<scr_GameManager>();
        BM = GameObject.FindGameObjectWithTag("bag").GetComponent<scr_bagMovement>();
        score = transform.Find("score").GetComponent<Text>();
        scoreUntillNextMedal = transform.Find("scoreUntillNextMedal").GetComponent<Text>();
        scoreMax = transform.Find("scoreMax").GetComponent<Text>();
        
        remainingBags = transform.Find("bagsleft").GetComponent<Text>();;
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
    //Show Medal instead of text for next medal;
	void Update () 
    {
        UpdateTexts();
        
	}
    void UpdateTexts()
    {
        score.text = "Points: " + WBH.GetScore().ToString();
        scoreMax.text = "Max Level Score: " + WBH.GetMaxScore().ToString() ;
        scoreUntillNextMedal.text = "Next Medal: " + CalculateRemainingParticlesForNextMedal().ToString();
        int bags = GM.GetRemainingBags() + 1;
        remainingBags.text = "Bags Left: " + bags.ToString();
    }
}
