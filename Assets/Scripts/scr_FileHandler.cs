﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class scr_FileHandler : MonoBehaviour 
{
    private string fileDirectory;
    private int LatestLevelIndex;
    private int goldAmount;
    private Vector2 resolution;
    private float SoundFXVolume;
    private float MusicVolume;
    private int WindowMode;
    private int[] equipedPotions = new int[3];
    private List<int> l_UpgradeStatus = new List<int>();
    private InvetoryPotion[,] Inventory = new InvetoryPotion[8, 4];
    
    void Awake()
    {
        fileDirectory = Application.dataPath + "/Settings.txt";
        if (!File.Exists(fileDirectory)) // If the file is there
        {
            CreateSettingsFile();
        }
        else if (File.Exists(fileDirectory))
        {
            LoadSettingsFromFile();
        }
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.S))
        {
            for(int i = 0; i < 8; i++)
            {
                for(int y = 0; y < 4; y++)
                {
                    Debug.Log(Inventory[i, y].m_unlocked);
                }
            }
        }
    }
    public void WriteToFile(int p_lineIdex, string data)
    {
        //1.upgrades int[]
        //2.gold int
        //3.soundFX float
        //4.MusicVolume float 
        //5.Resolution Vector2
        //6.WindowedMode int
        //7.last level Index int
        //8.Potions equiped
        //9. Upgares.
        TextWriter tw2 = new StreamWriter(fileDirectory);
        tw2.WriteLine(data, p_lineIdex);
    }
    public void WriteGoldAmount(string amount)
    {
        lineChanger("Gold: "+amount, fileDirectory, 2);
    }
    public void WriteLastLevel(string amount)
    {
        lineChanger("lvl: " + amount, fileDirectory, 7);
    }
    public void WriteInventory(int p_index,int unlocked, int bought, int potionType, int goldCost, Vector2 originalPositon)
    {
        lineChanger(unlocked.ToString() + ":" + bought.ToString() + ":" + potionType.ToString() + ":" + goldCost.ToString()
            + ":" + originalPositon.x.ToString() + ":" + originalPositon.y.ToString(), fileDirectory, p_index);
    }
    public void WriteEquipedPotions(int[] potions)
    {
        string line = "";
        for (int i = 0; i < potions.Length; i++ )
        {
            line += potions[i] + ",";
        }
        lineChanger("potions: " + line, fileDirectory, 8);
    }
    static void lineChanger(string newText, string fileName, int line_to_edit)
    {
        string[] arrLine = File.ReadAllLines(fileName);
        arrLine[line_to_edit - 1] = newText;
        File.WriteAllLines(fileName, arrLine);
    }
    public void LoadSettingsFromFile()
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/Settings.txt");

        MusicVolume = 0;
        goldAmount = 0;
        SoundFXVolume = 0;
        resolution = new Vector2(1920, 1080);
        LatestLevelIndex = 1;
        //1.upgrades int[]
        LoadUpgrades(reader, reader.ReadLine());
        //2.gold int
        goldAmount = LoadGold(reader, reader.ReadLine());
        //3.soundFX float
        SoundFXVolume = LoadSoundFX(reader, reader.ReadLine());
        //4.MusicVolume float 
        MusicVolume = LoadMusicVolume(reader, reader.ReadLine());
        //5.Resolution Vector2
        LoadResolution(reader, reader.ReadLine());
        //6.WindowedMode int
        WindowMode = LoadWindowMode(reader, reader.ReadLine());
        //7.last level Index int
        LatestLevelIndex = LoadLastLevelIndex(reader, reader.ReadLine());
        //8. EqiupedItems
        equipedPotions = LoadEquipedPotions(reader, reader.ReadLine());
        //9. Inventory
        Inventory = LoadInventory(reader, reader.ReadLine());


        reader.Close();
    }
    public List<int> GetUpgradeStatus()
    {
        return l_UpgradeStatus;
    }
    public int GetGold()
    {
        return goldAmount;
    }
    public float GetSoundFXVolume()
    {
        return SoundFXVolume;
    }
    public float GetMusicVolume()
    {
        return MusicVolume;
    }
    public int GetLastLevelIndex()
    {
        return LatestLevelIndex;
    }
    public int GetEquipedPotions(int index)
    {
        return equipedPotions[index];
    }
    public InvetoryPotion[,] GetInventory()
    {
        return Inventory;
    }
    int LoadWindowMode(StreamReader reader, string window)
    {
        string windowResult = System.Text.RegularExpressions.Regex.Replace(window, @"\D", "");
        int[] windowNumbers = new int[windowResult.Length];
        int finalBool = 0;

        for (int i = 0; i < windowResult.Length; i++)
        {
            finalBool = (int)Char.GetNumericValue(Convert.ToChar(windowResult.ElementAt(i)));
            windowNumbers[i] = finalBool;
        }
        for (int i = 0; i < windowNumbers.Length; i++)
        {
            finalBool = windowNumbers[i] * Convert.ToInt32(Math.Pow(10, windowNumbers.Length - i - 1));
        }
        WindowMode = finalBool;
        return WindowMode;
    }
    void LoadResolution(StreamReader reader, string resolution)
    {
        string ResResult = System.Text.RegularExpressions.Regex.Replace(resolution, @"\D", "");
        int[] resNumbers = new int[ResResult.Length];
    }
    int LoadLastLevelIndex(StreamReader reader, string level)
    {
        string levelResult = System.Text.RegularExpressions.Regex.Replace(level, @"\D", "");
        int[] levelNumbers = new int[level.Length];
        int finalLevel = 0;

        for (int i = 0; i < levelResult.Length; i++)
        {
            finalLevel = (int)Char.GetNumericValue(Convert.ToChar(levelResult.ElementAt(i)));
            levelNumbers[i] = finalLevel;
        }
        for (int i = 0; i < levelResult.Length; i++)
        {
            finalLevel = levelNumbers[i] * Convert.ToInt32(Math.Pow(10, levelResult.Length - i - 1));
        }
        LatestLevelIndex = finalLevel;
        LatestLevelIndex = 1;
        return LatestLevelIndex;
    }
    float LoadMusicVolume(StreamReader reader, string MusicFX)
    {
        string MusicFXResult = System.Text.RegularExpressions.Regex.Replace(MusicFX, @"\D", "");
        float[] MusicNumbers = new float[MusicFXResult.Length];
        int FinalSoundFX = 0;
        for (int i = 0; i < MusicFXResult.Length; i++)
        {
            FinalSoundFX = (int)Char.GetNumericValue(Convert.ToChar(MusicFXResult.ElementAt(i)));
            MusicNumbers[i] = FinalSoundFX;
        }
        for (int i = 0; i < MusicNumbers.Length; i++)
        {
            MusicVolume += MusicNumbers[i] * Convert.ToInt32(Math.Pow(10, MusicNumbers.Length - i - 1));
        }
        return MusicVolume;
    }
    float LoadSoundFX(StreamReader reader, string SoundFX)
    {
        string SoundFXResult = System.Text.RegularExpressions.Regex.Replace(SoundFX, @"\D", "");
        float[] FXnumbers = new float[SoundFXResult.Length];
        int FinalSoundFX = 0;
        for (int i = 0; i < SoundFXResult.Length; i++)
        {
            FinalSoundFX = (int)Char.GetNumericValue(Convert.ToChar(SoundFXResult.ElementAt(i)));
            FXnumbers[i] = FinalSoundFX;
        }
        for (int i = 0; i < FXnumbers.Length; i++)
        {
            SoundFXVolume += FXnumbers[i] * Convert.ToInt32(Math.Pow(10, FXnumbers.Length - i - 1));
        }
        return SoundFXVolume;
    }

    InvetoryPotion[,] LoadInventory(StreamReader reader, string line)
    {
        for(int x =0; x < 8; x++)
        {
            for( int y = 0; y < 4; y++)
            {
                char splitter = ':';
                string[] test = line.Split(splitter);
                string result = System.Text.RegularExpressions.Regex.Replace(line, @"\D", ""); // trims the string to only be numbers
                for (int i = 0; i < result.Length; i++)
                {
                    //gold and OG pos needs separe handflers since they can be multiple objects
                    

                    //


                    
                    

                    //


                    // OBS!!!
                    //REMOVE THIS
                    Inventory[x, y].m_unlocked = true;
                        //Convert.ToBoolean(Convert.ToInt32(result.ElementAt(0).ToString()));


                    //



                    Inventory[x, y].m_bought = Convert.ToBoolean(Convert.ToInt32(result.ElementAt(1).ToString()));
                    Inventory[x, y].m_potionType = Convert.ToInt32(result.ElementAt(2).ToString());
                    Inventory[x, y].m_goldCost = Convert.ToInt32(test[3].ToString());
                    Inventory[x, y].m_originalPos = new Vector2(Convert.ToSingle(test[4].ToString()), Convert.ToSingle(test[5].ToString()));
                    Debug.Log(Convert.ToInt32(test[2].ToString()));
                    //Debug.Log(Inventory[x, y].m_potionType);
                }
                line = reader.ReadLine();

            }
        }
        return Inventory;
    }
    int LoadGold(StreamReader reader, string gold)
    {
        string result = System.Text.RegularExpressions.Regex.Replace(gold, @"\D", "");
        int finalAmount = 0;
        int[] numbers = new int[result.Length];
        for (int i = 0; i < result.Length; i++)
        {
            finalAmount = (int)Char.GetNumericValue(Convert.ToChar(result.ElementAt(i)));
            numbers[i] = finalAmount;
        }
        for (int i = 0; i < numbers.Length; i++)
        {
            goldAmount += numbers[i] * Convert.ToInt32(Math.Pow(10, numbers.Length - i - 1));
        }
        return goldAmount;
    }
    void LoadUpgrades(StreamReader reader, string upgrades)
    {
        string result = System.Text.RegularExpressions.Regex.Replace(upgrades, @"\D", ""); // trims the string to only be numbers
        for (int i = 0; i < result.Length; i++)
        {
            l_UpgradeStatus.Add(0);
        }
        for (int y = 0; y < l_UpgradeStatus.Count; y++)  //Reads from the string of numbers into the status, 0 not bought, 1 bought.
        {
            l_UpgradeStatus[y] = (int)Char.GetNumericValue(result.ElementAt(y));
        }
    }

    int[] LoadEquipedPotions(StreamReader reader, string potions)
    {
        string result = System.Text.RegularExpressions.Regex.Replace(potions, @"\D", "");
        for (int i = 0; i < result.Length; i++)
        {
            equipedPotions[i] = (int)Char.GetNumericValue(result.ElementAt(i));
        }
        return equipedPotions;
    }
    void CreateSettingsFile()
    {
        TextWriter tw = new StreamWriter(fileDirectory);
        resolution = new Vector2(1920, 1080);
        WindowMode = 1;
        SoundFXVolume = 0.5f;
        MusicVolume = 0.5f;
        LatestLevelIndex = 1;
        goldAmount = 50;

        for (int i = 0; i < 11; i++)
        {
            l_UpgradeStatus.Add(0);
        }
        for (int i = 0; i < l_UpgradeStatus.Count; i++)
        {
            if (i != 0)
            {
                tw.Write("," + l_UpgradeStatus[i].ToString());
            }
            else
            {
                tw.Write("Upgrades: " + l_UpgradeStatus[i].ToString());
            }
        }
        tw.WriteLine("\r\nGold: " + goldAmount.ToString());
        tw.WriteLine("SoundFX: " + SoundFXVolume.ToString());
        tw.WriteLine("Music: " + MusicVolume.ToString());
        tw.WriteLine("Resolution: " + resolution.ToString());
        tw.WriteLine("WindowMode: " + WindowMode.ToString());
        tw.Write("lvl: " + LatestLevelIndex.ToString());

        for (int i = 0; i < equipedPotions.Length; i++ )
        {
            if (i != 0)
            {
                tw.Write("," + equipedPotions[i].ToString());
            }
            else
            {
                tw.Write("\r\npotions: " + equipedPotions[i].ToString());
            }
        }
        
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                tw.Write("\r\n:" + Convert.ToInt32(Inventory[x, y].m_unlocked).ToString() + ":" + Convert.ToInt32(Inventory[x, y].m_bought).ToString() + ":" +
                    Inventory[x, y].m_potionType.ToString() + ":" + Inventory[x, y].m_goldCost.ToString() + ":" + Inventory[x, y].m_originalPos.x.ToString() + ":" + Inventory[x, y].m_originalPos.y.ToString());
            }
        }

        tw.Close();
    }
}
