using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class scr_winbagBehaviour : MonoBehaviour 
{
    private scr_GameManager GM;
    private scr_bagMovement BM;
    private scr_IngameSoundManager ISM;
    private scr_PotionEffects PE;
    private scr_obp pooler;
    private scr_FileHandler FH;
    private int collectedParticles;
    private int remaimingParticles;
    private int maxParticles;
    private int maxScore;
    private float timeSinceCollectedParticles;

    private float m_particle_1_score;
    private float m_particle_2_score;
    private float m_particle_3_score;
    private int m_score;

    private StreamReader reader;
    private StreamWriter writer;

	// Use this for initialization
	void Start () 
    {
        GM = GameObject.Find("GameManager").GetComponent<scr_GameManager>();
        BM = GameObject.FindGameObjectWithTag("bag").GetComponent<scr_bagMovement>();
        ISM = GM.GetComponent<scr_IngameSoundManager>();
        pooler = GameObject.FindGameObjectWithTag("pooler").GetComponent<scr_obp>();
        FH = GM.GetComponent<scr_FileHandler>();
        PE = GM.GetComponent<scr_PotionEffects>();

        collectedParticles = 0;
        m_particle_1_score = GM.GetParticleScore(0);
        m_particle_2_score = GM.GetParticleScore(1);
        m_particle_3_score = GM.GetParticleScore(2);

        //The actual amount of particles;
        maxParticles = GM.GetMaxWinParticles() * GM.GetMaxBagAmount();
        maxScore = maxParticles * (int)m_particle_2_score ;
	}
    void ChangeLevel()
    {
        if (Input.GetKey(KeyCode.A))
        {
            SaveLevelInfo(0, m_score, maxScore);
            SceneManager.LoadScene(Application.loadedLevel - 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            SaveLevelInfo(0, m_score, maxScore);
            SceneManager.LoadScene(Application.loadedLevel + 1);
        }
    }
    void SaveLevelInfo(int worldIndex , float p_levelScore, float p_levelMaxScore)
    {
        float goldAmount = GM.GetGold() * PE.GetGoldMultiplier();
        FH.WriteGoldAmount(goldAmount.ToString());

        bool p_completed;
        if(m_score > GM.GetMedalValue(0) * maxScore)
            p_completed = true;
        else
            p_completed = false;

        string fileDirectory;
        fileDirectory = "/world_" + worldIndex.ToString() + "_data.txt";
        fileDirectory = Application.dataPath + fileDirectory;
        if (!File.Exists(fileDirectory)) // If the file is there
        {
            int index = Application.loadedLevel;
            for (int u = 0; u < 10; u++)
            {
                System.IO.File.WriteAllText(fileDirectory, "0\n0\n0\n0\n0\n0\n0\n0\n0\n0\n ");
            }
            var lines = File.ReadAllLines(fileDirectory);
            lines[index] = Convert.ToInt32(p_completed).ToString() + ':' + p_levelScore.ToString() + ':' + p_levelMaxScore.ToString();
            File.WriteAllLines(fileDirectory, lines);
        }
        else
        {
            int index = Application.loadedLevel;
            var lines = File.ReadAllLines(fileDirectory);
            lines[index] = Convert.ToInt32(p_completed).ToString() + ':' + p_levelScore.ToString() + ':' + p_levelMaxScore.ToString();
            File.WriteAllLines(fileDirectory,lines);
        }
    }
	// Update is called once per frame
	void Update ()
    {
        CheckWinCondition();
        CheckLoseCondition();
        ChangeLevel();
	}
    public int GetMaxParticles()
    {
        return maxParticles;
    }
    public int GetCollectedParticles()
    {
        return collectedParticles;
    }
    void OnCollisionEnter2D(Collision2D colli)
    {
        if (colli.gameObject.tag == "particle")
        {
            if(colli.gameObject.name == "p1")
            {
                m_score = m_score + (int)m_particle_1_score;
            }
            else if (colli.gameObject.name == "p2")
            {
                m_score = m_score + (int)m_particle_2_score;
            }
            else if (colli.gameObject.name == "p3")
            {
                m_score = m_score + (int)m_particle_3_score;
            }
            collectedParticles += 1;
            colli.gameObject.SetActive(false);
            timeSinceCollectedParticles = 0;
            ISM.PlayCollectParticleSounds();
        }
    }
    public int GetScore()
    {
        return m_score;
    }
    public int GetMaxScore()
    {
        return maxScore;
    }
    void CheckLoseCondition()
    {
        timeSinceCollectedParticles = timeSinceCollectedParticles + Time.deltaTime;
        if(timeSinceCollectedParticles > 3 && GM.GetRemainingBags() == 0  && GameObject.FindGameObjectWithTag("bag") == null)
        {
            remaimingParticles = maxParticles - collectedParticles;  // how many is supposed to be left to collect
            if (remaimingParticles - pooler.GetIdleActiveParticles().Count < maxParticles * 0.5f * 5) // how many remaining particles - slow particles compared to bronze limit.
            {
                if (pooler.GetIdleActiveParticles().Count < 1)
                {
                    GM.CheckEndGameConditions();
                    SaveLevelInfo(0, m_score, maxScore);
                }
            }
        }
        //Timer when latest corn has arrivewd, if more than 5, do BIG CHECK :D:D::D:D
    }
    void CheckWinCondition()
    {
        if (m_score >= GM.GetMedalValue(3) * maxScore)
        {
            GM.SetEndGameState(EndGameState.master);
        }
        else if (m_score >= GM.GetMedalValue(2) * maxScore)
        {
            GM.SetEndGameState(EndGameState.gold);
        }
        else if (m_score >= GM.GetMedalValue(1) * maxScore)
        {
            GM.SetEndGameState(EndGameState.silver);
        }
        else if (m_score >= GM.GetMedalValue(0) * maxScore)
        {
            GM.SetEndGameState(EndGameState.bronze);
        }
    }
}
